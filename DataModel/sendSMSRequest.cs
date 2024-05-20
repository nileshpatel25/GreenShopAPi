using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class sendSMSRequest
    {
        public string id { get; set; }
        public string vendorId { get; set; }
        public string message { get; set; }
    }
}