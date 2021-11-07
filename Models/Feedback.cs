using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;



namespace LoginandR.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int fID { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "User ID")]
        public int uID { get; set; }
        [Required(ErrorMessage = "Cannot be blank")]
        [Display(Name = "Supplier ID")]
        public int supID { get; set; }
        [Display(Name = "Feedback Date")]
        public DateTime fDate { get; set; }
        [Required(ErrorMessage = "Please enter your message")]
        [Display(Name = "Feedback Message")]
        public string fMessage { get; set; }
        public bool fStatus { get; set; } = true;
        [ForeignKey("uID")]
        public virtual User User { get; set; }
        [ForeignKey("supID")]
        public virtual Supplier Supplier { get; set; }
    }
}