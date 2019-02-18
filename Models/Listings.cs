using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class Listings
    {
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Open")]
        public decimal Open { get; set; }
        [Display(Name = "Close")]
        public decimal Close { get; set; }
        [Display(Name = "Change")]
        public decimal Change { get; set; }

    }
}