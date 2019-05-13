namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filesuploadupdated2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UplodedFiles", "Tags");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UplodedFiles", "Tags", c => c.String());
        }
    }
}
