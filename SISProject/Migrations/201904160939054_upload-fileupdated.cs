namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadfileupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "MiddleName", c => c.String());
            AddColumn("dbo.UplodedFiles", "Description", c => c.String());
            AddColumn("dbo.UplodedFiles", "Type", c => c.String());
            DropColumn("dbo.Admins", "MidedleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admins", "MidedleName", c => c.String());
            DropColumn("dbo.UplodedFiles", "Type");
            DropColumn("dbo.UplodedFiles", "Description");
            DropColumn("dbo.Admins", "MiddleName");
        }
    }
}
