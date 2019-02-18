using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SAILI.Models
{
    public class SaltOwner
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int EncryptOwnerID { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [StringLength(128)]
        public string SaltDOB { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [StringLength(128)]
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public ApplicationUser user { get; set; }
    }
}