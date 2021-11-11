using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;



namespace LoginandR.Models
{
    /// <summary>
    /// Feedback class
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// feedback id
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int fID { get; set; }

        /// <summary>
        /// user id
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "User ID")]
        public int uID { get; set; }

        /// <summary>
        /// supplier id
        /// </summary>
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Supplier ID")]
        public int supID { get; set; }

        /// <summary>
        /// feedback date
        /// </summary>
        [Display(Name = "Feedback Date")]
        public DateTime fDate { get; set; }

        /// <summary>
        /// feedback message
        /// </summary>
        [Required(ErrorMessage = "Please enter your message")]
        [Display(Name = "Feedback Message")]
        public string fMessage { get; set; }

        /// <summary>
        /// feedback status
        /// </summary>
        public bool fStatus { get; set; } = true;

        /// <summary>
        /// lazy loading for foreign uID from user table
        /// </summary>
        [ForeignKey("uID")]
        public virtual User User { get; set; }

        /// <summary>
        /// lazy loading for foreign supID from supplier table
        /// </summary>
        [ForeignKey("supID")]
        public virtual Supplier Supplier { get; set; }
    }
}