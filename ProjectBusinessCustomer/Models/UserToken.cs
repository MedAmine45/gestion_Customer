using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("UserToken")]
    public class UserToken
    {
        [Key]
        public int Id { get; set; }
        public string Uri { get; set; }
        [Required(ErrorMessage = "please enter your token")]
        public string Token { get; set; }
        [Required(ErrorMessage = "please enter your project name")]
        public string Projectname { get; set; }
    }
}