using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SAILI.Models
{
    public class EncryptionServices
    {
        /*========================== Encryption SAILI ================================ */

       


        public Owner EncryptOwner (Owner model)
        {
            String SaltDOB = null;
            String SaltOwnerID = null;

            SaltDOB = GetSalt();
            SaltOwnerID = GetSalt();

            model.OwnerID = GenerateAccount();
            model.OwnerID = Encrypt(model.OwnerID, SaltOwnerID);
            model.DOB = Encrypt(model.DOB, SaltDOB);
            InsertSaltOwner(SaltDOB, model.UserID);

            return model;
        }

        public TraderAccount EncryptTraderAccount(TraderAccount model)
        {
            String SaltTraderAccount = null;
            String SaltOwnerID = null;

            SaltTraderAccount = GetSalt();
            SaltOwnerID = GetSalt();

            model.TradingAccountID = GenerateAccount();
            model.TradingAccountID= Encrypt(model.TradingAccountID, SaltTraderAccount);

            return model;
        }

        //==== Generate Random Number for Account ============================//

        private static string GenerateAccount()
        {
            Random random = new System.Random();
            int value = random.Next(0, 1000000); //returns integer of 0-1000000

            var byteArray = new byte[256];
            random.NextBytes(byteArray);//fill with random bytes
            return random.ToString();
        }

//=========== Insert Salt to EncryptUserAccount ======================//
// Done firstly before return encrypted model ========================//

        private void InsertSaltOwner(string SaltDBO, string UserID)
        {
            string databaseConnection = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(databaseConnection))
            {
                using (SqlCommand comd = new SqlCommand("spInsertSaltOwner", conn))
                {
                    comd.CommandType = CommandType.StoredProcedure;
                    comd.Parameters.Add("@SaltDBO", SqlDbType.VarChar).Value = SaltDBO;
                    comd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserID;
                    conn.Open();
                    comd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        } 

        //==== Get salt with cryptographically strong byte values ===========//

        private string GetSalt()
        {
            //define min and max salt size
            int minSaltSize = 4;
            int maxSaltSize = 8;

            string salt = null;

            //Generate random number for the size of the salt
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            byte[] saltBytes = new byte[saltSize];

            //Initialize a random number generator
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            //Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);

            foreach (byte x in saltBytes)
            {
                salt += x.ToString();
            }

            return salt;
        }

        public string Encrypt(string inText, string key)
        {
            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    inText = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return inText;
        }

        public string Decrypt(string cryptTxt, string key)
        {
            cryptTxt = cryptTxt.Replace(" ", "+");
            byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    cryptTxt = Encoding.Unicode.GetString(mStream.ToArray());
                }
            }
            return cryptTxt;
        }

        /*========================= End of EncryptUserAccount ========================== */
    }
}