namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateAndStatusCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "ProjectEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "ProjectStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectStatus");
            DropColumn("dbo.Projects", "ProjectEndDate");
            DropColumn("dbo.Projects", "ProjectStartDate");
        }
    }
}
