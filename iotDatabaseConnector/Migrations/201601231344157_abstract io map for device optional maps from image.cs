namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abstractiomapfordeviceoptionalmapsfromimage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IoMapDefinitions", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropIndex("dbo.IoMapDefinitions", new[] { "DeviceId" });
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            AddColumn("dbo.DeviceMapDefinitions", "IsImageMap", c => c.Boolean(nullable: false));
            AddColumn("dbo.DeviceMapDefinitions", "ImageMapUrl", c => c.String());
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DeviceMapDefinitions", "DeviceId");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            AddForeignKey("dbo.DeviceMapDefinitions", "DeviceId", "dbo.Devices", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id", cascadeDelete: true);
            DropColumn("dbo.IoMapDefinitions", "IsImageMap");
            DropColumn("dbo.IoMapDefinitions", "ImageMapUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IoMapDefinitions", "ImageMapUrl", c => c.String());
            AddColumn("dbo.IoMapDefinitions", "IsImageMap", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropForeignKey("dbo.DeviceMapDefinitions", "DeviceId", "dbo.Devices");
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.DeviceMapDefinitions", new[] { "DeviceId" });
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int());
            DropColumn("dbo.DeviceMapDefinitions", "ImageMapUrl");
            DropColumn("dbo.DeviceMapDefinitions", "IsImageMap");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            CreateIndex("dbo.IoMapDefinitions", "DeviceId");
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id");
            AddForeignKey("dbo.IoMapDefinitions", "DeviceId", "dbo.Devices", "Id", cascadeDelete: true);
        }
    }
}
