using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class OlympicBoard
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int OlympicBoardID { get; set; }

        [Display(Name = "Olympic Date")]
        public DateTime OlympicDate { get; set; }

        [Display(Name = "Sector")]
        public Sector sector { get; set; }

        public List<Portfolio> TopThree { get; set; }
    }

    public class OlympianBoard
    {
        [Display(Name = "Olympic Date")]
        public DateTime OlympicDate { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
    }

    public  class SortWinner
    {
        public string TradingAcountID { get; set; }
        public int PortfolioID { get; set; }
        public string OwnerID { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
    }
}