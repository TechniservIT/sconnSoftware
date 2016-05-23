namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionChangeHistories",
                c => new
                    {
                        ParameterChangeId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.String(nullable: false),
                        Property_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParameterChangeId)
                .ForeignKey("dbo.DeviceActionResults", t => t.Property_Id, cascadeDelete: true)
                .Index(t => t.Property_Id);
            
            CreateTable(
                "dbo.DeviceActionResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Action_Id = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceActions", t => t.Action_Id, cascadeDelete: true)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_Id)
                .Index(t => t.Action_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.DeviceActions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionName = c.String(nullable: false),
                        ActionDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                        LastActivationTime = c.DateTime(nullable: false),
                        Device_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeviceName = c.String(nullable: false),
                        IsVirtual = c.Boolean(nullable: false),
                        Credentials_Id = c.Int(),
                        DeviceLocation_Id = c.Int(nullable: false),
                        EndpInfo_Id = c.Int(nullable: false),
                        Site_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceCredentials", t => t.Credentials_Id)
                .ForeignKey("dbo.Locations", t => t.DeviceLocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.EndpointInfoes", t => t.EndpInfo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.Site_Id, cascadeDelete: true)
                .ForeignKey("dbo.DeviceTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Credentials_Id)
                .Index(t => t.DeviceLocation_Id)
                .Index(t => t.EndpInfo_Id)
                .Index(t => t.Site_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.DeviceCredentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        HashData = c.Binary(),
                        PermissionExpireDate = c.DateTime(nullable: false),
                        PasswordExpireDate = c.DateTime(nullable: false),
                        AuthLevel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppAuthLevels", t => t.AuthLevel_Id)
                .Index(t => t.AuthLevel_Id);
            
            CreateTable(
                "dbo.AppAuthLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Write = c.Boolean(nullable: false),
                        Read = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        LocationVisualRepresentationURL = c.String(),
                        Domain_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id)
                .Index(t => t.Domain_Id);
            
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
            
            CreateTable(
                "dbo.EndpointInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Hostname = c.String(nullable: false),
                        Port = c.Int(nullable: false),
                        RequiresAuthentication = c.Boolean(nullable: false),
                        SupportsAllJoynProtocol = c.Boolean(nullable: false),
                        SupportsCoAPProtocol = c.Boolean(nullable: false),
                        SupportsMQTTProtocol = c.Boolean(nullable: false),
                        SupportsRESTfulProtocol = c.Boolean(nullable: false),
                        SupportsSconnProtocol = c.Boolean(nullable: false),
                        SupportsOnvifProtocol = c.Boolean(nullable: false),
                        Domain_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.iotDomains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DomainName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false),
                        TypeDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                        Domain_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id, cascadeDelete: false)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false),
                        Domain_Id = c.Int(nullable: false),
                        siteLocation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.siteLocation_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.siteLocation_Id);
            
            CreateTable(
                "dbo.DeviceProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false),
                        PropertyDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        Device_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Devices", t => t.Device_Id, cascadeDelete: true)
                .Index(t => t.Device_Id);
            
            CreateTable(
                "dbo.DeviceParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Property_Id = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceProperties", t => t.Property_Id, cascadeDelete: true)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_Id)
                .Index(t => t.Property_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.ParameterChangeHistories",
                c => new
                    {
                        ParameterChangeId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.String(nullable: false),
                        Property_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParameterChangeId)
                .ForeignKey("dbo.DeviceParameters", t => t.Property_Id, cascadeDelete: true)
                .Index(t => t.Property_Id);
            
            CreateTable(
                "dbo.ParameterTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DocumentationURL = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActionParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Action_Id = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceActions", t => t.Action_Id, cascadeDelete: true)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_Id)
                .Index(t => t.Action_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.sconnActionMappers",
                c => new
                    {
                        MapperId = c.Int(nullable: false, identity: true),
                        ConfigType = c.Int(nullable: false),
                        SeqNumber = c.Int(nullable: false),
                        ActionParam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.MapperId)
                .ForeignKey("dbo.ActionParameters", t => t.ActionParam_Id)
                .Index(t => t.ActionParam_Id);
            
            CreateTable(
                "dbo.sconnActionResultMappers",
                c => new
                    {
                        MapperId = c.Int(nullable: false, identity: true),
                        ConfigType = c.Int(nullable: false),
                        SeqNumber = c.Int(nullable: false),
                        ActionParam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.MapperId)
                .ForeignKey("dbo.DeviceActionResults", t => t.ActionParam_Id)
                .Index(t => t.ActionParam_Id);
            
            CreateTable(
                "dbo.sconnPropertyMappers",
                c => new
                    {
                        MapperId = c.Int(nullable: false, identity: true),
                        ConfigType = c.Int(nullable: false),
                        SeqNumber = c.Int(nullable: false),
                        Parameter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.MapperId)
                .ForeignKey("dbo.DeviceParameters", t => t.Parameter_Id)
                .Index(t => t.Parameter_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sconnPropertyMappers", "Parameter_Id", "dbo.DeviceParameters");
            DropForeignKey("dbo.ActionChangeHistories", "Property_Id", "dbo.DeviceActionResults");
            DropForeignKey("dbo.DeviceActionResults", "Type_Id", "dbo.ParameterTypes");
            DropForeignKey("dbo.sconnActionResultMappers", "ActionParam_Id", "dbo.DeviceActionResults");
            DropForeignKey("dbo.DeviceActionResults", "Action_Id", "dbo.DeviceActions");
            DropForeignKey("dbo.ActionParameters", "Type_Id", "dbo.ParameterTypes");
            DropForeignKey("dbo.sconnActionMappers", "ActionParam_Id", "dbo.ActionParameters");
            DropForeignKey("dbo.ActionParameters", "Action_Id", "dbo.DeviceActions");
            DropForeignKey("dbo.DeviceActions", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Type_Id", "dbo.DeviceTypes");
            DropForeignKey("dbo.Devices", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.DeviceParameters", "Type_Id", "dbo.ParameterTypes");
            DropForeignKey("dbo.DeviceParameters", "Property_Id", "dbo.DeviceProperties");
            DropForeignKey("dbo.ParameterChangeHistories", "Property_Id", "dbo.DeviceParameters");
            DropForeignKey("dbo.DeviceProperties", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.Devices", "EndpInfo_Id", "dbo.EndpointInfoes");
            DropForeignKey("dbo.Devices", "DeviceLocation_Id", "dbo.Locations");
            DropForeignKey("dbo.IpCameras", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Sites", "siteLocation_Id", "dbo.Locations");
            DropForeignKey("dbo.Sites", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.Locations", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.EndpointInfoes", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.DeviceTypes", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.IpCameras", "Endpoint_Id", "dbo.EndpointInfoes");
            DropForeignKey("dbo.Devices", "Credentials_Id", "dbo.DeviceCredentials");
            DropForeignKey("dbo.DeviceCredentials", "AuthLevel_Id", "dbo.AppAuthLevels");
            DropIndex("dbo.sconnPropertyMappers", new[] { "Parameter_Id" });
            DropIndex("dbo.sconnActionResultMappers", new[] { "ActionParam_Id" });
            DropIndex("dbo.sconnActionMappers", new[] { "ActionParam_Id" });
            DropIndex("dbo.ActionParameters", new[] { "Type_Id" });
            DropIndex("dbo.ActionParameters", new[] { "Action_Id" });
            DropIndex("dbo.ParameterChangeHistories", new[] { "Property_Id" });
            DropIndex("dbo.DeviceParameters", new[] { "Type_Id" });
            DropIndex("dbo.DeviceParameters", new[] { "Property_Id" });
            DropIndex("dbo.DeviceProperties", new[] { "Device_Id" });
            DropIndex("dbo.Sites", new[] { "siteLocation_Id" });
            DropIndex("dbo.Sites", new[] { "Domain_Id" });
            DropIndex("dbo.DeviceTypes", new[] { "Domain_Id" });
            DropIndex("dbo.EndpointInfoes", new[] { "Domain_Id" });
            DropIndex("dbo.IpCameras", new[] { "Location_Id" });
            DropIndex("dbo.IpCameras", new[] { "Endpoint_Id" });
            DropIndex("dbo.Locations", new[] { "Domain_Id" });
            DropIndex("dbo.DeviceCredentials", new[] { "AuthLevel_Id" });
            DropIndex("dbo.Devices", new[] { "Type_Id" });
            DropIndex("dbo.Devices", new[] { "Site_Id" });
            DropIndex("dbo.Devices", new[] { "EndpInfo_Id" });
            DropIndex("dbo.Devices", new[] { "DeviceLocation_Id" });
            DropIndex("dbo.Devices", new[] { "Credentials_Id" });
            DropIndex("dbo.DeviceActions", new[] { "Device_Id" });
            DropIndex("dbo.DeviceActionResults", new[] { "Type_Id" });
            DropIndex("dbo.DeviceActionResults", new[] { "Action_Id" });
            DropIndex("dbo.ActionChangeHistories", new[] { "Property_Id" });
            DropTable("dbo.sconnPropertyMappers");
            DropTable("dbo.sconnActionResultMappers");
            DropTable("dbo.sconnActionMappers");
            DropTable("dbo.ActionParameters");
            DropTable("dbo.ParameterTypes");
            DropTable("dbo.ParameterChangeHistories");
            DropTable("dbo.DeviceParameters");
            DropTable("dbo.DeviceProperties");
            DropTable("dbo.Sites");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.iotDomains");
            DropTable("dbo.EndpointInfoes");
            DropTable("dbo.IpCameras");
            DropTable("dbo.Locations");
            DropTable("dbo.AppAuthLevels");
            DropTable("dbo.DeviceCredentials");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceActions");
            DropTable("dbo.DeviceActionResults");
            DropTable("dbo.ActionChangeHistories");
        }
    }
}
