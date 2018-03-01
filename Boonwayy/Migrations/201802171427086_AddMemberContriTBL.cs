namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMemberContriTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberContributions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountInCents = c.Int(nullable: false),
                        MERFID = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                        ContributionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MERFs", t => t.MERFID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.MERFID)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberContributions", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MemberContributions", "MERFID", "dbo.MERFs");
            DropIndex("dbo.MemberContributions", new[] { "ApplicationUserId" });
            DropIndex("dbo.MemberContributions", new[] { "MERFID" });
            DropTable("dbo.MemberContributions");
        }
    }
}
