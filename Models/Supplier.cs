using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginandR.Models
{
    public class Supplier
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int supID { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string supUsername { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string supPwd { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("supPwd", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Supplier Name")]
        [StringLength(50, MinimumLength = 2)]
        public string supName { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        /*[RegularExpression(@"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$", ErrorMessage = "Please enter correct phone num")]*/
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Please enter correct phone num")]
        public string supPhone { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Address")]
        public string supAddress { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string supEmail { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Description")]
        public string supDescription { get; set; }
        [Display(Name = "Supplier Image")]
        public string supImg { get; set; }
        [Display(Name = "Status")]
        public Boolean supStatus { get; set; } = true;
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [Display(Name = "Reset Password Code")]
        public string ResetPasswordCode { get; set; }
    }
}