using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class address
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string addres { get; set; }
        public string address2 { get; set; }
        public string landmark { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string remark { get; set; }
        public bool deleted { get; set; }
        public DateTime creatAt { get; set; }
    }
}