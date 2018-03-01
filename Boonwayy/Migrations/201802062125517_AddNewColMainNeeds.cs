namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColMainNeeds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectNeeds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "ProjectNeeds");
        }
    }
}
