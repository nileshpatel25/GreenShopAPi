namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smslink : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.newslatters",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        emailid = c.String(),
                        active = c.Boolean(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.reviews",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        productid = c.String(),
                        name = c.String(),
                        reviewdetails = c.String(),
                        starcount = c.Int(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SMSLinks",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        code = c.String(),
                        link = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SMSLinks");
            DropTable("dbo.reviews");
            DropTable("dbo.newslatters");
        }
    }
}
