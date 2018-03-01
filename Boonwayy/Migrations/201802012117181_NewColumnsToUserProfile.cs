namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewColumnsToUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "SocialSecurityNumber", c => c.String());
            AddColumn("dbo.UserProfiles", "State", c => c.String());
            AddColumn("dbo.UserProfiles", "Address", c => c.String());
            AddColumn("dbo.UserProfiles", "Profession", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Profession");
            DropColumn("dbo.UserProfiles", "Address");
            DropColumn("dbo.UserProfiles", "State");
            DropColumn("dbo.UserProfiles", "SocialSecurityNumber");
        }
    }
}
