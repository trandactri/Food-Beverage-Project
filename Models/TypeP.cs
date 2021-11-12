using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LoginandR.Models
{
    /// <summary>
    /// TypeP class
    /// </summary>
    public class TypeP
    {
        /// <summary>
        /// type id
        /// </summary>
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Type ID")]
        public int tID { get; set; }

        /// <summary>
        /// type name
        /// </summary>
        [Display(Name = "Type Name")]
        public string tName { get; set; }
    }
}