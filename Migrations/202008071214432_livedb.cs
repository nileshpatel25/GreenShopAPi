namespace apiGreenShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class livedb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        address = c.String(),
                        address2 = c.String(),
                        landmark = c.String(),
                        city = c.String(),
                        pincode = c.String(),
                        remark = c.String(),
                        deleted = c.Boolean(nullable: false),
                        creatAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.cartdetails",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        browserId = c.String(),
                        httpContextSessionId = c.String(),
                        customerId = c.String(),
                        productId = c.String(),
                        quantity = c.Double(nullable: false),
                        unitprice = c.Double(nullable: false),
                        price = c.Double(nullable: false),
                        discountper = c.Double(nullable: false),
                        discountprice = c.Double(nullable: false),
                        taxslabid = c.String(),
                        totaltax = c.Double(nullable: false),
                        totalprice = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        subcategoryid = c.String(),
                        subsubcategoryid = c.String(),
                        image = c.String(),
                        deleted = c.Boolean(nullable: false),
                        orderno = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DiscountMasters",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        vendorid = c.String(),
                        title = c.String(),
                        discounton = c.String(),
                        type = c.String(),
                        couponcode = c.String(),
                        couponmaxuse = c.Int(nullable: false),
                        discountper = c.Double(nullable: false),
                        discountamt = c.Double(nullable: false),
                        maxdiscountamt = c.Double(nullable: false),
                        fromdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        todate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        conditionaldiscount = c.String(),
                        mindiscountamt = c.Double(nullable: false),
                        status = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createdAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DiscountMenuSpecifics",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        discountid = c.String(),
                        vendorid = c.String(),
                        productid = c.String(),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.DiscountUserSpecifics",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        discountid = c.String(),
                        vendorid = c.String(),
                        mobileno = c.String(),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Emailconfigurations",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        subject = c.String(),
                        body = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.orderdetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        orderid = c.String(),
                        productid = c.String(),
                        quantity = c.Double(nullable: false),
                        unitprice = c.Double(nullable: false),
                        price = c.Double(nullable: false),
                        discountper = c.Double(nullable: false),
                        discountprice = c.Double(nullable: false),
                        taxslabid = c.String(),
                        totaltax = c.Double(nullable: false),
                        totalprice = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ordermasters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        customerid = c.String(),
                        orderno = c.String(),
                        emailid = c.String(),
                        name = c.String(),
                        address = c.String(),
                        city = c.String(),
                        pincode = c.String(),
                        landmark = c.String(),
                        phoneno = c.String(),
                        namebilling = c.String(),
                        addressbilling = c.String(),
                        citybilling = c.String(),
                        pincodebilling = c.String(),
                        landmarkbilling = c.String(),
                        phonenobilling = c.String(),
                        paymenttype = c.String(),
                        flgIsPaymentDone = c.Boolean(nullable: false),
                        transectionId = c.String(),
                        bankname = c.String(),
                        transectiondate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        transectionRemarks = c.String(),
                        shipmentstatus = c.String(),
                        inCourierComId = c.String(),
                        shippingTrackingId = c.String(),
                        orderAcceptDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        orderPackedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        orderDispatchedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        deliveryDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        flgIsCancelRequest = c.Boolean(nullable: false),
                        cancleDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        flgIsReturn = c.Boolean(nullable: false),
                        returnDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        flgIsCallRequest = c.Boolean(nullable: false),
                        callNumber = c.String(),
                        remark = c.String(),
                        flgIsCancel = c.Boolean(nullable: false),
                        invoiceno = c.String(),
                        orderdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ordertype = c.String(),
                        status = c.String(),
                        tax = c.Double(nullable: false),
                        shippingCharge = c.Double(nullable: false),
                        discount = c.Double(nullable: false),
                        totalamount = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.productimages",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        productid = c.String(),
                        image = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        unitid = c.String(),
                        categoryid = c.String(),
                        subcategoryid = c.String(),
                        subsubcategoryid = c.String(),
                        intaxslabid = c.String(),
                        price = c.Double(nullable: false),
                        discountper = c.Double(nullable: false),
                        discountprice = c.Double(nullable: false),
                        discription = c.String(),
                        image = c.String(),
                        orderno = c.Double(nullable: false),
                        quantity = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        active = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromotionalMobileNoes",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        vendorId = c.String(),
                        mobileno = c.String(),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.pushnotificationids",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        userid = c.String(),
                        pushId = c.String(),
                        createAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.sliders",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        code = c.String(),
                        image = c.String(),
                        deleted = c.Boolean(nullable: false),
                        active = c.Boolean(nullable: false),
                        fromdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        todate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        orderno = c.Double(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.smsconfigurations",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        subject = c.String(),
                        body = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SMSHistories",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        vendorId = c.String(),
                        mobileno = c.String(),
                        message = c.String(),
                        createddt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.TaxSlabMasters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        stSlabName = c.String(),
                        CGSTHome = c.Double(nullable: false),
                        SGSTHome = c.Double(nullable: false),
                        IGSTHome = c.Double(nullable: false),
                        CGST = c.Double(nullable: false),
                        SGST = c.Double(nullable: false),
                        IGST = c.Double(nullable: false),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        deleted = c.Boolean(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        updateAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "firstname", c => c.String());
            AddColumn("dbo.AspNetUsers", "lastname", c => c.String());
            AddColumn("dbo.AspNetUsers", "address", c => c.String());
            AddColumn("dbo.AspNetUsers", "address2", c => c.String());
            AddColumn("dbo.AspNetUsers", "landmark", c => c.String());
            AddColumn("dbo.AspNetUsers", "city", c => c.String());
            AddColumn("dbo.AspNetUsers", "pincode", c => c.String());
            AddColumn("dbo.AspNetUsers", "othercontactno", c => c.String());
            AddColumn("dbo.AspNetUsers", "source", c => c.String());
            AddColumn("dbo.AspNetUsers", "profilepic", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "profilepic");
            DropColumn("dbo.AspNetUsers", "source");
            DropColumn("dbo.AspNetUsers", "othercontactno");
            DropColumn("dbo.AspNetUsers", "pincode");
            DropColumn("dbo.AspNetUsers", "city");
            DropColumn("dbo.AspNetUsers", "landmark");
            DropColumn("dbo.AspNetUsers", "address2");
            DropColumn("dbo.AspNetUsers", "address");
            DropColumn("dbo.AspNetUsers", "lastname");
            DropColumn("dbo.AspNetUsers", "firstname");
            DropTable("dbo.Units");
            DropTable("dbo.TaxSlabMasters");
            DropTable("dbo.SMSHistories");
            DropTable("dbo.smsconfigurations");
            DropTable("dbo.sliders");
            DropTable("dbo.pushnotificationids");
            DropTable("dbo.PromotionalMobileNoes");
            DropTable("dbo.Products");
            DropTable("dbo.productimages");
            DropTable("dbo.ordermasters");
            DropTable("dbo.orderdetails");
            DropTable("dbo.Emailconfigurations");
            DropTable("dbo.DiscountUserSpecifics");
            DropTable("dbo.DiscountMenuSpecifics");
            DropTable("dbo.DiscountMasters");
            DropTable("dbo.Cities");
            DropTable("dbo.Categories");
            DropTable("dbo.cartdetails");
            DropTable("dbo.Addresses");
        }
    }
}
