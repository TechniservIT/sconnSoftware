namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapdeflatlng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IoMapDefinitions", "Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.IoMapDefinitions", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IoMapDefinitions", "Longitude");
            DropColumn("dbo.IoMapDefinitions", "Latitude");
        }
    }
}
