namespace ProjectBusinessCustomer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bussines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 70),
                        Profile = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Fax = c.String(),
                        Activity = c.String(nullable: false),
                        ImportantNotes = c.String(nullable: false),
                        SocietyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID)
                .ForeignKey("dbo.Society", t => t.SocietyID, cascadeDelete: true)
                .Index(t => t.SocietyID);
            
            CreateTable(
                "dbo.Society",
                c => new
                    {
                        SocietyID = c.Int(nullable: false, identity: true),
                        SocietyName = c.String(nullable: false, maxLength: 30),
                        CompanySocial = c.String(nullable: false),
                        SocietyAddress = c.String(nullable: false),
                        SocietyEmail = c.String(nullable: false),
                        SocietyPhone = c.String(nullable: false),
                        WebSite = c.String(nullable: false),
                        SocietyFax = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SocietyID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Id = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Url = c.String(),
                        State = c.String(),
                        Revision = c.Int(nullable: false),
                        Visibilty = c.String(),
                        LastUpdateTime = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        TaskIdDev = c.Int(nullable: false),
                        Title = c.String(),
                        State = c.String(),
                        AssignedTo = c.String(),
                        AreaPath = c.String(),
                        WorkItemType = c.String(),
                        ChangedDate = c.String(),
                        Description = c.String(),
                        IterationPath = c.String(),
                        TeamProject = c.String(),
                        ChangedBy = c.String(),
                        CreatedDate = c.String(),
                        CommentCount = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        Reason = c.String(),
                        StateChangeDate = c.String(),
                        Priority = c.Int(nullable: false),
                        History = c.String(),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Customer", "SocietyID", "dbo.Society");
            DropIndex("dbo.Task", new[] { "ProjectID" });
            DropIndex("dbo.Customer", new[] { "SocietyID" });
            DropTable("dbo.Task");
            DropTable("dbo.Project");
            DropTable("dbo.Society");
            DropTable("dbo.Customer");
        }
    }
}
