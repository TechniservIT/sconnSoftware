namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapiodefconntodevicegeneric : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.IoMapDefinitions", "DeviceId");
            AddForeignKey("dbo.IoMapDefinitions", "DeviceId", "dbo.Devices", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IoMapDefinitions", "DeviceId", "dbo.Devices");
            DropIndex("dbo.IoMapDefinitions", new[] { "DeviceId" });
        }
    }
}
