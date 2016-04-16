namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genericdevicewithmapdefitionmapdefwithiomapdefs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            RenameColumn(table: "dbo.DeviceMapDefinitions", name: "Definition_Id", newName: "MapDefinition_Id");
            RenameIndex(table: "dbo.DeviceMapDefinitions", name: "IX_Definition_Id", newName: "IX_MapDefinition_Id");
            AddColumn("dbo.DeviceMapDefinitions", "IsImageMap", c => c.Boolean(nullable: false));
            AddColumn("dbo.DeviceMapDefinitions", "ImageMapUrl", c => c.String());
            AddColumn("dbo.DeviceMapDefinitions", "Device_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.DeviceMapDefinitions", "Device_Id");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id", cascadeDelete: true);
            DropColumn("dbo.DeviceMapDefinitions", "DeviceId");
            DropColumn("dbo.IoMapDefinitions", "IsImageMap");
            DropColumn("dbo.IoMapDefinitions", "ImageMapUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IoMapDefinitions", "ImageMapUrl", c => c.String());
            AddColumn("dbo.IoMapDefinitions", "IsImageMap", c => c.Boolean(nullable: false));
            AddColumn("dbo.DeviceMapDefinitions", "DeviceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.DeviceMapDefinitions", new[] { "Device_Id" });
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int());
            DropColumn("dbo.DeviceMapDefinitions", "Device_Id");
            DropColumn("dbo.DeviceMapDefinitions", "ImageMapUrl");
            DropColumn("dbo.DeviceMapDefinitions", "IsImageMap");
            RenameIndex(table: "dbo.DeviceMapDefinitions", name: "IX_MapDefinition_Id", newName: "IX_Definition_Id");
            RenameColumn(table: "dbo.DeviceMapDefinitions", name: "MapDefinition_Id", newName: "Definition_Id");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id");
        }
    }
}
