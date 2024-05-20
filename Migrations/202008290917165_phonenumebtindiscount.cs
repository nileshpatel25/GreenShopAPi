namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phonenumebtindiscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountUserSpecifics", "PhoneNumber", c => c.String());
            DropColumn("dbo.DiscountUserSpecifics", "mobileno");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DiscountUserSpecifics", "mobileno", c => c.String());
            DropColumn("dbo.DiscountUserSpecifics", "PhoneNumber");
        }
    }
}
