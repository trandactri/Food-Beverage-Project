using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginandR.Models
{
    public class User
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int uID { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display (Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string uUsername { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string uPwd { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("uPwd", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string uFirstname { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        public string uLastname { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        /*[RegularExpression(@"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$", ErrorMessage = "Please enter correct phone num")]*/
        [RegularExpression("([0-9]+)",ErrorMessage ="Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10)]
        public string uPhone { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Address")]
        public string uAddress { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [MyDate(ErrorMessage = "Invalid date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime uBirthday { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string uEmail { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Gender")]
        public string uGender { get; set; }
        [Display(Name = "Credit Card Number")]
        [StringLength(18, MinimumLength = 14)]
        public string uCreditCard { get; set; }
        public string FullName()
        {
            return this.uFirstname + " " + this.uLastname;
        }
        [Display(Name = "Status")]
        public Boolean isActive { get; set; } = true;
        [Display(Name = "Reset Password Code")]
        public string ResetPasswordCode { get; set; }
    }

    /// <summary>
    /// Custom attribute for valid date
    /// </summary>
    public class MyDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Function used to check valid date
        /// </summary>
        /// <param name="value">value want to be checked</param>
        /// <returns>If date less than or equal to today return true, else return false</returns>
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime d = Convert.ToDateTime(value);
            return d <= DateTime.Now; //Dates Less than or equal to today are valid (true)
        }
    }
}