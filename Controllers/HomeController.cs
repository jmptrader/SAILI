
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAILI.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using SAILI;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;
using System.IO;
using System.Xml;
using System.Web.Helpers;
using System.Text;

namespace SAILI.Controllers
{

    public class HomeController : Controller
    {

        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();

        public ActionResult Index()
        {
            var model = DefaultConnection.Homes.Find(1);

            return View(model);
        }

        public ActionResult AdminIndex()
        {
            return View(DefaultConnection.Homes.ToList());
        }

        [HttpPost]
        public JsonResult GetLocality(string post)
        {

            Postcode postcode = new Postcode();
            List<Postcode> listing = new List<Postcode>();

            string cs = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
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
                    listing.Add(new Postcode()
                    {
                        PostcodeID = Convert.ToInt32(rdr["PostcodeID"].ToString()),
                        postcode = rdr["postcode"].ToString(),
                        Locality = rdr["Locality"].ToString(),
                        State = rdr["State"].ToString(),
                        Country = rdr["Country"].ToString()
                    });
                }

                rdr.Close();
            }

            return Json(listing, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Help()
        {
            var model = DefaultConnection.Homes.Find(1);
            return View(model);
        }


        public ActionResult EmailForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailForm(EmailForm model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} at "+model.FromEmail+"</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("sailiemailservices@gmail.com"));  // replace with valid value 
                message.From = new MailAddress(model.FromEmail);  // replace with valid value
                message.Subject = model.Title;
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "azure_1deac5e7ef5ee3193eccd188f32361f5@azure.com",  // replace with valid value
                        Password = "caesar55Bc$"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.sendgrid.net";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }


        public ActionResult Sent()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Home model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                DefaultConnection.Homes.Add(new Home()
                {
                    About = model.About,
                    HelpName = model.HelpName,
                    HelpPhoneNumber = model.HelpPhoneNumber,
                    HelpEmail = model.HelpEmail
                });
                DefaultConnection.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Details(int id)
        {
            var homeDetails = DefaultConnection.Homes.Where(h => h.HomeID == id).FirstOrDefault();

            if (homeDetails == null)
            {
                return HttpNotFound();
            }

            return View(homeDetails);
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var home = DefaultConnection.Homes.Where(c => c.HomeID == id).FirstOrDefault();
            return View(home);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Home model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                TryUpdateModel(model);
                DefaultConnection.Entry(model).State = EntityState.Modified;
                DefaultConnection.SaveChanges();
                return RedirectToAction("AdminIndex", model.HomeID);
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult CloseWindow()
        {
            ViewData["ShouldClose"] = true;

            return View();
        }

    }
}