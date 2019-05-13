namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseupdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "FirstName", c => c.String());
            AddColumn("dbo.Admins", "MiddleName", c => c.String());
            AddColumn("dbo.Admins", "LastName", c => c.String());
            AddColumn("dbo.students", "FirstName", c => c.String());
            AddColumn("dbo.students", "MiddleName", c => c.String());
            AddColumn("dbo.students", "LastName", c => c.String());
            AddColumn("dbo.students", "RollNo", c => c.Int(nullable: false));
            AddColumn("dbo.UplodedFiles", "Name", c => c.String());
            DropColumn("dbo.Admins", "Name");
            DropColumn("dbo.students", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.students", "Name", c => c.String());
            AddColumn("dbo.Admins", "Name", c => c.String());
            DropColumn("dbo.UplodedFiles", "Name");
            DropColumn("dbo.students", "RollNo");
            DropColumn("dbo.students", "LastName");
            DropColumn("dbo.students", "MiddleName");
            DropColumn("dbo.students", "FirstName");
            DropColumn("dbo.Admins", "LastName");
            DropColumn("dbo.Admins", "MiddleName");
            DropColumn("dbo.Admins", "FirstName");
        }
    }
}
