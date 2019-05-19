namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Noticechanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "CreatedBy", c => c.String());
            AddColumn("dbo.Notices", "Createdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notices", "path", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "path");
            DropColumn("dbo.Notices", "Createdate");
            DropColumn("dbo.Notices", "CreatedBy");
        }
    }
}
