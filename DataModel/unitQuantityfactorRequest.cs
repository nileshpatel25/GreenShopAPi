using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class unitQuantityfactorRequest
    {
        public string id { get; set; }
        public string unitfactornameid { get; set; }
        public string unitname { get; set; }
        public double quantityfactor { get; set; }
        public double pricefactor { get; set; }
    }
}