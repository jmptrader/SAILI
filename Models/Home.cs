using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class Home
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int HomeID { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "About")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string About { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Help Name")]
        public string HelpName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Text)]
        [Display(Name = "Help Phone Number")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string HelpPhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Help Email")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string HelpEmail { get; set; }
    }
}