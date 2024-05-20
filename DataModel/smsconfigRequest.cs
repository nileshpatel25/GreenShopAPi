using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiGreenShop.DataModel
{
    public class smsconfigRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updateAt { get; set; }
    }
}