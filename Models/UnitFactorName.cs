using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class UnitFactorName
    {
        public string id { get; set; }
        public string unitfactorname { get; set; }
        public bool deleted { get; set; }

        public DateTime createdAt { get; set; }
    }
}