namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIdColumn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ERFDonations", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ERFDonations", new[] { "ProjectId" });
            AddColumn("dbo.ERFDonations", "MERFID", c => c.Int(nullable: false));
            CreateIndex("dbo.ERFDonations", "MERFID");
            AddForeignKey("dbo.ERFDonations", "MERFID", "dbo.MERFs", "Id", cascadeDelete: true);
            DropColumn("dbo.ERFDonations", "ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ERFDonations", "ProjectId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ERFDonations", "MERFID", "dbo.MERFs");
            DropIndex("dbo.ERFDonations", new[] { "MERFID" });
            DropColumn("dbo.ERFDonations", "MERFID");
            CreateIndex("dbo.ERFDonations", "ProjectId");
            AddForeignKey("dbo.ERFDonations", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
