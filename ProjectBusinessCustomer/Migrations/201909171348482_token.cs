namespace ProjectBusinessCustomer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class token : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserToken", "Token", c => c.String(nullable: false));
            AlterColumn("dbo.UserToken", "Projectname", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserToken", "Projectname", c => c.String());
            AlterColumn("dbo.UserToken", "Token", c => c.String());
        }
    }
}
