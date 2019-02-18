using SAILI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace SAILI.Controllers
{
    [Authorize]
    public class OwnerController : Controller
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        // GET: Owner/Details/5
        public ActionResult Details(string UserID)
        {
            var owner = DefaultConnection.Users.Find(UserID);
            return View(owner);
        }

        public ActionResult CheckStatusOwner()
        {
            string userId = User.Identity.GetUserId();
            if(User.IsInRole("Administrator"))
            {
                return RedirectToAction("ResourceIndex", "Account");
            }

            SailiRepository repository = new SailiRepository();
            string ownerid = repository.CheckStatus(User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(ownerid))
            {
                return RedirectToAction("CheckStatusTrader", "TraderAccount", new
                {
                    OwnerID = ownerid
                });
            }
            else
            {
                return RedirectToAction("Create", new
                {
                    userId = User.Identity.GetUserId()
                });
            }
        }

        [HttpPost]
        public JsonResult GetPost(string post)
        {
            Postcode postcode = new Postcode();
            List<Postcode> listing = new List<Postcode>();

            string cs = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;
            using (SqlConnection con = new System.Data.SqlClient.SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("spGetLocality", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@postcode";
                parameter.Value = post;

                cmd.Parameters.Add(parameter);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    postcode = new Postcode();
                    postcode.PostcodeID = Convert.ToInt32(rdr["postcodeID"]);
                    postcode.postcode = rdr["postcode"].ToString();
                    postcode.Locality = rdr["Locality"].ToString();
                    postcode.State = rdr["State"].ToString();
                    postcode.Country = rdr["Country"].ToString();
                    listing.Add(postcode);
                }

                rdr.Close();
            }

            return Json(listing, JsonRequestBehavior.AllowGet);
        }

        // GET: Owner/Create
        public ActionResult Create(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(!userId.Equals(User.Identity.GetUserId()))
            {
                DataSerializer<string>.SecurityPriorityNumberOne(User.Identity.GetUserId());
            }
                
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner model)
        {
            bool check = false;

            if (model.FirstName == null)
                check = true;
            if (model.MiddleName == null)
                check = true;
            if (model.LastName == null)
                check = true;
            if (model.DOB == null)
                check = true;
            if (model.AddressNumber == null)
                check = true;
            if (model.AddressName == null)
                check = true;
            if (model.PostcodeID == null)
                check = true;
            if (check)
                return View(model);


            char[] delimiter = { ',' };
            string[] temp = model.PostcodeID.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            EncryptionServices encryptionService = new EncryptionServices();
            SailiRepository repository = new SailiRepository();
            Vector<Postcode> Locality = new Vector<Postcode>();
            model.UserID = User.Identity.GetUserId();
            model = encryptionService.EncryptOwner(model);

            temp = repository.RemoveEmptyEntries(temp);
            string post = temp[0];
            Locality = repository.GetLocality(post);

            try
            {
                foreach (var item in Locality)
                {
                    if (item == null)
                        break;

                    if(item.postcode.Equals(temp[0].ToString()) && 
                        item.Locality.Equals(temp[1].ToString()))
                    {
                        model.PostcodeID = item.PostcodeID.ToString();
                    }
                }
                DefaultConnection.Owners.Add(new Owner()
                {
                    OwnerID = model.OwnerID,
                    UserID = model.UserID,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,
                    DOB = model.DOB,
                    AddressNumber = model.AddressNumber,
                    AddressName = model.AddressName,
                    PostcodeID = model.PostcodeID
                });
                 DefaultConnection.SaveChanges();


                return RedirectToAction("CheckStatusTrader", "TraderAccount", new
                {
                    OwnerID = model.OwnerID.ToString()
                });
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Owner/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Owner/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Owner/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Owner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
