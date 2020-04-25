using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("Society")]
    public class Society
    {
        [Key]
        public int SocietyID { get; set; }

        [Required(ErrorMessage = "please enter your society name")]
        [StringLength(30)]
        public string SocietyName { get; set; }

        [Required(ErrorMessage = "please enter your company social name")]
        public string CompanySocial { get; set; }

        [Required(ErrorMessage = "please enter your society address")]
        public string SocietyAddress { get; set; }
        [Required(ErrorMessage = "please enter your society email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address  ")]
        [EmailAddress]
        public string SocietyEmail { get; set; }


        [Required(ErrorMessage = "please enter your society Phone")]
        [Phone]
        public string SocietyPhone { get; set; }


        [Required(ErrorMessage = "please enter your  website")]
        public string WebSite { get; set; }

        [Required(ErrorMessage = "please enter your society fax")]
        [Phone]
        public string SocietyFax { get; set; }
        public virtual ICollection<Customer> Customers { get; set; } // propriété de navigation
    }
}