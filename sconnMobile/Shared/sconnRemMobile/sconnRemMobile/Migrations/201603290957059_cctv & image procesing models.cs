namespace iotDatabaseConnector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cctvimageprocesingmodels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IpCameras", "Site_Id", c => c.Int());
            CreateIndex("dbo.IpCameras", "Site_Id");
            AddForeignKey("dbo.IpCameras", "Site_Id", "dbo.Sites", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IpCameras", "Site_Id", "dbo.Sites");
            DropIndex("dbo.IpCameras", new[] { "Site_Id" });
            DropColumn("dbo.IpCameras", "Site_Id");
        }
    }
}
