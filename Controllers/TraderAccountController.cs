using SAILI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace SAILI.Controllers
{
    public class TraderAccountController : Controller
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        private SailiRepository repository = new SailiRepository();
        private EncryptionServices encryptionServices = new EncryptionServices();
        // GET: TraderAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: TraderAccount/Details/5
        public ActionResult Details(string TradingAccountID)
        {
            var trader = DefaultConnection.TraderAccounts.Find(TradingAccountID);
            if(trader == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(trader);
        }

        public ActionResult CheckStatusTrader(string OwnerID)
        {
            repository = new SailiRepository();
            TraderAccount trader = new TraderAccount();
            trader = repository.CheckTradingAccount(OwnerID);
            if(trader == null){
                return RedirectToAction("Create", new{
                    OwnerID = OwnerID
                });
            }
            else
            {
                return RedirectToAction("Details", "TraderAccount", new
                {
                    TradingAccountID = trader.TradingAccountID.ToString()
                });
            }
        }

        public  ActionResult Create(string OwnerID)
        {
            if (OwnerID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var owner = DefaultConnection.Owners.Find(OwnerID);

            if (!owner.UserID.Equals(User.Identity.GetUserId()))
            {
                DataSerializer<string>.SecurityPriorityNumberOne(User.Identity.GetUserId());
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            encryptionServices = new EncryptionServices();
            TraderAccount model = new TraderAccount();
            model.OwnerID = OwnerID;
            model = encryptionServices.EncryptTraderAccount(model);
            DefaultConnection.TraderAccounts.Add(new TraderAccount()
            {
                TradingAccountID = model.TradingAccountID,
                OwnerID = model.OwnerID,
                CreationDate = DateTime.Now,
                Balance = 1000000          
            });
            DefaultConnection.SaveChanges();

            return RedirectToAction("Details", new
            {
                TradingAccountID = model.TradingAccountID.ToString()
            });
        }

        // POST: TraderAccount/Create
        [HttpPost]
        public ActionResult Create(TraderAccount model)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
