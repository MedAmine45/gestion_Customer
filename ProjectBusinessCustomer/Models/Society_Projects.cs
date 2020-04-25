using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("SocietyProjects")]
    public class Society_Projects
    {
        [Key]
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public int SocietyID { get; set; }

    }
}