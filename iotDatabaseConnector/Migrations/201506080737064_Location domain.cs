namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Locationdomain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Domain_DomainId", c => c.Int());
            CreateIndex("dbo.Locations", "Domain_DomainId");
            AddForeignKey("dbo.Locations", "Domain_DomainId", "dbo.iotDomains", "DomainId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "Domain_DomainId", "dbo.iotDomains");
            DropIndex("dbo.Locations", new[] { "Domain_DomainId" });
            DropColumn("dbo.Locations", "Domain_DomainId");
        }
    }
}
