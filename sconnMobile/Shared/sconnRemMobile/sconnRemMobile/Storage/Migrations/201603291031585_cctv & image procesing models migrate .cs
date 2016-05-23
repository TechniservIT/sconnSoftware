namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cctvimageprocesingmodelsmigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SurveillanceAnalysisConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IpCameras", t => t.Source_Id)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.SurveillanceEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IpCameras", t => t.Source_Id)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.SurveillanceRecordings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        RecordingSetup_Id = c.Int(),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SurveillanceRecordingSetups", t => t.RecordingSetup_Id)
                .ForeignKey("dbo.IpCameras", t => t.Source_Id)
                .Index(t => t.RecordingSetup_Id)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.SurveillanceRecordingSetups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Source_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IpCameras", t => t.Source_Id)
                .Index(t => t.Source_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveillanceRecordings", "Source_Id", "dbo.IpCameras");
            DropForeignKey("dbo.SurveillanceRecordings", "RecordingSetup_Id", "dbo.SurveillanceRecordingSetups");
            DropForeignKey("dbo.SurveillanceRecordingSetups", "Source_Id", "dbo.IpCameras");
            DropForeignKey("dbo.SurveillanceEvents", "Source_Id", "dbo.IpCameras");
            DropForeignKey("dbo.SurveillanceAnalysisConfigs", "Source_Id", "dbo.IpCameras");
            DropIndex("dbo.SurveillanceRecordingSetups", new[] { "Source_Id" });
            DropIndex("dbo.SurveillanceRecordings", new[] { "Source_Id" });
            DropIndex("dbo.SurveillanceRecordings", new[] { "RecordingSetup_Id" });
            DropIndex("dbo.SurveillanceEvents", new[] { "Source_Id" });
            DropIndex("dbo.SurveillanceAnalysisConfigs", new[] { "Source_Id" });
            DropTable("dbo.SurveillanceRecordingSetups");
            DropTable("dbo.SurveillanceRecordings");
            DropTable("dbo.SurveillanceEvents");
            DropTable("dbo.SurveillanceAnalysisConfigs");
        }
    }
}
