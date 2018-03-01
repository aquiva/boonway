namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectNicheCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectNiche", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectNiche");
        }
    }
}
