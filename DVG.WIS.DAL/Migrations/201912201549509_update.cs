namespace DVG.WIS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        ActionTimeStamp = c.Long(nullable: false, identity: true),
                        ActionDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        ActionType = c.Int(nullable: false),
                        ObjectId = c.Long(),
                        ActionText = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.ActionTimeStamp);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 500),
                        Sapo = c.String(nullable: false, maxLength: 500),
                        Description = c.String(),
                        Avatar = c.String(nullable: false, maxLength: 256),
                        Source = c.String(maxLength: 256),
                        CategoryId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KeyName = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 1000),
                        Controller = c.String(nullable: false, maxLength: 256),
                        Action = c.String(nullable: false, maxLength: 256),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthGroupActionMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthGroupId = c.Int(nullable: false),
                        AthActionId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthGroupCategoryMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthGroupId = c.Int(nullable: false),
                        NewsType = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthGroupNewsStatusMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthGroupId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthGroupUserMapping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AuthGroupId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BannerAds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 500),
                        PageId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        ImageUrl = c.String(nullable: false, maxLength: 256),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256),
                        ParentId = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigSystem",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Value = c.String(maxLength: 200),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.ControlSystem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KeyName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 500),
                        Controller = c.String(maxLength: 256),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 50),
                        Type = c.Int(nullable: false),
                        Source = c.String(nullable: false, maxLength: 100),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        PermissionID = c.Int(nullable: false, identity: true),
                        PermissionName = c.String(nullable: false),
                        GroupID = c.Int(nullable: false),
                        IsHidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionID);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Avatar = c.String(nullable: false, maxLength: 256),
                        Description = c.String(nullable: false),
                        Position = c.Int(nullable: false),
                        Link = c.String(nullable: false, maxLength: 500),
                        SubjectName = c.String(nullable: false, maxLength: 256),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        ModifiedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Password = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false),
                        FullName = c.String(maxLength: 256),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedDateSpan = c.Long(nullable: false),
                        Desciption = c.String(),
                        Address = c.String(maxLength: 256),
                        LastLogin = c.Long(nullable: false),
                        LastPasswordChange = c.Long(nullable: false),
                        PasswordQuestion = c.String(),
                        PasswordAnswer = c.String(),
                        Avatar = c.String(maxLength: 256),
                        Status = c.Int(nullable: false),
                        Gender = c.Boolean(),
                        Signature = c.String(),
                        CityId = c.Byte(),
                        Birthday = c.Long(),
                        LastUpdate = c.Long(),
                        Skype = c.String(maxLength: 256),
                        Facebook = c.String(maxLength: 256),
                        Google = c.String(maxLength: 256),
                        GoogleId = c.String(maxLength: 256),
                        FacebookId = c.String(maxLength: 256),
                        Transporter = c.String(maxLength: 256),
                        UserType = c.Long(nullable: false)
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserRole");
            DropTable("dbo.Persons");
            DropTable("dbo.Permission");
            DropTable("dbo.Customers");
            DropTable("dbo.ControlSystem");
            DropTable("dbo.ConfigSystem");
            DropTable("dbo.Categories");
            DropTable("dbo.BannerAds");
            DropTable("dbo.AuthGroupUserMapping");
            DropTable("dbo.AuthGroupNewsStatusMapping");
            DropTable("dbo.AuthGroupCategoryMapping");
            DropTable("dbo.AuthGroupActionMapping");
            DropTable("dbo.AuthGroup");
            DropTable("dbo.AuthAction");
            DropTable("dbo.Articles");
            DropTable("dbo.Activity");
        }
    }
}
