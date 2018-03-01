namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReqAttUserProfile : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "SocialSecurityNumber", c => c.String(nullable: false));
            AlterColumn("dbo.UserProfiles", "Profession", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "Profession", c => c.String());
            AlterColumn("dbo.UserProfiles", "SocialSecurityNumber", c => c.String());
        }
    }
}
