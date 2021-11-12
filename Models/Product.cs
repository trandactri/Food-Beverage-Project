using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Product class
    /// </summary>
    public class Product
    {
        /// <summary>
        /// product id 
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Product ID")]
        public int proID { get; set; }

        /// <summary>
        /// type id
        /// </summary>
        [Required]
        [Display(Name = "Type ID")]
        public int tID { get; set; }

        /// <summary>
        /// supplier id
        /// </summary>
        [Required]
        [Display(Name = "Supplier ID")]
        public int supID { get; set; }

        /// <summary>
        /// product name
        /// </summary>
        [Required]
        [Display(Name = "Product Name")]
        public string proName { get; set; }

        /// <summary>
        /// product price
        /// </summary>
        [Required]
        [Display(Name = "Price (VNĐ)")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Invalid Price")]
        public double proPrice { get; set; }

        /// <summary>
        /// product image
        /// </summary>
        [Display(Name = "Image")]
        public string proImg { get; set; }

        /// <summary>
        /// product description
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        public string proDescription { get; set; }

        /// <summary>
        /// product discount
        /// </summary>
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Invalid Discount")]
        [Display(Name = "Discount (%)")]
        public double discount { get; set; }

        /// <summary>
        /// product quantity
        /// </summary>
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Invalid Quantity")]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        /// <summary>
        /// product status
        /// </summary>
        public bool proStatus { get; set; } = true;

        /// <summary>
        /// lazy loading for foreign key from typeP
        /// </summary>
        [ForeignKey("tID")]
        public virtual TypeP TypeP { get; set; }

        /// <summary>
        /// lazy loading foreign key from supplier
        /// </summary>
        [ForeignKey("supID")]
        public virtual Supplier Supplier { get; set; }
    }
}