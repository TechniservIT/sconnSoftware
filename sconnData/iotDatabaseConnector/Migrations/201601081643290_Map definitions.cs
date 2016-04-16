namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mapdefinitions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        Device_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.DeviceMapDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceId = c.Int(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Definition_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapDefinitions", t => t.Definition_Id, cascadeDelete: true)
                .Index(t => t.Definition_Id);
            
            CreateTable(
                "dbo.IoMapDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IoId = c.Int(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Definition_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceMapDefinitions", t => t.Definition_Id, cascadeDelete: true)
                .Index(t => t.Definition_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IoMapDefinitions", "Definition_Id", "dbo.DeviceMapDefinitions");
            DropForeignKey("dbo.DeviceMapDefinitions", "Definition_Id", "dbo.MapDefinitions");
            DropForeignKey("dbo.MapDefinitions", "Device_Id", "dbo.Devices");
            DropIndex("dbo.IoMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.DeviceMapDefinitions", new[] { "Definition_Id" });
            DropIndex("dbo.MapDefinitions", new[] { "Device_Id" });
            DropTable("dbo.IoMapDefinitions");
            DropTable("dbo.DeviceMapDefinitions");
            DropTable("dbo.MapDefinitions");
        }
    }
}
