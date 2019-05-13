namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filesuploadupdated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UplodedFiles", "Files_Id", "dbo.Files");
            DropForeignKey("dbo.UplodedFiles", "semister_Id", "dbo.Semisters");
            DropIndex("dbo.UplodedFiles", new[] { "Files_Id" });
            DropIndex("dbo.UplodedFiles", new[] { "semister_Id" });
            AddColumn("dbo.UplodedFiles", "UplodedDate", c => c.String());
            AddColumn("dbo.UplodedFiles", "UpdatedFileName", c => c.String());
            AddColumn("dbo.UplodedFiles", "Filename", c => c.String());
            DropColumn("dbo.UplodedFiles", "Type");
            DropColumn("dbo.UplodedFiles", "Files_Id");
            DropColumn("dbo.UplodedFiles", "semister_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UplodedFiles", "semister_Id", c => c.Int());
            AddColumn("dbo.UplodedFiles", "Files_Id", c => c.Int());
            AddColumn("dbo.UplodedFiles", "Type", c => c.String());
            DropColumn("dbo.UplodedFiles", "Filename");
            DropColumn("dbo.UplodedFiles", "UpdatedFileName");
            DropColumn("dbo.UplodedFiles", "UplodedDate");
            CreateIndex("dbo.UplodedFiles", "semister_Id");
            CreateIndex("dbo.UplodedFiles", "Files_Id");
            AddForeignKey("dbo.UplodedFiles", "semister_Id", "dbo.Semisters", "Id");
            AddForeignKey("dbo.UplodedFiles", "Files_Id", "dbo.Files", "Id");
        }
    }
}
