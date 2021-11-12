using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginandR.Models
{
    /// <summary>
    /// Supplier class
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// supplier id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int supID { get; set; }

        /// <summary>
        /// supplier username
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string supUsername { get; set; }

        /// <summary>
        /// supplier password
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        [Display(Name = "Password")]
        public string supPwd { get; set; }

        /// <summary>
        /// confirm password
        /// </summary>
        [NotMapped]
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("supPwd", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// supplier name
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Supplier Name")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage = "Allow letters only")]
        public string supName { get; set; }

        /// <summary>
        /// supplier phone number
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(0|\+84)(\s|\.)?((3[2-9])|(5[689])|(7[06-9])|(8[1-689])|(9[0-46-9]))(\d)(\s|\.)?(\d{3})(\s|\.)?(\d{3})$", ErrorMessage = "Please enter correct phone num")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Please enter correct phone num")]
        public string supPhone { get; set; }

        /// <summary>
        /// supplier address
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Address")]
        public string supAddress { get; set; }

        /// <summary>
        /// supplier email
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email, Please Re-Enter")]
        public string supEmail { get; set; }

        /// <summary>
        /// supplier description
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Description")]
        public string supDescription { get; set; }

        /// <summary>
        /// supplier image
        /// </summary>
        [Display(Name = "Supplier Image")]
        public string supImg { get; set; }

        /// <summary>
        /// supplier status
        /// </summary>
        [Display(Name = "Status")]
        public Boolean supStatus { get; set; } = true;

        /// <summary>
        /// image file
        /// </summary>
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        /// <summary>
        /// reset password code
        /// </summary>
        [Display(Name = "Reset Password Code")]
        public string ResetPasswordCode { get; set; }
    }
}
