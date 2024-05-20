using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class TaxSlabMaster
    {
        public string Id { get; set; }
        public string stSlabName { get; set; }
        public double CGSTHome { get; set; }
        public double SGSTHome { get; set; }
        public double IGSTHome { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime updateAt { get; set; }
    }
}