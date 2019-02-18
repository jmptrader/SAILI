using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class TraderAccount
    {
        [Key]
        [StringLength(128)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string TradingAccountID { get; set; }

        [Required]
        [StringLength(128)]
        [DataType(DataType.Text)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string OwnerID { get; set; }

        [ForeignKey("OwnerID")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public Owner owner { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Balance")]
        public decimal  Balance { get; set; }
    }
}