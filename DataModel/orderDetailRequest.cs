using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class orderDetailRequest
    {
        public string Id { get; set; }
        public string orderid { get; set; }
        public string productid { get; set; }
        public string unitname { get; set; }
        public double quantity { get; set; }
        public double unitprice { get; set; }
        public double price { get; set; }
        public double discountper { get; set; }
        public double discountprice { get; set; }
        public string taxslabid { get; set; }
        public double totaltax { get; set; }
        public double totalprice { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}