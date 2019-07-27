namespace SISProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Address = c.String(),
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
                        LoginTime = c.DateTime(),
                        LogoutTime = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notices",
                c => new
                    {
                        NoticeId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ShortDescription = c.String(nullable: false),
                        LongDescription = c.String(nullable: false),
                        NoticeType = c.String(nullable: false),
                        Image = c.String(),
                        CreatedBy = c.String(),
                        Createdate = c.DateTime(nullable: false),
                        path = c.String(),
                    })
                .PrimaryKey(t => t.NoticeId);
            
            CreateTable(
                "dbo.Semisters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SemesterName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(),
                        LastName = c.String(nullable: false),
                        RollNo = c.Int(nullable: false),
                        SemisterId = c.Int(nullable: false),
                        Faculty = c.String(),
                        Email = c.String(nullable: false),
                        Address = c.String(),
                        Gender = c.String(nullable: false),
                        EnrollDate = c.DateTime(),
                        StudentRole = c.String(),
                        NotificationSeenDate = c.String(),
                        Status = c.Boolean(nullable: false),
                        photopath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semisters", t => t.SemisterId, cascadeDelete: true)
                .Index(t => t.SemisterId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNo = c.String(nullable: false),
                        Address = c.String(),
                        Gender = c.String(nullable: false),
                        HireDate = c.DateTime(),
                        status = c.Boolean(nullable: false),
                        NotificationSeenDate = c.String(),
                        photopath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UplodedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        FIlesType = c.String(),
                        Tags = c.String(),
                        UplodedBy = c.String(),
                        UplodedDate = c.String(),
                        UpdatedFileName = c.String(),
                        imagepath = c.String(),
                        Filename = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.students", "SemisterId", "dbo.Semisters");
            DropIndex("dbo.students", new[] { "SemisterId" });
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
