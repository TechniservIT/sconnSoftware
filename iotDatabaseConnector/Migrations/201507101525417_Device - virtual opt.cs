namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Devicevirtualopt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "IsVirtual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Devices", "IsVirtual");
        }
    }
}
