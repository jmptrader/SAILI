using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class Company
    {
        [Key]
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Symbol")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string Symbol { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Company Name")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string CompanyName { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int SectorID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public bool IsDeleted { get; set; }
    }
}