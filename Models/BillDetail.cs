using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    public class BillDetail
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "Bill Detail ID")]
        public int bdetID { get; set; }
        [Required]
        [Display(Name = "Bill ID")]
        public int bID { get; set; }
        [Required]
        [Display(Name = "Product ID")]
        public int proID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string proName { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        [Required]
        [Display(Name = "Price")]
        public double price { get; set; }

        [ForeignKey("proID")]
        public virtual Product Products { get; set; }
        [ForeignKey("bID")]
        public virtual Bill Bill { get; set; }
    }
}