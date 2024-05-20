namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class registrationdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "registrationdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "registrationdate");
        }
    }
}
