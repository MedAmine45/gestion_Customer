namespace ProjectBusinessCustomer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usertoken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(),
                        Token = c.String(),
                        Projectname = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserToken");
        }
    }
}
