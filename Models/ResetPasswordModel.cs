using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// reset password model class
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// new password
        /// </summary>
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,32}$", ErrorMessage = "Use more than 7 characters include letters and numbers")]
        public string NewPassword { get; set; }

        /// <summary>
        /// confirm password
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// reset code
        /// </summary>
        [Required]
        public string ResetCode { get; set; }
    }
}