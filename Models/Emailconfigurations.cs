using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class Emailconfigurations
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime updateAt { get; set; }
    }
}