namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genderadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.students", "Gender", c => c.String());
            AddColumn("dbo.Teachers", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "Gender");
            DropColumn("dbo.students", "Gender");
        }
    }
}
