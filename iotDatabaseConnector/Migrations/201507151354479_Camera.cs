namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Camera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IpCameras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccessUrl = c.String(),
                        RequiresAuth = c.Boolean(nullable: false),
                        Endpoint_Id = c.Int(),
                        Location_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EndpointInfoes", t => t.Endpoint_Id)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .Index(t => t.Endpoint_Id)
                .Index(t => t.Location_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IpCameras", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.IpCameras", "Endpoint_Id", "dbo.EndpointInfoes");
            DropIndex("dbo.IpCameras", new[] { "Location_Id" });
            DropIndex("dbo.IpCameras", new[] { "Endpoint_Id" });
            DropTable("dbo.IpCameras");
        }
    }
}
