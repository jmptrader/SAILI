using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAILI.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNet.Identity;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;
using System.Net;
using System.Data.Entity;

namespace SAILI.Controllers
{
    [Authorize(Roles = "Trader")]
    public class PortfolioController : Controller
    {
        private Vector<Listings> listing = new Vector<Listings>();
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        private SailiRepository repository = new SailiRepository();
        private string databaseConnection = ConfigurationManager.ConnectionStrings["SailiDbContext"].ConnectionString;

        // GET: Portfolio
        public ActionResult Index()
        {
            string symbol = repository.GetAllSymbols();
            List<Listings> price = null;
            DataSerializer<string>.GetPriceListings(ref price, symbol);
            return View(price);
        }

        public ActionResult StockEnquiry(Listings list)
        {
            return View(list);
        }

        //Method serves two
        public ActionResult ConfirmBuy(string symbol)
        {
            BuyModel model = null;
            List<Listings> listings = null;
            string[] temp = symbol.Split(',');

            if(temp.Count() < 2) // means it has come from navbar request
            {
                DataSerializer<string>.GetSingleListings(ref listings, symbol);

                foreach(var company in listings)
                {
                    model = new BuyModel();
                    model.Symbol = company.Symbol;
                    model.Open = company.Open;
                    model.PurchasePrice = company.Close;
                    model.CompanyName = company.CompanyName;
                    model.Change = company.Close - company.Open;
                }
            }
            else // Has come from a Portfolio request
            {
                model = new BuyModel();
                model.Symbol = temp[0];
                model.Open = Convert.ToDecimal(temp[1]);
                model.PurchasePrice = Convert.ToDecimal(temp[2]);
                model.Change = Convert.ToDecimal(temp[3]);
                model.CompanyName = temp[4];
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(BuyModel model)
        {
            if (model.Quantity < 1)
            {
                TempData["BuyException"] = "Buying 0 stocks... Please try again";
                return RedirectToAction("ConfirmBuy", new { symbol = model.Symbol });
            }

            decimal transactionCost = (decimal)TransactionCost.BuyCost * (model.Quantity * model.PurchasePrice);
            string[] temp = model.Symbol.Split(',');
            SailiRepository repository = new SailiRepository();
            Buy buy = new Buy();
            buy.TradeDate = DateTime.Now;
            buy.TickerSymbol = temp[0];
            buy.Quantity = model.Quantity;
            buy.PurchasePrice = model.PurchasePrice;
            buy.TransactionAmount = model.Quantity * model.PurchasePrice + transactionCost;
            buy.TransactionCost = transactionCost;
            buy = repository.Finalizebuy(buy, User.Identity.GetUserId());

            Owner owner = repository.GetOwner(User.Identity.GetUserId());

            TraderAccount trader = repository.GetTrader(owner.OwnerID);

            return RedirectToAction("Details", "TraderAccount", new
            {
                TradingAccountID = trader.TradingAccountID
            });
        }

        public ActionResult ConfirmSell(string tradingAccountID, int buyID)
        {
            if(tradingAccountID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Portfolio portfolio = null;
            SailiRepository repository = new SailiRepository();
            portfolio = repository.GetPortfolio(tradingAccountID);

            if (portfolio == null)
                return HttpNotFound();

            var buy = DefaultConnection.Buys.Find(buyID);

            if (!buy.PortfolioId.Equals(portfolio.PortfolioID))
            {
                DataSerializer<string>.SecurityPriorityNumberOne(User.Identity.GetUserId());
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            Company company = new Company();
            List<Listings> listings = new List<Listings>();
            company = repository.GetCompany(company, buy.TickerSymbol);
            DataSerializer<string>.GetSingleListings(ref listings, company.Symbol);
            Sell sell = new Sell();
            sell.BuyID = buy.BuyID;
            sell.TickerSymbol = buy.TickerSymbol;
            sell.PurchasePrice = buy.PurchasePrice;

            foreach(var list in listings)
            {
                if (list.Symbol.Equals(sell.TickerSymbol))
                    sell.SoldPrice = list.Close;
            }

            TempData["ShareQuantity"] = buy.Quantity;
            TempData["CompanyName"] = company.CompanyName;

            return View(sell);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sell(Sell model)
        {
            try
            {
                Buy buy = DefaultConnection.Buys.Find(model.BuyID);
                Portfolio portfolio = DefaultConnection.Portfolios.Find(buy.PortfolioId);
                SailiRepository repository = new SailiRepository();
                Owner owner = new Owner();
                owner = repository.GetOwner(User.Identity.GetUserId());
                TraderAccount trader = new TraderAccount();
                trader = repository.GetTrader(owner.OwnerID);

                if (model.Quantity > buy.Quantity)
                {
                    TempData["SellException"] = "Selling more stocks than what is baught... Please try again";
                    return RedirectToAction("ConfirmSell", new { tradingAccountID = portfolio.TradingAccountID, buyID = buy.BuyID });
                }
                decimal transactionCost = (decimal)TransactionCost.SellCost * (model.Quantity * model.SoldPrice);
                model.PortfolioID = portfolio.PortfolioID;
                model.TradeDate = DateTime.Now;
                model.BuyID = buy.BuyID;
                model.TransactionCost = transactionCost;
                model.TransactionAmount = model.Quantity * model.SoldPrice - transactionCost;
                trader.Balance = trader.Balance + model.TransactionAmount;
                buy.Quantity = buy.Quantity - model.Quantity;
                TryUpdateModel(buy);
                DefaultConnection.Entry(buy).State = EntityState.Modified;
                DefaultConnection.SaveChanges();
                TryUpdateModel(trader);
                DefaultConnection.Entry(trader).State = EntityState.Modified;
                DefaultConnection.SaveChanges();
                TryUpdateModel(model);
                DefaultConnection.Sells.Add(model);
                DefaultConnection.SaveChanges();

                return RedirectToAction("Details", "TraderAccount", new
                {
                    TradingAccountID = portfolio.TradingAccountID
                });
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult GetListing(string symbol)
        {
            List<Listings> listings = null;
            List<Company> companies = null;
            Company company = null;
            company = repository.GetCompany(company, symbol);
            if(company != null)
            {
                companies = new List<Company>();
                companies.Add(company);
                DataSerializer<string>.GetCurrentPrice(ref listings, companies);
                return Json(listings, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false });
            }          
        }
    }
}
