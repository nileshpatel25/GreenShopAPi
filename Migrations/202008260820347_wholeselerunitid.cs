namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wholeselerunitid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "wholesellerunitid", c => c.String());
            AddColumn("dbo.Products", "wholesellerunitfactorid", c => c.String());
            AddColumn("dbo.Products", "premiumunitid", c => c.String());
            AddColumn("dbo.Products", "premiumunitfactorid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "premiumunitfactorid");
            DropColumn("dbo.Products", "premiumunitid");
            DropColumn("dbo.Products", "wholesellerunitfactorid");
            DropColumn("dbo.Products", "wholesellerunitid");
        }
    }
}
