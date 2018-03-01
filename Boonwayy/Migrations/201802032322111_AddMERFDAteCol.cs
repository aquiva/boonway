namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMERFDAteCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MERFs", "MERFStartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MERFs", "MERFStartDate");
        }
    }
}
