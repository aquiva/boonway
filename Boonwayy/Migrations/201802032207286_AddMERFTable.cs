namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMERFTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MERFs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Story = c.String(nullable: false),
                        VideoUrl = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MERFs", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.MERFs", new[] { "ApplicationUserId" });
            DropTable("dbo.MERFs");
        }
    }
}
