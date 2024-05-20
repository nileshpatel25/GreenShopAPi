using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class productimages
    {
        public string id { get; set; }
        public string productid { get; set; }
        public string image { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime updateAt { get; set; }
    }
}