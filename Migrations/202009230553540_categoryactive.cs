namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "active");
        }
    }
}
