namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Devicecategoryenum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceTypes", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceTypes", "Category");
        }
    }
}
