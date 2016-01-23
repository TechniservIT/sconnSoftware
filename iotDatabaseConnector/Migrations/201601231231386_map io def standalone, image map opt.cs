namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mapiodefstandaloneimagemapopt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceMapDefinitions", "Definition_Id", "dbo.MapDefinitions");
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropIndex("dbo.DeviceMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            AddColumn("dbo.IoMapDefinitions", "DeviceId", c => c.Int(nullable: false));
            AddColumn("dbo.IoMapDefinitions", "IsImageMap", c => c.Boolean(nullable: false));
            AddColumn("dbo.IoMapDefinitions", "ImageMapUrl", c => c.String());
            AlterColumn("dbo.DeviceMapDefinitions", "Definition_Id", c => c.Int());
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int());
            CreateIndex("dbo.DeviceMapDefinitions", "Definition_Id");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            AddForeignKey("dbo.DeviceMapDefinitions", "Definition_Id", "dbo.MapDefinitions", "Id");
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropForeignKey("dbo.DeviceMapDefinitions", "Definition_Id", "dbo.MapDefinitions");
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.DeviceMapDefinitions", new[] { "Definition_Id" });
            AlterColumn("dbo.IoMapDefinitions", "Definition_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.DeviceMapDefinitions", "Definition_Id", c => c.Int(nullable: false));
            DropColumn("dbo.IoMapDefinitions", "ImageMapUrl");
            DropColumn("dbo.IoMapDefinitions", "IsImageMap");
            DropColumn("dbo.IoMapDefinitions", "DeviceId");
            CreateIndex("dbo.IoMapDefinitions", "Definition_Id");
            CreateIndex("dbo.DeviceMapDefinitions", "Definition_Id");
            AddForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DeviceMapDefinitions", "Definition_Id", "dbo.MapDefinitions", "Id", cascadeDelete: true);
        }
    }
}
