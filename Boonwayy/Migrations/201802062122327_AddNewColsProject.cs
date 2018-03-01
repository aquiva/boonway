namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColsProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "City", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "State", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "Country", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Country");
            DropColumn("dbo.Projects", "State");
            DropColumn("dbo.Projects", "City");
        }
    }
}
