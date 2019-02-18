using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int ContactID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Number")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Contact Email")]
        [System.Web.Mvc.HiddenInput(DisplayValue = true)]
        public string ContactEmail { get; set; }
    }
}