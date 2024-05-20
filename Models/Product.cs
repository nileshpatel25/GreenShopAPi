using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class Product
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
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime updateAt { get; set; }
    }
}