namespace ProjectBusinessCustomer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "userEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Task", "userEmail");
        }
    }
}
