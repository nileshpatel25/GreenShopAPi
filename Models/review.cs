using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class review
    {
        public string id { get; set; }
        public string productid { get; set; }
        public string name { get; set; }
        public string reviewdetails { get; set; }
        public int starcount { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
    }
}