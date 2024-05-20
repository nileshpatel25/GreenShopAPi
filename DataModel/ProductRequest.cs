using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class ProductRequest
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string unitid { get; set; }
        public string unitfactorid { get; set; }
        public string categoryid { get; set; }
        public string subcategoryid { get; set; }
        public string subsubcategoryid { get; set; }
        public string intaxslabid { get; set; }
        public double price { get; set; }
        public double discountper { get; set; }
        public double discountprice { get; set; }
        public string wholesellerunitid { get; set; }
        public string wholesellerunitfactorid { get; set; }
        public double wholesellerPrice { get; set; }
        public double wholesellerdiscountper { get; set; }
        public double wholesellerdiscountprice { get; set; }
        public string premiumunitid { get; set; }
        public string premiumunitfactorid { get; set; }
        public double premiumPrice { get; set; }
        public double premiumdiscountper { get; set; }
        public double premiumdiscountprice { get; set; }
        public string discription { get; set; }
        public string image { get; set; }
        public double orderno { get; set; }
        public double quantity { get; set; }
        public bool deleted { get; set; }
        public bool active { get; set; }
    }
}