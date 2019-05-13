namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UplodedFiles", "Tags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UplodedFiles", "Tags");
        }
    }
}
