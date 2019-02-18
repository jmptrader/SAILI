using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SAILI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAILI.Models
{
    public class Portfolio
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Portfolio ID")]
        public int PortfolioID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public List<Buy> Buys { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public List<Sell> Sells { get; set; }

        [StringLength(128)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string TradingAccountID { get; set; }

        [ForeignKey("TradingAccountID"),System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public TraderAccount account { get; set; }

    }

    public class PortfolioViewModel
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Portfolio ID")]
        public int PortfolioID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public List<BuyViewModel> Buys { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public List<SellViewModel> Sells { get; set; }

        [StringLength(128)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string TradingAccountID { get; set; }

        [ForeignKey("TradingAccountID"), System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public TraderAccount account { get; set; }

    }

    public class Buy
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Buy ID")]
        public int BuyID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Trade Date")]
        public DateTime TradeDate { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int PortfolioId { get; set; }

        [ForeignKey("PortfolioId")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Portfolio portfolio { get; set; }

        [Display(Name = "Ticker Symbol")]
        public string TickerSymbol { get; set; }


        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Transaction Cost")]
        public decimal TransactionCost  { get; set; }

    }

    public class BuyViewModel
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Buy ID")]
        public int BuyID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Trade Date")]
        public DateTime TradeDate { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int PortfolioId { get; set; }

        [ForeignKey("PortfolioId")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Portfolio portfolio { get; set; }

        [Display(Name = "Ticker Symbol")]
        public string TickerSymbol { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Transaction Cost")]
        public decimal TransactionCost { get; set; }

        [Display(Name = "Performance")]
        public decimal Performance { get; set; }

        [Display(Name = "Open")]
        public decimal Open { get; set; }

        [Display(Name = "Close")]
        public decimal Close { get; set; }

    }

    public class BuyModel
    {
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        [Display(Name = "Open")]
        public decimal Open { get; set; }

        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Change")]
        public decimal Change { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

    }

    public class Sell
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Sell ID")]
        public int SellID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Transaction Date")]
        public DateTime TradeDate { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public int BuyID { get; set; }

        [ForeignKey("BuyID")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Buy buy { get; set; }

        [Display(Name = "Ticker Symbol")]
        public string TickerSymbol { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Sold Price")]
        public decimal SoldPrice { get; set; }

        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Transaction Cost")]
        public decimal TransactionCost { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int PortfolioID { get; set; }

    }

    public class SellViewModel
    {
        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        [Display(Name = "Sell ID")]
        public int SellID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Transaction Date")]
        public DateTime TradeDate { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public int BuyID { get; set; }

        [ForeignKey("BuyID")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Buy buy { get; set; }

        [Display(Name = "Ticker Symbol")]
        public string TickerSymbol { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Sold Price")]
        public decimal SoldPrice { get; set; }

        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Transaction Cost")]
        public decimal TransactionCost { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int PortfolioID { get; set; }
    }




    public class Historical
    {
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        public List<HistoricalPrices> historicalPrices { get; set; }
    }

    public class HistoricalPrices
    {
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Open")]
        public decimal Open { get; set; }

        [Display(Name = "Close")]
        public decimal Close { get; set; }

    }

    public class TransactionCost
    {
        public const Double BuyCost = .01;
        public const Double SellCost = .25; 
    }
}