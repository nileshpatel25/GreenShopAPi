using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class DiscountMenuSpecific
    {
        public string id { get; set; }
        public string discountid { get; set; }
        public string vendorid { get; set; }
        public string productid { get; set; }
        public string name { get; set; }
    }
}