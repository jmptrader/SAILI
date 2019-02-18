 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class StockPrice
    {
        [Key]
        public int SharePriceID { get; set; }

        public DateTime Date { get; set; }

        public string Symbol { get; set; }

        [ForeignKey("Symbol")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Company company { get; set; } 

        public decimal Open { get; set; }

        public decimal Closing { get; set; }

        public decimal PercentageChange { get; set; }

        public decimal PE { get; set; }

        public decimal EPS { get; set; }

    }
}