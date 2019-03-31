namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        Post = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Description = c.String(),
                        FileType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        RandomPass = c.String(),
                        PasswordHash = c.Binary(),
                        PasswordSalt = c.Binary(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        NoticeId = c.Int(nullable: false, identity: true),
                        ShortDescription = c.String(),
                        LongDescription = c.String(),
                        NoticeType = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.NoticeId);
            
            CreateTable(
                "dbo.Semisters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SemisterName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        StudentRole = c.String(),
                        Status = c.Boolean(nullable: false),
                        Semister_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semisters", t => t.Semister_Id)
                .Index(t => t.Semister_Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNo = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UplodedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FIlesType = c.String(),
                        Tags = c.String(),
                        UplodedBy = c.String(),
                        Files_Id = c.Int(),
                        semister_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.Files_Id)
                .ForeignKey("dbo.Semisters", t => t.semister_Id)
                .Index(t => t.Files_Id)
                .Index(t => t.semister_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UplodedFiles", "semister_Id", "dbo.Semisters");
            DropForeignKey("dbo.UplodedFiles", "Files_Id", "dbo.Files");
            DropForeignKey("dbo.students", "Semister_Id", "dbo.Semisters");
            DropIndex("dbo.UplodedFiles", new[] { "semister_Id" });
            DropIndex("dbo.UplodedFiles", new[] { "Files_Id" });
            DropIndex("dbo.students", new[] { "Semister_Id" });
            DropTable("dbo.UplodedFiles");
            DropTable("dbo.Teachers");
            DropTable("dbo.students");
            DropTable("dbo.Semisters");
            DropTable("dbo.Notices");
            DropTable("dbo.Logins");
            DropTable("dbo.Files");
            DropTable("dbo.Admins");
        }
    }
}
