namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migdataupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "Title", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "Title");
        }
    }
}
