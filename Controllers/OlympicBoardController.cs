using Microsoft.AspNet.Identity;
using SAILI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;


namespace SAILI.Controllers
{

    public class OlympicBoardController : Controller
    {
        private ApplicationDbContext DefaultConnection = new ApplicationDbContext();
        private SailiRepository repository = new SailiRepository();

        // GET: OlympicBoard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLeaderBoard()
        {
            DateTime eventDate = new DateTime();
            eventDate.AddDays(05).AddMonths(08).AddYears(2016).AddHours(09).AddMinutes(00).AddSeconds(00).AddMilliseconds(00);
            OlympianBoard olympianBoard = new OlympianBoard();
            olympianBoard.OlympicDate = eventDate;
            List<TraderAccount> traders = new List<TraderAccount>();
            List<SortWinner> winners = new List<SortWinner>();
            List<Owner> owners = new List<Owner>();
            winners = repository.GetTopThree(ref traders, ref owners, ref winners);
            bool check = false;

            foreach(var itemOwner in owners){
                if(User.Identity.GetUserId().Equals(itemOwner.UserID)){
                    foreach(var itemTrader in traders){
                        if(itemTrader.OwnerID.Equals(itemOwner.OwnerID)){
                            TempData["TradingAccountID"] = itemTrader.TradingAccountID;
                            check = true;
                        }
                    }
                }
                if (check)
                    break;
            }

            TempData["GoldAmount"] = winners[0].Amount;
            TempData["GoldTradingAccountID"] = winners[0].TradingAcountID;
            TempData["GoldProjectID"] = winners[0].PortfolioID;
            TempData["GoldName"] = winners[0].Name;

            TempData["SilverAmount"] = winners[1].Amount;
            TempData["SilverTradingAccountID"] = winners[1].TradingAcountID;
            TempData["SilverProjectID"] = winners[1].PortfolioID;
            TempData["SilverName"] = winners[1].Name;

            TempData["BronzeAmount"] = winners[2].Amount;
            TempData["BronzeTradingAccountID"] = winners[2].TradingAcountID;
            TempData["BrozeProjectID"] = winners[2].PortfolioID;
            TempData["BronzeName"] = winners[2].Name;

            return View(winners);

        }
    }
}
