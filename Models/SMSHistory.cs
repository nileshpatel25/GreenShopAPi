using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class SMSHistory
    {
        public string id { get; set; }
        public string vendorId { get; set; }
        public string mobileno { get; set; }
        public string message { get; set; }
        public DateTime createddt { get; set; }
    }
}