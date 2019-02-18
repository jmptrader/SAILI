using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SAILI.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;

namespace SAILI.Models
{
    public class SailiRepository
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        private string databaseConnection = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;

        public SailiRepository() { }

        public void SeedPostcode()
        {
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            const string Country = "Australia";
            string line = null;
            bool columnHeader = false;
            int count = 0;

            try
            {
                var serverPath =
                System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/postcodes.csv");

                StreamReader file = new System.IO.StreamReader(serverPath);

                while ((line = file.ReadLine()) != null)
                {
                    string[] temp = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                    if (!columnHeader)
                    {
                        columnHeader = true;
                        continue;
                    }
                    if(count < 8920)
                    {
                        count++;
                        continue;
                    }

                    DefaultConnection.Postcodes.Add(new Postcode()
                    {
                        postcode = temp[(int)PostcodeEnum.Postcode],
                        Locality = temp[(int)PostcodeEnum.Locality],
                        State = temp[(int)PostcodeEnum.State],
                        Country = Country
                    });
                    DefaultConnection.SaveChanges();
                    continue;
                }
                file.Close();
            }
            catch
            {

            }
        }
/*============================= END OF SEED POSTCODE=============================*/

        public  void SendEmail()
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("s3386614@student.rmit.edu.au", "Kiet Lam"));
            
                // From
                mailMsg.From = new MailAddress("kitkat1375@hotmail.com", "Kiet");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "subject";
                string text = "text body";
                string html = @"<p>html body</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_6675a52d59c6adb5d183927445f466d1@azure.com", "caesar55Bc$");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {

            }
        }

        public string CheckStatus(string UserID)
        {
            string ownerid = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spCheckOwner", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        ownerid = sqlReader["OwnerID"].ToString();
                    }
                }
                con.Close();
            }
            return ownerid;
        }

        public string[] RemoveEmptyEntries(string[] temp)
        {
            var trim = " ";
            for(int i = 1; i < temp.Length; i++)
            {
                switch(i)
                {
                    case 1:
                        temp[i] = temp[i].TrimStart(trim.ToCharArray());
                        break;
                    case 2: 
                        temp[i] = temp[i].TrimStart(trim.ToCharArray());
                        break;
                }
                
            }
            return temp;
        }

        public TraderAccount CheckTradingAccount(string OwnerID)
        {
            TraderAccount trader = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spTradingAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@OwnerID", OwnerID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        trader = new TraderAccount();
                        trader.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                        trader.OwnerID = sqlReader["OwnerID"].ToString();
                        trader.CreationDate = Convert.ToDateTime(sqlReader["CreationDate"].ToString());
                        trader.Balance = Convert.ToDecimal(sqlReader["Balance"].ToString());
                    }
                }
                con.Close();
            }
            return trader;
        }

        public Vector<Postcode> GetLocality(string postcode)
        {
            Vector<Postcode> localities = new Vector<Postcode>();
            Postcode model = null;


            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetLocality", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Postcode", postcode));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        model = new Postcode();
                        model.PostcodeID = Convert.ToInt32(sqlReader["PostcodeID"]);
                        model.postcode = sqlReader["postcode"].ToString();
                        model.Locality = sqlReader["Locality"].ToString();
                        model.State = sqlReader["State"].ToString();
                        model.Country = sqlReader["Country"].ToString();
                        localities.Add(model);
                    }
                }
                con.Close();
            }
            return localities;
        }

        public Owner GetOwner(string UserID)
        {
            Owner owner = null;
            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetOwner", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        owner = new Owner();
                        owner.OwnerID = sqlReader["OwnerID"].ToString();
                        owner.UserID = UserID;
                        owner.FirstName = sqlReader["FirstName"].ToString();
                        owner.LastName = sqlReader["LastName"].ToString();
                        owner.MiddleName = sqlReader["MiddleName"].ToString();
                        owner.DOB = sqlReader["DOB"].ToString();
                        owner.AddressNumber = sqlReader["AddressNumber"].ToString();
                        owner.AddressName = sqlReader["AddressName"].ToString();
                        owner.PostcodeID = sqlReader["PostcodeID"].ToString();
                    }
                }
                con.Close();
            }
            return owner;
        }

        public void InsertOwner(Owner model)
        {

            using (SqlConnection conn = new SqlConnection(databaseConnection))
            {
                using (SqlCommand comd = new SqlCommand("spInsertOwner", conn))
                {
                    comd.CommandType = CommandType.StoredProcedure;
                    comd.Parameters.Add("@OwnerID", SqlDbType.VarChar).Value = model.OwnerID;
                    comd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = model.UserID;
                    comd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = model.FirstName;
                    comd.Parameters.Add("@MiddeleName", SqlDbType.VarChar).Value = model.MiddleName;
                    comd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = model.LastName;
                    comd.Parameters.Add("@DOB", SqlDbType.VarChar).Value = model.DOB;
                    comd.Parameters.Add("@AddressNumber", SqlDbType.VarChar).Value = model.AddressNumber;
                    comd.Parameters.Add("@AddressName", SqlDbType.VarChar).Value = model.AddressName;
                    comd.Parameters.Add("@Postcode", SqlDbType.VarChar).Value = model.PostcodeID;
                    conn.Open();
                    comd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public Company GetCompany(Company company, string symbol)
        {
            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetCompany", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Symbol", symbol));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        company = new Company();
                        company.Symbol = sqlReader["Symbol"].ToString();
                        company.CompanyName = sqlReader["CompanyName"].ToString();
                        company.SectorID = Convert.ToInt32(sqlReader["SectorID"].ToString());
                        company.IsDeleted = Convert.ToBoolean(sqlReader["ISdeleted"].ToString());
                    }
                }
                con.Close();
            }
            return company;
        } 

        public void SeedCompany()
        {
            char[] delimiter = { ',', '\t', '\r', '\n', '"', '/', '{', '}' };
            string line = null;
            bool columnHeader = false;

            try
            {
                var serverPath =
                System.Web.Hosting.HostingEnvironment.MapPath("~/Resources/companylist.csv");

                StreamReader file = new System.IO.StreamReader(serverPath);

                while ((line = file.ReadLine()) != null)
                {
                    string[] temp = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                    if (!columnHeader)
                    {
                        columnHeader = true;
                        continue;
                    }
                    DefaultConnection.Companies.Add(new Company()
                    {
                        Symbol = temp[0],
                        CompanyName = temp[1],
                        SectorID = 0,
                        IsDeleted = false
                    }); 

                    DefaultConnection.SaveChanges();
                    continue;
                } 

                file.Close();
            }
            catch
            {

            }
        }

        public string GetAllSymbols()
        {
            string symbols = null;
            int count = 0;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetAllSymbols", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        count++;
                        if(count == 10)
                        {
                            symbols += sqlReader["Symbol"];
                            break;
                        }
                        else
                            symbols += sqlReader["Symbol"] + "+";
                    }
                }
                con.Close();
            }
            return symbols;
        }

        public List<Listings> GetCompanyNames(List<Listings> listings)
        {
            Vector<Company> companies = new Vector<Company>();
            Company company = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetAllSymbols", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        company = new Company();
                        company.Symbol = sqlReader["Symbol"].ToString();
                        company.CompanyName = sqlReader["CompanyName"].ToString();
                        companies.Add(company);
                    }
                }
                con.Close();
            }
            foreach (var list in listings)
            {
                if (list == null)
                    break;

                foreach (var item in companies)
                {
                    if (item == null)
                        break;
                    if (item.Symbol.Equals(list.Symbol))
                        list.CompanyName = item.CompanyName;
                }
            }
            return listings;
        }
        public string GetCompanyName(string symbol)
        {
            string companyName = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetCompany", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Symbol", symbol));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        companyName = sqlReader["CompanyName"].ToString();
                        break;
                    }
                }
                con.Close();
            }
            return companyName;
        }

        public Buy Finalizebuy(Buy model, string userID)
        {
            Owner owner = null;
            TraderAccount account = null;
            Portfolio portfolio = null;
            List<Portfolio> portfolios = new List<Portfolio>();
            int last = 0;


            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetOwner", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID", userID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        owner = new Owner();
                        owner.OwnerID = sqlReader["OwnerID"].ToString();
                        owner.UserID = sqlReader["UserID"].ToString();
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spTradingAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@OwnerID", owner.OwnerID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        account = new TraderAccount();
                        account.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                        account.OwnerID = sqlReader["OwnerID"].ToString();
                        account.CreationDate = Convert.ToDateTime(sqlReader["CreationDate"].ToString());
                        account.Balance = Convert.ToDecimal(sqlReader["Balance"].ToString());
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetPortfolio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TradingAccountID", account.TradingAccountID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        portfolio = new Portfolio();
                        portfolio.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        portfolio.TradingAccountID= sqlReader["TradingAccountID"].ToString();
                    }
                }
                con.Close();
            }
            if(portfolio == null)
            {
                using (SqlConnection con = new SqlConnection(databaseConnection))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetLastPortfolio", con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataReader sqlReader = cmd.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            last = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        }
                    }
                    con.Close();
                }

                DefaultConnection.Portfolios.Add(new Portfolio()
                {
                    PortfolioID = last + 1,
                    TradingAccountID = account.TradingAccountID
                });
                DefaultConnection.SaveChanges();
                model.PortfolioId = last + 1;
                DefaultConnection.Buys.Add(model);
                DefaultConnection.SaveChanges();
                account.Balance = account.Balance - model.TransactionAmount;
                DefaultConnection.Entry(account).State = EntityState.Modified;
                DefaultConnection.SaveChanges();
            }
            else
            {
                model.PortfolioId = portfolio.PortfolioID;
                DefaultConnection.Buys.Add(model);
                DefaultConnection.SaveChanges();
                account.Balance = account.Balance - model.TransactionAmount;
                DefaultConnection.Entry(account).State = EntityState.Modified;
                DefaultConnection.SaveChanges();
            }

            return model;
        }

        public Portfolio GetPortfolio(string tradingAccountID)
        {
            Portfolio portfolio = new Portfolio();
            Buy buy = null;
            Sell sell = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetPortfolio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("TradingAccountID", tradingAccountID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        portfolio.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        portfolio.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                    }
                }
                con.Close();
            }

            portfolio.Buys = new List<Buy>();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetBuys", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PortfolioID", portfolio.PortfolioID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        if (Convert.ToInt32(sqlReader["Quantity"].ToString()) > 0)
                        {
                            buy = new Buy();
                            buy.BuyID = Convert.ToInt32(sqlReader["BuyID"].ToString());
                            buy.TradeDate = Convert.ToDateTime(sqlReader["TradeDate"].ToString());
                            buy.PortfolioId = Convert.ToInt32(sqlReader["PortfolioId"].ToString());
                            buy.TickerSymbol = sqlReader["TickerSymbol"].ToString();
                            buy.Quantity = Convert.ToInt32(sqlReader["Quantity"].ToString());
                            buy.PurchasePrice = Convert.ToDecimal(sqlReader["PurchasePrice"].ToString());
                            buy.TransactionAmount = Convert.ToDecimal(sqlReader["TransactionAmount"].ToString());
                            buy.TransactionCost = Convert.ToDecimal(sqlReader["TransactionCost"].ToString());
                        }

                    }
                }
                con.Close();
            }

            portfolio.Sells = new List<Sell>();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetSells", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PortfolioID", portfolio.PortfolioID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        sell = new Sell();
                        sell.SellID = Convert.ToInt32(sqlReader["SellID"].ToString());
                        sell.TradeDate = Convert.ToDateTime(sqlReader["TradeDate"].ToString());
                        sell.BuyID = Convert.ToInt32(sqlReader["BuyID"].ToString());
                        sell.TickerSymbol = sqlReader["TickerSymbol"].ToString();
                        sell.Quantity = Convert.ToInt32(sqlReader["Quantity"].ToString());
                        sell.PurchasePrice = Convert.ToDecimal(sqlReader["PurchasePrice"].ToString());
                        sell.SoldPrice = Convert.ToDecimal(sqlReader["SoldPrice"].ToString());
                        sell.TransactionAmount = Convert.ToDecimal(sqlReader["TransactionAmount"].ToString());
                        sell.TransactionCost = Convert.ToDecimal(sqlReader["TransactionCost"].ToString());
                        sell.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());

                        portfolio.Sells.Add(sell);
                    }
                }
                con.Close();
            }
            return portfolio;
        }

        public PortfolioViewModel GetPortfolioVodelModel(string tradingAccountID)
        {
            PortfolioViewModel portfolioModel = new PortfolioViewModel();
            BuyViewModel buy = null;
            SellViewModel sell = null;

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetPortfolio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("TradingAccountID", tradingAccountID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        portfolioModel.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        portfolioModel.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                    }
                }
                con.Close();
            }

            portfolioModel.Buys = new List<BuyViewModel>();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetBuys", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PortfolioID", portfolioModel.PortfolioID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        if(Convert.ToInt32(sqlReader["Quantity"].ToString()) > 0)
                        {
                            buy = new BuyViewModel();
                            buy.BuyID = Convert.ToInt32(sqlReader["BuyID"].ToString());
                            buy.TradeDate = Convert.ToDateTime(sqlReader["TradeDate"].ToString());
                            buy.PortfolioId = Convert.ToInt32(sqlReader["PortfolioId"].ToString());
                            buy.TickerSymbol = sqlReader["TickerSymbol"].ToString();
                            buy.Quantity = Convert.ToInt32(sqlReader["Quantity"].ToString());
                            buy.PurchasePrice = Convert.ToDecimal(sqlReader["PurchasePrice"].ToString());
                            buy.TransactionAmount = Convert.ToDecimal(sqlReader["TransactionAmount"].ToString());
                            buy.TransactionCost = Convert.ToDecimal(sqlReader["TransactionCost"].ToString());
                            buy.CompanyName = GetCompanyName(buy.TickerSymbol);

                            portfolioModel.Buys.Add(buy);
                        }

                    }
                }
                con.Close();
            }

            portfolioModel.Sells = new List<SellViewModel>();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetSells", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@PortfolioID", portfolioModel.PortfolioID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        sell = new SellViewModel();
                        sell.SellID = Convert.ToInt32(sqlReader["SellID"].ToString());
                        sell.TradeDate = Convert.ToDateTime(sqlReader["TradeDate"].ToString());
                        sell.BuyID = Convert.ToInt32(sqlReader["BuyID"].ToString());
                        sell.TickerSymbol = sqlReader["TickerSymbol"].ToString();
                        sell.Quantity = Convert.ToInt32(sqlReader["Quantity"].ToString());
                        sell.PurchasePrice = Convert.ToDecimal(sqlReader["PurchasePrice"].ToString());
                        sell.SoldPrice = Convert.ToDecimal(sqlReader["SoldPrice"].ToString());
                        sell.TransactionAmount = Convert.ToDecimal(sqlReader["TransactionAmount"].ToString());
                        sell.TransactionCost = Convert.ToDecimal(sqlReader["TransactionCost"].ToString());
                        sell.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        sell.CompanyName = GetCompanyName(sell.TickerSymbol);

                        portfolioModel.Sells.Add(sell);
                    }
                }
                con.Close();
            }
            return portfolioModel;
        }

        public TraderAccount GetTrader(string ownerID)
        {
            TraderAccount trader = new TraderAccount();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spGetTraderAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@OwnerID", ownerID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        trader = new TraderAccount();
                        trader.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                        trader.OwnerID = sqlReader["OwnerID"].ToString();
                        trader.CreationDate = Convert.ToDateTime(sqlReader["CreationDate"].ToString());
                        trader.Balance = Convert.ToDecimal(sqlReader["Balance"].ToString());
                    }
                }
                con.Close();
            }
            return trader;
        }

        public string AppendSymbols(List<Company> companies)
        {
            string append = null;
            int count = 0;
            foreach(var company in companies)
            {
                count++;
                if(count == companies.Count){
                    append += company.Symbol;
                }
                else{
                    append += company.Symbol + "+";
                }

            }
            return append;
        }

        public List<SortWinner> GetTopThree(ref List<TraderAccount> traders, ref List<Owner> owners, ref List<SortWinner> winners)
        {
            Owner owner = null;
            TraderAccount trader = null;
            List<Buy> buys = new List<Buy>();
            List<Sell> sells = new List<Sell>();
            List<OlympianBoard> board = new List<OlympianBoard>();
            Portfolio portfolio = null;
            List<Portfolio> portfolios = new List<Portfolio>();
            List<PortfolioViewModel> portfolioView = new List<PortfolioViewModel>();

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spWildcardOwner", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        owner = new Owner();
                        owner.OwnerID = sqlReader["OwnerID"].ToString();
                        owner.UserID = sqlReader["UserID"].ToString();
                        owner.FirstName = sqlReader["FirstName"].ToString();
                        owner.MiddleName = sqlReader["MiddleName"].ToString();
                        owner.LastName = sqlReader["LastName"].ToString();
                        owner.DOB = sqlReader["DOB"].ToString();
                        owner.AddressNumber = sqlReader["AddressNumber"].ToString();
                        owner.AddressName = sqlReader["AddressName"].ToString();
                        owner.PostcodeID = sqlReader["PostcodeID"].ToString();
                        owners.Add(owner);
                    }
                }
                con.Close();
            }


            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spWildcardPortfolio", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        portfolio = new Portfolio();
                        portfolio.PortfolioID = Convert.ToInt32(sqlReader["PortfolioID"].ToString());
                        portfolio.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                        portfolios.Add(portfolio);
                    }
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(databaseConnection))
            {
                using (SqlCommand cmd = new SqlCommand("spWildcardTraderAccount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        trader = new TraderAccount();
                        trader.TradingAccountID = sqlReader["TradingAccountID"].ToString();
                        trader.OwnerID = sqlReader["OwnerID"].ToString();
                        trader.CreationDate = Convert.ToDateTime(sqlReader["CreationDate"].ToString());
                        trader.Balance = Convert.ToDecimal(sqlReader["Balance"].ToString());
                        traders.Add(trader);
                    }
                }
                con.Close();
            }

            List<PortfolioViewModel> PortfolioModelList = new List<PortfolioViewModel>();
            PortfolioViewModel portfolioViewModel = null;

            foreach (var trade in traders)
            {
                portfolioViewModel = null;
                portfolioViewModel = GetPortfolioVodelModel(trade.TradingAccountID);
                DataSerializer<string>.GetCurrentPerformance(ref portfolioViewModel);
                PortfolioModelList.Add(portfolioViewModel);
            }

            foreach(var item in PortfolioModelList)
            {
                SortWinner winner = new SortWinner();
                winner.TradingAcountID = item.TradingAccountID;
                winner.PortfolioID = item.PortfolioID;
                winners.Add(winner);
            }

            foreach(var winner in winners)
            {
                foreach(var item in PortfolioModelList)
                {
                    foreach(var items in item.Buys)
                    {
                        if(winner.PortfolioID == items.PortfolioId)
                        {
                            winner.Amount += items.Performance;
                            winner.Amount += items.TransactionAmount;
                            winner.Amount = winner.Amount - items.TransactionCost;
                            winner.Amount = Math.Round(winner.Amount, 2);
                            items.PortfolioId = 0;
                        }
                    }
                }
            }


            foreach (var winner in winners)
            {
                foreach(var trade in traders)
                {
                    if(trade.TradingAccountID.Equals(winner.TradingAcountID))
                    {
                        winner.OwnerID = trade.OwnerID;
                        winner.Amount += trade.Balance;
                        break;
                    }
                }
            }

            foreach (var itemOwner in owners)
            {
                foreach(var itemWinner in winners)
                {
                    if(itemWinner.OwnerID.Equals(itemOwner.OwnerID))
                    {
                        itemWinner.Name = itemOwner.FirstName + " " + itemOwner.LastName;
                        break;
                    }
                }
            }

            for(int i = 0; i < winners.Count; i++)
            {
                for(int j = 0; j < winners.Count; j++)
                {
                    if(winners[i].Amount > winners[j].Amount)
                    {
                        decimal tempAmount = winners[i].Amount;
                        string tradeIdTemp = winners[i].TradingAcountID;
                        int portIdTemp = winners[i].PortfolioID;
                        string ownerIdTemp = winners[i].OwnerID;
                        string nameTemp = winners[i].Name;
                        winners[i].Amount = winners[j].Amount;
                        winners[i].TradingAcountID = winners[j].TradingAcountID;
                        winners[i].PortfolioID = winners[j].PortfolioID;
                        winners[i].OwnerID = winners[j].OwnerID;
                        winners[i].Name = winners[j].Name;
                        winners[j].Amount = tempAmount;
                        winners[j].TradingAcountID = tradeIdTemp;
                        winners[j].PortfolioID = portIdTemp;
                        winners[j].OwnerID = ownerIdTemp;
                        winners[j].Name = nameTemp;
                    }
                }

            }
            return winners;
        }
    }
}