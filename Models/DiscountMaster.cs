using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class DiscountMaster
    {
        public string id { get; set; }
        public string vendorid { get; set; }
        public string title { get; set; }
        public string discounton { get; set; }
        public string type { get; set; }
        public string couponcode { get; set; }
        public int couponmaxuse { get; set; }
        public double discountper { get; set; }
        public double discountamt { get; set; }
        public double maxdiscountamt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime fromdate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime todate { get; set; }
        public string conditionaldiscount { get; set; }
        public double mindiscountamt { get; set; }
        public string status { get; set; }
        public bool deleted { get; set; }
        public DateTime createdAt { get; set; }
    }
}