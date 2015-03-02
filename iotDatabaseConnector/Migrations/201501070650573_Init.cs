namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionParameters",
                c => new
                    {
                        ParameterId = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Action_ActionId = c.Int(),
                        Type_ParameterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParameterId)
                .ForeignKey("dbo.DeviceActions", t => t.Action_ActionId)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_ParameterId, cascadeDelete: true)
                .Index(t => t.Action_ActionId)
                .Index(t => t.Type_ParameterId);
            
            CreateTable(
                "dbo.DeviceActions",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionName = c.String(nullable: false),
                        ActionDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                        LastActivationTime = c.DateTime(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false),
                        DeviceName = c.String(nullable: false),
                        Credentials_CredentialId = c.Int(),
                        Site_SiteId = c.Int(),
                        DeviceLocation_LocationId = c.Int(nullable: false),
                        Type_DeviceTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.DeviceCredentials", t => t.Credentials_CredentialId)
                .ForeignKey("dbo.Sites", t => t.Site_SiteId)
                .ForeignKey("dbo.Locations", t => t.DeviceLocation_LocationId, cascadeDelete: true)
                .ForeignKey("dbo.EndpointInfoes", t => t.DeviceId)
                .ForeignKey("dbo.DeviceTypes", t => t.Type_DeviceTypeId, cascadeDelete: true)
                .Index(t => t.DeviceId)
                .Index(t => t.Credentials_CredentialId)
                .Index(t => t.Site_SiteId)
                .Index(t => t.DeviceLocation_LocationId)
                .Index(t => t.Type_DeviceTypeId);
            
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
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        LocationVisualRepresentationURL = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false),
                        Domain_DomainId = c.Int(nullable: false),
                        siteLocation_LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.iotDomains", t => t.Domain_DomainId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.siteLocation_LocationId)
                .Index(t => t.Domain_DomainId)
                .Index(t => t.siteLocation_LocationId);
            
            CreateTable(
                "dbo.iotDomains",
                c => new
                    {
                        DomainId = c.Int(nullable: false, identity: true),
                        DomainName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DomainId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Domain_DomainId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.iotDomains", t => t.Domain_DomainId, cascadeDelete: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Domain_DomainId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.EndpointInfoes",
                c => new
                    {
                        EndpointId = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.EndpointId);
            
            CreateTable(
                "dbo.DeviceProperties",
                c => new
                    {
                        PropertyId = c.Int(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false),
                        PropertyDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        Device_DeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PropertyId)
                .ForeignKey("dbo.Devices", t => t.Device_DeviceId, cascadeDelete: true)
                .Index(t => t.Device_DeviceId);
            
            CreateTable(
                "dbo.DeviceParameters",
                c => new
                    {
                        ParameterId = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        ParamDescription = c.String(),
                        VisualRepresentationUrl = c.String(),
                        Action_ActionId = c.Int(),
                        Property_PropertyId = c.Int(),
                        Type_ParameterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParameterId)
                .ForeignKey("dbo.DeviceActions", t => t.Action_ActionId)
                .ForeignKey("dbo.DeviceProperties", t => t.Property_PropertyId)
                .ForeignKey("dbo.ParameterTypes", t => t.Type_ParameterId, cascadeDelete: true)
                .Index(t => t.Action_ActionId)
                .Index(t => t.Property_PropertyId)
                .Index(t => t.Type_ParameterId);
            
            CreateTable(
                "dbo.sconnConfigMappers",
                c => new
                    {
                        MapperId = c.Int(nullable: false, identity: true),
                        ConfigType = c.Int(nullable: false),
                        SeqNumber = c.Int(nullable: false),
                        Parameter_ParameterId = c.Int(),
                        ActionParameter_ParameterId = c.Int(),
                    })
                .PrimaryKey(t => t.MapperId)
                .ForeignKey("dbo.DeviceParameters", t => t.Parameter_ParameterId)
                .ForeignKey("dbo.ActionParameters", t => t.ActionParameter_ParameterId)
                .Index(t => t.Parameter_ParameterId)
                .Index(t => t.ActionParameter_ParameterId);
            
            CreateTable(
                "dbo.ParameterTypes",
                c => new
                    {
                        ParameterId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DocumentationURL = c.String(),
                    })
                .PrimaryKey(t => t.ParameterId);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        DeviceTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false),
                        TypeDescription = c.String(),
                        VisualRepresentationURL = c.String(),
                    })
                .PrimaryKey(t => t.DeviceTypeId);
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        PerimissionId = c.Int(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.PerimissionId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ActionParameters", "Type_ParameterId", "dbo.ParameterTypes");
            DropForeignKey("dbo.sconnConfigMappers", "ActionParameter_ParameterId", "dbo.ActionParameters");
            DropForeignKey("dbo.ActionParameters", "Action_ActionId", "dbo.DeviceActions");
            DropForeignKey("dbo.DeviceActions", "Device_DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "Type_DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.DeviceParameters", "Type_ParameterId", "dbo.ParameterTypes");
            DropForeignKey("dbo.sconnConfigMappers", "Parameter_ParameterId", "dbo.DeviceParameters");
            DropForeignKey("dbo.DeviceParameters", "Property_PropertyId", "dbo.DeviceProperties");
            DropForeignKey("dbo.DeviceParameters", "Action_ActionId", "dbo.DeviceActions");
            DropForeignKey("dbo.DeviceProperties", "Device_DeviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "DeviceId", "dbo.EndpointInfoes");
            DropForeignKey("dbo.Devices", "DeviceLocation_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Sites", "siteLocation_LocationId", "dbo.Locations");
            DropForeignKey("dbo.Sites", "Domain_DomainId", "dbo.iotDomains");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Domain_DomainId", "dbo.iotDomains");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Devices", "Site_SiteId", "dbo.Sites");
            DropForeignKey("dbo.Devices", "Credentials_CredentialId", "dbo.DeviceCredentials");
            DropForeignKey("dbo.DeviceCredentials", "AuthLevel_AppAuthLevelId", "dbo.AppAuthLevels");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.sconnConfigMappers", new[] { "ActionParameter_ParameterId" });
            DropIndex("dbo.sconnConfigMappers", new[] { "Parameter_ParameterId" });
            DropIndex("dbo.DeviceParameters", new[] { "Type_ParameterId" });
            DropIndex("dbo.DeviceParameters", new[] { "Property_PropertyId" });
            DropIndex("dbo.DeviceParameters", new[] { "Action_ActionId" });
            DropIndex("dbo.DeviceProperties", new[] { "Device_DeviceId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Domain_DomainId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Sites", new[] { "siteLocation_LocationId" });
            DropIndex("dbo.Sites", new[] { "Domain_DomainId" });
            DropIndex("dbo.DeviceCredentials", new[] { "AuthLevel_AppAuthLevelId" });
            DropIndex("dbo.Devices", new[] { "Type_DeviceTypeId" });
            DropIndex("dbo.Devices", new[] { "DeviceLocation_LocationId" });
            DropIndex("dbo.Devices", new[] { "Site_SiteId" });
            DropIndex("dbo.Devices", new[] { "Credentials_CredentialId" });
            DropIndex("dbo.Devices", new[] { "DeviceId" });
            DropIndex("dbo.DeviceActions", new[] { "Device_DeviceId" });
            DropIndex("dbo.ActionParameters", new[] { "Type_ParameterId" });
            DropIndex("dbo.ActionParameters", new[] { "Action_ActionId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.ParameterTypes");
            DropTable("dbo.sconnConfigMappers");
            DropTable("dbo.DeviceParameters");
            DropTable("dbo.DeviceProperties");
            DropTable("dbo.EndpointInfoes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.iotDomains");
            DropTable("dbo.Sites");
            DropTable("dbo.Locations");
            DropTable("dbo.AppAuthLevels");
            DropTable("dbo.DeviceCredentials");
            DropTable("dbo.Devices");
            DropTable("dbo.DeviceActions");
            DropTable("dbo.ActionParameters");
        }
    }
}
