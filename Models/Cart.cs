using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Cart class
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// cart id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Cart ID")]
        public int cartID { get; set; }

        /// <summary>
        /// product id
        /// </summary>
        [Required]
        [Display(Name = "Product ID")]
        public int proID { get; set; }

        /// <summary>
        /// user id
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        public int uID { get; set; }

        /// <summary>
        /// product name
        /// </summary>
        [Required]
        [Display(Name = "Product Name")]
        public string proName { get; set; }

        /// <summary>
        /// cart quantity
        /// </summary>
        [Required]
        [Display(Name = "Cart Quantity")]
        public int cartQuantity { get; set; }

        /// <summary>
        /// product price
        /// </summary>
        [Required]
        [Display(Name = "Price (VNĐ)")]
        public double proPrice { get; set; }

        /// <summary>
        /// cart price
        /// </summary>
        [Required]
        [Display(Name = "Cart Price")]
        public double cartPrice { get; set; }

        /// <summary>
        /// product image
        /// </summary>
        [Required]
        [Display(Name = "Image")]
        public string proImg { get; set; }

        /// <summary>
        /// lazy loading for proID from product table
        /// </summary>
        [ForeignKey("proID")]
        public virtual Product Products { get; set; }

        /// <summary>
        /// lazy loading for proID from product table
        /// </summary>
        [ForeignKey("uID")]
        public virtual User Users { get; set; }



        /// <summary>
        /// Constructor for temporary cart
        /// </summary>
        /// <param name="iproID">product id</param>
        public Cart(int iproID)
        {
            using (DB_Entities _db = new DB_Entities())
            {
                this.proID = iproID;
                Product sp = _db.Products.Single(n => n.proID == iproID);
                this.proName = sp.proName;
                this.proImg = sp.proImg;
                this.proPrice = sp.proPrice - sp.proPrice * sp.discount / 100;                
                this.cartQuantity = 1;
                this.cartPrice = proPrice * cartQuantity;
            }

        }

        /// <summary>
        /// Non-parameterized constructor
        /// </summary>
        public Cart()
        {

        }

    }
}