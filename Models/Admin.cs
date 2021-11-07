using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    public class Admin
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int adID { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string adUsername { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string adPwd { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("adPwd", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string adFirstname { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        public string adLastname { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$", ErrorMessage = "Please enter correct phone num")]
        public string adPhone { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string adEmail { get; set; }
        public string FullName()
        {
            return this.adFirstname + " " + this.adLastname;
        }
    }
}