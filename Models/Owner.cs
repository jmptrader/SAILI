using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class Owner
    {
        [Key]
        [StringLength(128)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string OwnerID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public bool IsDeleted { get; set; }

        [StringLength(128)]
        [DataType(DataType.Text)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public ApplicationUser user { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Middle Name")]
        public string MiddleName{ get; set; } 

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(128)]
        [Display(Name = "DOB")]
        public string DOB { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Address Number")]
        public string AddressNumber { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Address Name")]
        public string AddressName { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string PostcodeID { get; set; }

    }
}