namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectCategoryCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectCategory");
        }
    }
}
