namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forceupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appversions", "forceUpdate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appversions", "forceUpdate");
        }
    }
}
