using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBusinessCustomer.Models
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
        public int Revision { get; set; }
        public string Visibilty { get; set; }
        public string LastUpdateTime { get; set; }
        public virtual ICollection<Task> Tasks { get; set; } // propriété de navigation
    }
    public class Projects
    {
        public int count { get; set; }
        public Value[] value { get; set; }
    }
    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public string lastUpdateTime { get; set; }
    }
}