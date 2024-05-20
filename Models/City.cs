using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class City
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }

    }
}