namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parameterchangehistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParameterChangeHistories",
                c => new
                    {
                        ParameterChangeId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.String(nullable: false),
                        Property_ParameterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParameterChangeId)
                .ForeignKey("dbo.DeviceParameters", t => t.Property_ParameterId, cascadeDelete: true)
                .Index(t => t.Property_ParameterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParameterChangeHistories", "Property_ParameterId", "dbo.DeviceParameters");
            DropIndex("dbo.ParameterChangeHistories", new[] { "Property_ParameterId" });
            DropTable("dbo.ParameterChangeHistories");
        }
    }
}
