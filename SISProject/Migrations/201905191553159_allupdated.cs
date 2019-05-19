namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allupdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logins", "LoginTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logins", "LoginTime", c => c.DateTime(nullable: false));
        }
    }
}
