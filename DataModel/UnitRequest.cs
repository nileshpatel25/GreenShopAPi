using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class UnitRequest
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public bool deleted { get; set; }
    }
}