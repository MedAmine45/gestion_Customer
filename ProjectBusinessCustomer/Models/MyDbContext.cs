namespace ProjectBusinessCustomer.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MyDbContext : DbContext
    {
        // Your context has been configured to use a 'MyDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ProjectBusinessCustomer.Models.MyDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MyDbContext' 
        // connection string in the application configuration file.
        public MyDbContext()
            : base("name=MyDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Society> Societies { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Society_Projects> Society_Projects { get; set; }

        public virtual DbSet<UserToken> Tokens { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}