namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIDColsErf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ERFDonations", "MERFID", "dbo.MERFs");
            DropForeignKey("dbo.MemberContributions", "MERFID", "dbo.MERFs");
            DropIndex("dbo.ERFDonations", new[] { "MERFID" });
            DropIndex("dbo.MemberContributions", new[] { "MERFID" });
            DropColumn("dbo.ERFDonations", "MERFID");
            DropColumn("dbo.MemberContributions", "MERFID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberContributions", "MERFID", c => c.Int(nullable: false));
            AddColumn("dbo.ERFDonations", "MERFID", c => c.Int(nullable: false));
            CreateIndex("dbo.MemberContributions", "MERFID");
            CreateIndex("dbo.ERFDonations", "MERFID");
            AddForeignKey("dbo.MemberContributions", "MERFID", "dbo.MERFs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ERFDonations", "MERFID", "dbo.MERFs", "Id", cascadeDelete: true);
        }
    }
}
