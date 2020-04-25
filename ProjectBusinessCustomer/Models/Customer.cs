using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [StringLength(70)]
        [Required(ErrorMessage = "Please enter your name")]
        public string CustomerName { get; set; }


        [Required(ErrorMessage = "please enter your profile")]
        public string Profile { get; set; }


        [Required(ErrorMessage = "please enter your country ")]
        public string Country { get; set; }


        [Required(ErrorMessage = "please enter your region ")]
        public string Region { get; set; }

        [Required(ErrorMessage = "please enter your address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "please enter your email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter your phone")]
        [Phone]
        public string Phone { get; set; }

        [Phone]
        public string Fax { get; set; }

        [Required(ErrorMessage = "please enter your activity ")]
        public string Activity { get; set; }

        [Required(ErrorMessage = "please enter your notes")]
        public string ImportantNotes { get; set; }


        public int SocietyID { get; set; } //Propriété de clé étrangère

        public virtual Society Society { get; set; } // propriété de navigation
    }
}