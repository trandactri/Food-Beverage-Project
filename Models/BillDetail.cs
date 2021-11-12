using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Bill Detail class
    /// </summary>
    public class BillDetail
    {
        /// <summary>
        /// bill detail id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Bill Detail ID")]
        public int bdetID { get; set; }

        /// <summary>
        /// bill id
        /// </summary>
        [Required]
        [Display(Name = "Bill ID")]
        public int bID { get; set; }

        /// <summary>
        /// product id
        /// </summary>
        [Required]
        [Display(Name = "Product ID")]
        public int proID { get; set; }

        /// <summary>
        /// product name
        /// </summary>
        [Required]
        [Display(Name = "Product Name")]
        public string proName { get; set; }

        /// <summary>
        /// product quantity
        /// </summary>
        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        /// <summary>
        /// bill price
        /// </summary>
        [Required]
        [Display(Name = "Price")]
        public double price { get; set; }

        /// <summary>
        /// lazy loading for foreign key from product
        /// </summary>
        [ForeignKey("proID")]
        public virtual Product Products { get; set; }

        /// <summary>
        /// lazy loading foreign key from bill
        /// </summary>
        [ForeignKey("bID")]
        public virtual Bill Bill { get; set; }
    }
}