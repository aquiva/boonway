namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewDb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Donations", "DonatorName", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Donations", "City", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Donations", "City", c => c.String());
            AlterColumn("dbo.Donations", "Email", c => c.String());
            AlterColumn("dbo.Donations", "DonatorName", c => c.String());
        }
    }
}
