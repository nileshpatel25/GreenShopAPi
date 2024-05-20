using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace apiGreenShop.Models
{
    public class ordermaster
    {
        public string Id { get; set; }
        public string customerid { get; set; }
        public string orderno { get; set; }
        public string emailid { get; set; }

        //filed for shipping address
        public string name { get; set; }

        public string address { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string landmark { get; set; }
        public string phoneno { get; set; }

        //filed for billing address
        public string namebilling { get; set; }

        public string addressbilling { get; set; }    
        public string citybilling { get; set; }
        public string pincodebilling { get; set; }
        public string landmarkbilling { get; set; }
        public string phonenobilling { get; set; }
        public string paymenttype { get; set; }
        public bool flgIsPaymentDone { get; set; }
        public string transectionId { get; set; }
        public string bankname { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime transectiondate { get; set; }
        public string transectionRemarks { get; set; }
        public string shipmentstatus { get; set; }
        public string inCourierComId { get; set; }
        public string shippingTrackingId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime orderAcceptDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime orderPackedDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime orderDispatchedDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime deliveryDate { get; set; }
        public bool flgIsCancelRequest { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime cancleDate { get; set; }
        public bool flgIsReturn { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime returnDate { get; set; }
        public bool flgIsCallRequest { get; set; }
        public string callNumber { get; set; }
        public string remark { get; set; }
        public bool flgIsCancel { get; set; }

        public string invoiceno { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime orderdate { get; set; }
        public string ordertype { get; set; }
        public string status { get; set; }
        public double tax { get; set; }
        public double shippingCharge { get; set; }
        public double discount { get; set; }
        public double totalamount { get; set; }
        [Column(TypeName ="datetime2")]
        //added for estimate deliverymanagement
        public DateTime estimatedeliverydate { get; set; }
        public string estimatedeliverytime { get; set; }
        //end
        public bool deleted { get; set; }
        public DateTime createAt { get; set; }

       
    }
}