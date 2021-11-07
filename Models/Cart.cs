using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    public class Cart
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Required]
        [Display(Name = "Cart ID")]
        public int cartID { get; set; }
        [Required]
        [Display(Name = "Product ID")]
        public int proID { get; set; }
        [Required]
        [Display(Name = "User ID")]
        public int uID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string proName { get; set; }
        [Required]
        [Display(Name = "Cart Quantity")]
        public int cartQuantity { get; set; }
        [Required]
        [Display(Name = "Price (VNĐ)")]
        public double proPrice { get; set; }
        [Required]
        [Display(Name = "Cart Price")]
        public double cartPrice { get; set; }
        [Required]
        [Display(Name = "Image")]
        public string proImg { get; set; }

        [ForeignKey("proID")]
        public virtual Product Products { get; set; }
        [ForeignKey("uID")]
        public virtual User Users { get; set; }




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


        public Cart()
        {

        }

    }
}