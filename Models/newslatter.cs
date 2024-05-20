using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class newslatter
    {
        public string id { get; set; }
        public string emailid { get; set; }
        public bool active { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
    }
}