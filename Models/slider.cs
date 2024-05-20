using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class slider
    {
        public string id { get; set; }
        public string code { get; set; }
        public string image { get; set; }
        public bool deleted { get; set; }
        public bool active { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime fromdate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime todate { get; set; }
        public double orderno { get; set; }
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime updateAt { get; set; }
    }
}