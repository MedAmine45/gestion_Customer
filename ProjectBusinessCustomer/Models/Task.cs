using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("Task")]
    public class Task
    {
        [Key]
        public int TaskID { set; get; }

        public int TaskIdDev { set; get; }

        public string Title { set; get; }

        public string State { set; get; }

        public string AssignedTo { set; get; }

        public string AreaPath { set; get; }

        public string WorkItemType { set; get; }

        public string ChangedDate { set; get; }

        public string Description { set; get; }

        public string IterationPath { set; get; }

        public string TeamProject { set; get; }

        public string ChangedBy { set; get; }

        public string CreatedDate { set; get; }

        public int CommentCount { set; get; }

        public string CreatedBy { set; get; }

        public string Reason { set; get; }

        public string StateChangeDate { set; get; }

        public int Priority { set; get; }

        public string History { set; get; }

        public string userEmail { set; get; }


        //public virtual string Id { get; set; }
        //public virtual ApplicationUser ApplicationUser { set; get; }

        public int ProjectID { get; set; } //Propriété de clé étrangère

        public virtual Project Project { get; set; } // propriété de navigation
    }
}