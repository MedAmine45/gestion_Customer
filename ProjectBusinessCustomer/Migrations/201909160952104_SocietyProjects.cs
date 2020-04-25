namespace ProjectBusinessCustomer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocietyProjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocietyProjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        SocietyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SocietyProjects");
        }
    }
}
