using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    public class Bill
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "Bill ID")]
        public int bID { get; set; }
        [Display(Name = "Delivery Status")]
        public bool deliStatus { get; set; }
        [Required]
        [Display(Name = "User ID")]
        public int uID { get; set; }
        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? bDate { get; set; }
        [Display(Name = "Shipped Date")]
        public DateTime? shipDate { get; set; }
        [Required]
        [Display(Name = "Paid Bill")]
        public bool paid { get; set; }
        [Required]
        [Display(Name = "Canceled Bill")]
        public bool caceled { get; set; }
        [Required]
        [Display(Name = "Deleted Bill")]
        public bool deleted { get; set; }
        [ForeignKey("uID")]
        public virtual User User { get; set; }
    }
}