namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unitanemordedetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.orderdetails", "unitname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.orderdetails", "unitname");
        }
    }
}
