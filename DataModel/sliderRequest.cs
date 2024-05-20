using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class sliderRequest
    {
        public string id { get; set; }
        public string code { get; set; }
        public string image { get; set; }
        public bool deleted { get; set; }
        public bool active { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public double orderno { get; set; }
    }
}