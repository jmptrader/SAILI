using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAILI.Models;


namespace SAILI.Controllers
{
    public class SharePriceController : Controller
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        private Vector<StockPrice> SharePrice;

        public SharePriceController() { }
        // GET: SharePrice
        public ActionResult Index()
        {
            return View();
        }

        // GET: SharePrice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetStockPrice()
        {
           // DataSerializer<string>.GetStockPrice(ref SharePrice);

            return View();
        }

    }
}
