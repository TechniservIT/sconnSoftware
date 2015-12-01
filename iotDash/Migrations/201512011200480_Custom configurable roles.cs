namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Customconfigurableroles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "DomainId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "SiteId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "DeviceId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Active", c => c.Boolean());
            AddColumn("dbo.AspNetRoles", "ValidFrom", c => c.DateTime());
            AddColumn("dbo.AspNetRoles", "ValidUntil", c => c.DateTime());
            AddColumn("dbo.AspNetRoles", "Type", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "Type");
            DropColumn("dbo.AspNetRoles", "ValidUntil");
            DropColumn("dbo.AspNetRoles", "ValidFrom");
            DropColumn("dbo.AspNetRoles", "Active");
            DropColumn("dbo.AspNetRoles", "DeviceId");
            DropColumn("dbo.AspNetRoles", "SiteId");
            DropColumn("dbo.AspNetRoles", "DomainId");
        }
    }
}
