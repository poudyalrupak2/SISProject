namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filesuploadupdated1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UplodedFiles", "imagepath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UplodedFiles", "imagepath");
        }
    }
}
