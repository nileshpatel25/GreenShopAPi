namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pcefactorinunitfactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnitQuantityFactors", "pricefactor", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UnitQuantityFactors", "pricefactor");
        }
    }
}
