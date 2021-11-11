using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// Admin Class
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Admin ID
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int adID { get; set; }

        /// <summary>
        /// Admin username
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string adUsername { get; set; }

        /// <summary>
        /// Admin password
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string adPwd { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("adPwd", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// admin first name
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,}$", ErrorMessage = "Allow letters only")]
        public string adFirstname { get; set; }

        /// <summary>
        /// admin last name 
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,}$", ErrorMessage = "Allow letters only")]
        public string adLastname { get; set; }

        /// <summary>
        /// admin phone number
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$", ErrorMessage = "Please enter correct phone num")]
        public string adPhone { get; set; }

        /// <summary>
        /// admin email
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string adEmail { get; set; }

        /// <summary>
        /// fullname
        /// </summary>
        /// <returns>combination of firstname and lastname</returns>
        public string FullName()
        {
            return this.adFirstname + " " + this.adLastname;
        }
    }
}