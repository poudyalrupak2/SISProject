namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticetitleupdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notices", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notices", "Title", c => c.Int(nullable: false));
        }
    }
}
