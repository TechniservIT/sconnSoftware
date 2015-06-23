namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Action_Id = c.Int(),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceActions", t => t.Action_Id)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_Id, cascadeDelete: true)
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
                        Id = c.Int(nullable: false),
                        DeviceName = c.String(nullable: false),
                        Credentials_CredentialId = c.Int(),
                        Site_Id = c.Int(),
                        DeviceLocation_Id = c.Int(nullable: false),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceCredentials", t => t.Credentials_CredentialId)
                .ForeignKey("dbo.Sites", t => t.Site_Id)
                .ForeignKey("dbo.Locations", t => t.DeviceLocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.EndpointInfoes", t => t.Id)
                .ForeignKey("dbo.DeviceTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Credentials_CredentialId)
                .Index(t => t.Site_Id)
                .Index(t => t.DeviceLocation_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.DeviceCredentials",
                c => new
                    {
                        CredentialId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        HashData = c.Binary(),
                        PermissionExpireDate = c.DateTime(nullable: false),
                        PasswordExpireDate = c.DateTime(nullable: false),
                        AuthLevel_AppAuthLevelId = c.Int(),
                    })
                .PrimaryKey(t => t.CredentialId)
                .ForeignKey("dbo.AppAuthLevels", t => t.AuthLevel_AppAuthLevelId)
                .Index(t => t.AuthLevel_AppAuthLevelId);
            
            CreateTable(
                "dbo.AppAuthLevels",
                c => new
                    {
                        AppAuthLevelId = c.Int(nullable: false, identity: true),
                        Write = c.Boolean(nullable: false),
                        Read = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AppAuthLevelId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        LocationVisualRepresentationURL = c.String(),
                        Domain_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.iotDomains",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                        Domain_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id, cascadeDelete: true)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false),
                        Domain_Id = c.String(nullable: false, maxLength: 128),
                        siteLocation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_Id, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.siteLocation_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.siteLocation_Id);
            
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
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Action_Id = c.Int(),
                        Property_Id = c.Int(),
                        Type_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceActions", t => t.Action_Id)
                .ForeignKey("dbo.DeviceProperties", t => t.Property_Id)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_Id, cascadeDelete: true)
                .Index(t => t.Action_Id)
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
                "dbo.sconnConfigMappers",
                c => new
                    {
                        MapperId = c.Int(nullable: false, identity: true),
                        ConfigType = c.Int(nullable: false),
                        SeqNumber = c.Int(nullable: false),
                        ActionParam_Id = c.Int(),
                        Parameter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.MapperId)
                .ForeignKey("dbo.ActionParameters", t => t.ActionParam_Id)
                .ForeignKey("dbo.DeviceParameters", t => t.Parameter_Id)
                .Index(t => t.ActionParam_Id)
                .Index(t => t.Parameter_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActionParameters", "Type_Id", "dbo.ParameterTypes");
            DropForeignKey("dbo.ActionParameters", "Action_Id", "dbo.DeviceActions");
            DropForeignKey("dbo.DeviceActions", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Type_Id", "dbo.DeviceTypes");
            DropForeignKey("dbo.DeviceParameters", "Type_Id", "dbo.ParameterTypes");
            DropForeignKey("dbo.sconnConfigMappers", "Parameter_Id", "dbo.DeviceParameters");
            DropForeignKey("dbo.sconnConfigMappers", "ActionParam_Id", "dbo.ActionParameters");
            DropForeignKey("dbo.DeviceParameters", "Property_Id", "dbo.DeviceProperties");
            DropForeignKey("dbo.ParameterChangeHistories", "Property_Id", "dbo.DeviceParameters");
            DropForeignKey("dbo.DeviceParameters", "Action_Id", "dbo.DeviceActions");
            DropForeignKey("dbo.DeviceProperties", "Device_Id", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Id", "dbo.EndpointInfoes");
            DropForeignKey("dbo.Devices", "DeviceLocation_Id", "dbo.Locations");
            DropForeignKey("dbo.Sites", "siteLocation_Id", "dbo.Locations");
            DropForeignKey("dbo.Sites", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.Devices", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.Locations", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.DeviceTypes", "Domain_Id", "dbo.iotDomains");
            DropForeignKey("dbo.Devices", "Credentials_CredentialId", "dbo.DeviceCredentials");
            DropForeignKey("dbo.DeviceCredentials", "AuthLevel_AppAuthLevelId", "dbo.AppAuthLevels");
            DropIndex("dbo.sconnConfigMappers", new[] { "Parameter_Id" });
            DropIndex("dbo.sconnConfigMappers", new[] { "ActionParam_Id" });
            DropIndex("dbo.ParameterChangeHistories", new[] { "Property_Id" });
            DropIndex("dbo.DeviceParameters", new[] { "Type_Id" });
            DropIndex("dbo.DeviceParameters", new[] { "Property_Id" });
            DropIndex("dbo.DeviceParameters", new[] { "Action_Id" });
            DropIndex("dbo.DeviceProperties", new[] { "Device_Id" });
            DropIndex("dbo.Sites", new[] { "siteLocation_Id" });
            DropIndex("dbo.Sites", new[] { "Domain_Id" });
            DropIndex("dbo.DeviceTypes", new[] { "Domain_Id" });
            DropIndex("dbo.Locations", new[] { "Domain_Id" });
            DropIndex("dbo.DeviceCredentials", new[] { "AuthLevel_AppAuthLevelId" });
            DropIndex("dbo.Devices", new[] { "Type_Id" });
            DropIndex("dbo.Devices", new[] { "DeviceLocation_Id" });
            DropIndex("dbo.Devices", new[] { "Site_Id" });
            DropIndex("dbo.Devices", new[] { "Credentials_CredentialId" });
            DropIndex("dbo.Devices", new[] { "Id" });
            DropIndex("dbo.DeviceActions", new[] { "Device_Id" });
            DropIndex("dbo.ActionParameters", new[] { "Type_Id" });
            DropIndex("dbo.ActionParameters", new[] { "Action_Id" });
            DropTable("dbo.ParameterTypes");
            DropTable("dbo.sconnConfigMappers");
            DropTable("dbo.ParameterChangeHistories");
            DropTable("dbo.DeviceParameters");
            DropTable("dbo.DeviceProperties");
            DropTable("dbo.EndpointInfoes");
            DropTable("dbo.Sites");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.iotDomains");
            DropTable("dbo.Locations");
            DropTable("dbo.AppAuthLevels");
            DropTable("dbo.DeviceCredentials");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceActions");
            DropTable("dbo.ActionParameters");
        }
    }
}
