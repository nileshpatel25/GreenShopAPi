namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class quantityfactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnitFactorNames",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        unitfactorname = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UnitQuantityFactors",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        unitfactornameid = c.String(),
                        unitname = c.String(),
                        quantityfactor = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.ordermasters", "estimatedeliverydate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.ordermasters", "estimatedeliverytime", c => c.String());
            AddColumn("dbo.Products", "unitfactorid", c => c.String());
            AddColumn("dbo.Products", "wholesellerPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "wholesellerdiscountper", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "wholesellerdiscountprice", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "premiumPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "premiumdiscountper", c => c.Double(nullable: false));
            AddColumn("dbo.Products", "premiumdiscountprice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "premiumdiscountprice");
            DropColumn("dbo.Products", "premiumdiscountper");
            DropColumn("dbo.Products", "premiumPrice");
            DropColumn("dbo.Products", "wholesellerdiscountprice");
            DropColumn("dbo.Products", "wholesellerdiscountper");
            DropColumn("dbo.Products", "wholesellerPrice");
            DropColumn("dbo.Products", "unitfactorid");
            DropColumn("dbo.ordermasters", "estimatedeliverytime");
            DropColumn("dbo.ordermasters", "estimatedeliverydate");
            DropTable("dbo.UnitQuantityFactors");
            DropTable("dbo.UnitFactorNames");
        }
    }
}
