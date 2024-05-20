using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class appversionRequest
    {
        public string version { get; set; }
        public bool forceUpdate { get; set; }
    }
}