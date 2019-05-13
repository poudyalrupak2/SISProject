namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postremoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Admins", "Post");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Admins", "Post", c => c.String());
        }
    }
}
