using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Bill Class
    /// </summary>
    public class Bill
    {
        /// <summary>
        /// Bill ID
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Bill ID")]
        public int bID { get; set; }

        /// <summary>
        /// Delivery status
        /// </summary>
        [Display(Name = "Delivery Status")]
        public bool deliStatus { get; set; }

        /// <summary>
        /// user id
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        public int uID { get; set; }

        /// <summary>
        /// bill date
        /// </summary>
        [Required]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? bDate { get; set; }

        /// <summary>
        /// ship date
        /// </summary>
        [Display(Name = "Shipped Date")]
        public DateTime? shipDate { get; set; }

        /// <summary>
        /// pay status
        /// </summary>
        [Required]
        [Display(Name = "Paid Bill")]
        public bool paid { get; set; }

        /// <summary>
        /// cancel status
        /// </summary>
        [Required]
        [Display(Name = "Canceled Bill")]
        public bool caceled { get; set; }

        /// <summary>
        /// delete status
        /// </summary>
        [Required]
        [Display(Name = "Deleted Bill")]
        public bool deleted { get; set; }

        /// <summary>
        /// Foreign key from user table
        /// </summary>
        [ForeignKey("uID")]
        public virtual User User { get; set; }
    }
}