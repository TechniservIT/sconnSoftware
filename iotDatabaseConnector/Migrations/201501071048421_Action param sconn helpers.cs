namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Actionparamsconnhelpers : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.sconnConfigMappers", name: "ActionParameter_ParameterId", newName: "ActionParam_ParameterId");
            RenameIndex(table: "dbo.sconnConfigMappers", name: "IX_ActionParameter_ParameterId", newName: "IX_ActionParam_ParameterId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.sconnConfigMappers", name: "IX_ActionParam_ParameterId", newName: "IX_ActionParameter_ParameterId");
            RenameColumn(table: "dbo.sconnConfigMappers", name: "ActionParam_ParameterId", newName: "ActionParameter_ParameterId");
        }
    }
}
