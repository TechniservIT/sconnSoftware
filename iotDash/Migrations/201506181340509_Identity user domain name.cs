namespace iotDash.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identityuserdomainname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DomainId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DomainId", c => c.Int(nullable: false));
        }
    }
}
