namespace DVG.WIS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthGroup", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.AuthGroup", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuthGroup", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AuthGroup", "CreatedBy", c => c.String());
            AddColumn("dbo.AuthGroup", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthGroup", "ModifiedBy");
            DropColumn("dbo.AuthGroup", "CreatedBy");
            DropColumn("dbo.AuthGroup", "ModifiedDate");
            DropColumn("dbo.AuthGroup", "CreatedDate");
            DropColumn("dbo.AuthGroup", "Status");
        }
    }
}
