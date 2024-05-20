using apiGreenShop.DataModel;
using apiGreenShop.Helper;
using apiGreenShop.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        SendSMS sendSMS = new SendSMS();
        PushNotificationLogic PushNotificationLogic = new PushNotificationLogic("");
        public ApplicationDbContext appDbContex { get; }



        // private readonly IMemoryCache memoryCache;
        public OrderController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        // TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("IndiaStandardTime");

        [HttpPost]
        [Route("addneworder")]
        public async Task<ResponseStatus> addneworder(orderRequest orderRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var guId = Guid.NewGuid();
                int? count = appDbContex.Ordermasters.ToList().Count() + 1;
                ordermaster ordermaster = new ordermaster
                {
                    Id = guId.ToString(),
                    // invoiceno = count.ToString(),
                    orderno = "OD" + count.ToString(),
                    customerid = orderRequest.customerid,
                    emailid = orderRequest.emailid,
                    name = orderRequest.name,
                    address = orderRequest.address,
                    city = orderRequest.city,
                    landmark = orderRequest.landmark,
                    pincode = orderRequest.pincode,
                    phoneno = orderRequest.phoneno,
                    namebilling = orderRequest.namebilling,
                    addressbilling = orderRequest.addressbilling,
                    citybilling = orderRequest.citybilling,
                    landmarkbilling = orderRequest.landmarkbilling,
                    pincodebilling = orderRequest.pincodebilling,
                    phonenobilling = orderRequest.phonenobilling,
                    paymenttype = orderRequest.paymenttype,
                    flgIsPaymentDone = orderRequest.flgIsPaymentDone,
                    transectionId = orderRequest.transectionId,
                    transectionRemarks = orderRequest.transectionRemarks,
                    bankname = orderRequest.bankname,
                    // transectiondate = orderRequest.tra,
                    shipmentstatus = orderRequest.shipmentstatus,
                    shippingCharge = orderRequest.shippingCharge,
                    orderdate = DateTime.UtcNow,
                    ordertype = "Cash On Delivery",
                    discount = orderRequest.discount,
                    status = orderRequest.status,
                    tax = orderRequest.tax,
                    totalamount = orderRequest.totalamount,
                    estimatedeliverydate = orderRequest.estimatedeliverydate,
                    estimatedeliverytime = orderRequest.estimatedeliverytime,
                    deleted = false,
                    createAt = DateTime.UtcNow



                };
                appDbContex.Ordermasters.Add(ordermaster);
                await appDbContex.SaveChangesAsync();

                // var people = System.Text.Json.JsonSerializer.Deserialize<List<orderdetails>>(json);


                for (int i = 0; i < orderRequest.orderdetails.Count; i++)
                {
                    var gId = Guid.NewGuid();
                    orderdetails orderdetails = new orderdetails
                    {
                        Id = gId.ToString(),
                        orderid = guId.ToString(),
                        productid = orderRequest.orderdetails[i].productid,
                        unitname = orderRequest.orderdetails[i].unitname,
                        quantity = orderRequest.orderdetails[i].quantity,
                        unitprice = orderRequest.orderdetails[i].unitprice,
                        price = orderRequest.orderdetails[i].price,
                        discountprice = orderRequest.orderdetails[i].discountprice,
                        discountper = orderRequest.orderdetails[i].discountper,
                        taxslabid = orderRequest.orderdetails[i].taxslabid,
                        totaltax = orderRequest.orderdetails[i].totaltax,
                        totalprice = orderRequest.orderdetails[i].totalprice,
                        deleted = false

                    };
                    //  memoryCache.Remove("orderlist");
                    appDbContex.Orderdetails.Add(orderdetails);
                    await appDbContex.SaveChangesAsync();
                    string productid = orderRequest.orderdetails[i].productid;
                    var product = appDbContex.Products.Where(a => a.Id == productid).SingleOrDefault();
                    if (product != null)
                    {
                        product.quantity = product.quantity - orderRequest.orderdetails[i].quantity;
                        // memoryCache.Remove("prodcutlist");
                        //  appDbContex.Update(product);
                        await appDbContex.SaveChangesAsync();
                    }
                }
                //  PushNotificationLogic pushNotificationLogic = new PushNotificationLogic();
                ////  string[] androidDeviceTocken;
                //  List<string> androidDeviceTocken = new List<string>();
                //  androidDeviceTocken.Add("d-IbdqErTbPYjkaM48fhLs:APA91bEpxaGTdvazkrJ3WN_apDaLRiHjejMY5ob6n9falIV-yXwFyM9Bn6p3y20bo5c09TaS7PU9E8zYKGprXFoYVo7gMgPAm60YiyrGfzXd8yPa3hfPc-k_v67abpNbhOlkp0AqKDzD");
                //  string notificationMessage = string.Empty;
                //  notificationMessage = "New Order Received";

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                hubContext.Clients.All.sendMessage("NewOrderReceived");

                // string estimatedate = SqlFunctions.DateName("day", orderRequest.estimatedeliverydate) + "/" + SqlFunctions.StringConvert((double)orderRequest.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", orderRequest.estimatedeliverydate);
                string estimatedate = orderRequest.estimatedeliverydate.ToString();
                estimatedate = estimatedate.Replace("12:00:00 AM", " ");

                // sendSMS.SendTextSms("Order Placed: Your orderNo :  " + "OD" + count.ToString() + " amounting to Rs." + orderRequest.totalamount + " has been received. your estimate delivery date is "+ estimatedate + " and delivery time is (7:00AM to 7:00PM), Green Shop.", "91" + orderRequest.phoneno);

                //send sms to owner 
                // sendSMS.SendTextSms("New Order Placed: we have received new order with orderNo :  " + "OD" + count.ToString() + " Amount Rs." + orderRequest.totalamount + " from " + orderRequest.name + " and Mo: " + orderRequest.phoneno + "", "919687651741");

                //  var myobj = new { Name = "Nilesh", City = "Chikhli" };
                var userfornotification = appDbContex.Users.Where(a => a.PhoneNumber == "9016254411").FirstOrDefault();
                if (userfornotification != null)
                {
                    PushNotificationLogic.sendpushnotificationtouser("New Order Placed: we have received new order with orderNo : " + "OD" + count.ToString() + ".", userfornotification.Id);
                }
                //   await pushNotificationLogic.SendPushNotification(androidDeviceTocken, "Balaji Fruits & Vegetables", notificationMessage, myobj);
                status.status = true;
                status.message = "order place successfully!";
                // status.objItem = guId.ToString();
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // Generate a random number between two numbers  
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        [HttpPost]
        [Route("orderbyorderid")]
        public async Task<ResponseStatus> getorderbyorderid(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = from o in appDbContex.Ordermasters.Where(a => a.Id == id && a.deleted == false)
                            select new
                            {
                                o.Id,
                                o.inCourierComId,
                                o.address,
                                o.addressbilling,
                                o.bankname,
                                o.callNumber,
                                cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),


                                o.city,
                                o.citybilling,
                                o.createAt,
                                o.customerid,
                                o.deleted,
                                o.deliveryDate,
                                o.discount,
                                o.emailid,
                                o.estimatedeliverydate,
                                o.estimatedeliverytime,
                                o.flgIsCallRequest,
                                o.flgIsCancel,
                                o.flgIsCancelRequest,
                                o.flgIsPaymentDone,
                                o.flgIsReturn,
                                o.invoiceno,
                                o.landmark,
                                o.landmarkbilling,
                                o.name,
                                o.namebilling,
                                o.orderAcceptDate,

                                //   o.orderdate,
                                orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),


                                o.orderDispatchedDate,
                                o.orderno,
                                o.orderPackedDate,
                                o.ordertype,
                                o.paymenttype,
                                o.phoneno,
                                o.phonenobilling,
                                o.pincode,
                                o.pincodebilling,
                                o.remark,

                                o.returnDate,
                                o.shipmentstatus,
                                o.shippingCharge,
                                o.shippingTrackingId,
                                o.status,
                                o.tax,
                                o.totalamount,
                                o.transectiondate,
                                o.transectionId,
                                o.transectionRemarks



                            };
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("orderbyuserid")]
        public async Task<ResponseStatus> getorderbyuserid(string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = from o in appDbContex.Ordermasters.OrderByDescending(a => a.orderdate).Where(a => a.customerid == userid && a.deleted == false && a.status != "Pending")
                            select new
                            {
                                o.Id,
                                o.inCourierComId,
                                o.address,
                                o.addressbilling,
                                o.bankname,
                                o.callNumber,
                                cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                o.city,
                                o.citybilling,
                                o.createAt,
                                o.customerid,
                                o.deleted,
                                deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                o.discount,
                                o.emailid,
                                o.estimatedeliverydate,
                                o.estimatedeliverytime,
                                o.flgIsCallRequest,
                                o.flgIsCancel,
                                o.flgIsCancelRequest,
                                o.flgIsPaymentDone,
                                o.flgIsReturn,
                                o.invoiceno,
                                o.landmark,
                                o.landmarkbilling,
                                o.name,
                                o.namebilling,
                                o.orderAcceptDate,

                                orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                o.orderDispatchedDate,
                                o.orderno,
                                o.orderPackedDate,
                                o.ordertype,
                                o.paymenttype,
                                o.phoneno,
                                o.phonenobilling,
                                o.pincode,
                                o.pincodebilling,
                                o.remark,

                                returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                o.shipmentstatus,
                                o.shippingCharge,
                                o.shippingTrackingId,
                                o.status,
                                o.tax,
                                o.totalamount,
                                o.transectiondate,
                                o.transectionId,
                                o.transectionRemarks



                            };
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("pendingorderbyuserid")]
        public async Task<ResponseStatus> getpendingorderbyuserid(string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = from o in appDbContex.Ordermasters.OrderByDescending(a => a.orderdate).Where(a => a.customerid == userid && a.deleted == false && a.status == "Pending")
                            select new
                            {
                                o.Id,
                                o.inCourierComId,
                                o.address,
                                o.addressbilling,
                                o.bankname,
                                o.callNumber,
                                cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                o.city,
                                o.citybilling,
                                o.createAt,
                                o.customerid,
                                o.deleted,
                                deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                o.discount,
                                o.emailid,
                                o.estimatedeliverydate,
                                o.estimatedeliverytime,
                                o.flgIsCallRequest,
                                o.flgIsCancel,
                                o.flgIsCancelRequest,
                                o.flgIsPaymentDone,
                                o.flgIsReturn,
                                o.invoiceno,
                                o.landmark,
                                o.landmarkbilling,
                                o.name,
                                o.namebilling,

                                orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),
                                orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                o.orderDispatchedDate,
                                o.orderno,
                                o.orderPackedDate,
                                o.ordertype,
                                o.paymenttype,
                                o.phoneno,
                                o.phonenobilling,
                                o.pincode,
                                o.pincodebilling,
                                o.remark,

                                returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                o.shipmentstatus,
                                o.shippingCharge,
                                o.shippingTrackingId,
                                o.status,
                                o.tax,
                                o.totalamount,
                                o.transectiondate,
                                o.transectionId,
                                o.transectionRemarks



                            };
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("returnorderbyuserid")]
        public async Task<ResponseStatus> getreturnorderbyuserid(string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = from o in appDbContex.Ordermasters.OrderByDescending(a => a.orderdate).Where(a => a.customerid == userid && a.deleted == false && a.status == "Return")
                            select new
                            {
                                o.Id,
                                o.inCourierComId,
                                o.address,
                                o.addressbilling,
                                o.bankname,
                                o.callNumber,
                                cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                o.city,
                                o.citybilling,
                                o.createAt,
                                o.customerid,
                                o.deleted,
                                deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                o.discount,
                                o.emailid,
                                o.estimatedeliverydate,
                                o.estimatedeliverytime,
                                o.flgIsCallRequest,
                                o.flgIsCancel,
                                o.flgIsCancelRequest,
                                o.flgIsPaymentDone,
                                o.flgIsReturn,
                                o.invoiceno,
                                o.landmark,
                                o.landmarkbilling,
                                o.name,
                                o.namebilling,

                                orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),
                                orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                o.orderDispatchedDate,
                                o.orderno,
                                o.orderPackedDate,
                                o.ordertype,
                                o.paymenttype,
                                o.phoneno,
                                o.phonenobilling,
                                o.pincode,
                                o.pincodebilling,
                                o.remark,

                                returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                o.shipmentstatus,
                                o.shippingCharge,
                                o.shippingTrackingId,
                                o.status,
                                o.tax,
                                o.totalamount,
                                o.transectiondate,
                                o.transectionId,
                                o.transectionRemarks



                            };
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("orderdetailsbyorderid")]
        public async Task<ResponseStatus> getorderdetailsbyorderid(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = from c in appDbContex.Orderdetails.Where(b => b.orderid == id)
                            select new
                            {
                                c.Id,
                                c.orderid,
                                c.price,
                                c.productid,
                                unit = c.unitname,
                                c.quantity,
                                c.totalprice,
                                c.totaltax,
                                c.unitprice,
                                c.discountper,
                                c.discountprice,
                                c.taxslabid,
                                c.deleted,
                                discountamt = (c.quantity * c.discountprice) + (c.discountper * (c.unitprice * c.quantity) / 100),
                                productname = appDbContex.Products.Where(a => a.Id == c.productid).FirstOrDefault().name,
                                taxslabId = appDbContex.Products.Where(a => a.Id == c.productid).FirstOrDefault().intaxslabid,
                                unitname = appDbContex.Units.Where(a => a.Id == appDbContex.Products.Where(b => b.Id == c.productid).FirstOrDefault().unitid).FirstOrDefault().name,
                                productimage = appDbContex.Products.Where(a => a.Id == c.productid).FirstOrDefault().image
                            };
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("allorderdetails")]
        public async Task<ResponseStatus> getallorderdetails()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var order = appDbContex.Orderdetails.OrderByDescending(a => a.orderid).ToList();
                status.status = true;
                status.lstItems = order;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("updatestatus")]
        public async Task<ResponseStatus> updateStatus(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var orderstatus = appDbContex.Ordermasters.Where(a => a.Id == id).FirstOrDefault();
                int? count = appDbContex.Ordermasters.ToList().Where(a => a.status != "Pending" && a.status != "Cancel").Count() + 1;
                if (orderstatus != null)
                {
                    if (orderstatus.status == "Pending")
                    {
                        orderstatus.status = "Confirm";
                        orderstatus.orderAcceptDate = DateTime.UtcNow;
                        orderstatus.invoiceno = count.ToString();

                        await appDbContex.SaveChangesAsync();
                        PushNotificationLogic.sendpushnotificationtouser("Your Order with Green Shop, OrderNo " + orderstatus.orderno + " has been Confirmed!", orderstatus.customerid);
                        //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                        //hubContext.Clients.All.sendMessage("Order Confirm.");
                        // sendSMS.SendTextSms("Order Confirmation - Your Order with Green Shop, OrderNo " + orderstatus.orderno + " has been Confirmed!, Green Shop.", "91" + orderstatus.phoneno);
                    }
                    else if (orderstatus.status == "Confirm")
                    {
                        orderstatus.status = "Delivered";
                        orderstatus.deliveryDate = DateTime.UtcNow;

                        await appDbContex.SaveChangesAsync();
                        PushNotificationLogic.sendpushnotificationtouser("Your OrderNo " + orderstatus.orderno + " was delivered.", orderstatus.customerid);

                        //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                        //hubContext.Clients.All.sendMessage("Order Delivered.");
                        //sendSMS.SendTextSms("Delivered: Your OrderNo " + orderstatus.orderno + " was delivered., Green Shop.", "91" + orderstatus.phoneno);
                    }
                    status.status = true;
                    status.objItem = orderstatus.Id;
                    status.message = "Order Status Updated Successfully!";
                    return status;
                }
                status.status = false;
                status.message = "Record not found!";
                return status;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("cancelorder")]
        public async Task<ResponseStatus> cancelorder(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var orderstatus = appDbContex.Ordermasters.Where(a => a.Id == id).FirstOrDefault();
                if (orderstatus != null)
                {

                    orderstatus.status = "Cancel";
                    orderstatus.cancleDate = DateTime.UtcNow;
                    orderstatus.flgIsCancel = true;

                    await appDbContex.SaveChangesAsync();
                    var order = appDbContex.Orderdetails.Where(a => a.orderid == id).ToList();
                    if (order != null)
                    {
                        foreach (var ls in order)
                        {
                            var product = appDbContex.Products.Where(a => a.Id == ls.productid).SingleOrDefault();
                            if (product != null)
                            {
                                product.quantity = product.quantity + ls.quantity;
                                // memoryCache.Remove("prodcutlist");
                                //  appDbContex.Update(product);
                                await appDbContex.SaveChangesAsync();
                            }
                        }

                    }
                    PushNotificationLogic.sendpushnotificationtouser("Order Cancelled- Based on your request; From your order with OrderNo " + orderstatus.orderno + " has been cancelled.", orderstatus.customerid);

                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                    hubContext.Clients.All.sendMessage("Order Cancelled.");
                    // sendSMS.SendTextSms("Order Cancelled- Based on your request; From your order with OrderNo " + orderstatus.orderno + " has been cancelled. Green Shop.", "91" + orderstatus.phoneno);
                    status.status = true;
                    status.objItem = orderstatus.Id;
                    status.message = "Order Cancel Successfully!";
                    return status;
                }
                status.status = false;
                status.message = "Record not found!";
                return status;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("returnorder")]
        public async Task<ResponseStatus> returnorder(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var orderstatus = appDbContex.Ordermasters.Where(a => a.Id == id).FirstOrDefault();
                if (orderstatus != null)
                {

                    orderstatus.status = "Return";
                    orderstatus.returnDate = DateTime.UtcNow;
                    orderstatus.flgIsReturn = true;

                    await appDbContex.SaveChangesAsync();
                    var order = appDbContex.Orderdetails.Where(a => a.orderid == id).ToList();
                    if (order != null)
                    {
                        foreach (var ls in order)
                        {
                            var product = appDbContex.Products.Where(a => a.Id == ls.productid).SingleOrDefault();
                            if (product != null)
                            {
                                product.quantity = product.quantity + ls.quantity;
                                // memoryCache.Remove("prodcutlist");
                                //  appDbContex.Update(product);
                                await appDbContex.SaveChangesAsync();
                            }
                        }

                    }
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                    hubContext.Clients.All.sendMessage("Order Returned.");
                    // sendSMS.SendTextSms("Order Cancelled- Based on your request; From your order with OrderNo " + orderstatus.orderno + " has been cancelled. Green Shop.", "91" + orderstatus.phoneno);
                    status.status = true;
                    status.objItem = orderstatus.Id;
                    status.message = "Order Return Successfully!";
                    return status;
                }
                status.status = false;
                status.message = "Record not found!";
                return status;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("getallorder")]
        public async Task<ResponseStatus> getallorder(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var cachekey = "orderlist";

                int count = appDbContex.Ordermasters.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                var orderlst = from o in appDbContex.Ordermasters.OrderByDescending(a => a.orderdate).Where(a => a.deleted == false).Skip(skip).Take(pageSize)
                               select new
                               {
                                   o.Id,
                                   o.inCourierComId,
                                   o.address,
                                   o.addressbilling,
                                   o.bankname,
                                   o.callNumber,
                                   cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                   o.city,
                                   o.citybilling,
                                   o.createAt,
                                   o.customerid,
                                   o.deleted,
                                   deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                   o.discount,
                                   o.emailid,
                                   estimatedeliverydate = SqlFunctions.DateName("day", o.estimatedeliverydate).Trim() + "/" + SqlFunctions.StringConvert((double)o.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.estimatedeliverydate),
                                   o.estimatedeliverytime,
                                   o.flgIsCallRequest,
                                   o.flgIsCancel,
                                   o.flgIsCancelRequest,
                                   o.flgIsPaymentDone,
                                   o.flgIsReturn,
                                   o.invoiceno,
                                   o.landmark,
                                   o.landmarkbilling,
                                   o.name,
                                   o.namebilling,
                                   orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),

                                   orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                   o.orderDispatchedDate,
                                   o.orderno,
                                   o.orderPackedDate,
                                   o.ordertype,
                                   o.paymenttype,
                                   o.phoneno,
                                   o.phonenobilling,
                                   o.pincode,
                                   o.pincodebilling,
                                   o.remark,

                                   returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                   o.shipmentstatus,
                                   o.shippingCharge,
                                   o.shippingTrackingId,
                                   o.status,
                                   o.tax,
                                   o.totalamount,
                                   o.transectiondate,
                                   o.transectionId,
                                   o.transectionRemarks



                               };
                status.lstItems = orderlst;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("getallPendingorder")]
        public async Task<ResponseStatus> getallpendingorder(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var cachekey = "orderlist";
                int count = appDbContex.Ordermasters.Where(a => a.status == "Pending" && a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                var orderlst = from o in appDbContex.Ordermasters.Where(a => a.status == "Pending" && a.deleted == false).OrderByDescending(a => a.orderdate).Skip(skip).Take(pageSize)
                               select new
                               {
                                   o.Id,
                                   o.inCourierComId,
                                   o.address,
                                   o.addressbilling,
                                   o.bankname,
                                   o.callNumber,
                                   cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                   o.city,
                                   o.citybilling,
                                   o.createAt,
                                   o.customerid,
                                   o.deleted,
                                   deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                   o.discount,
                                   o.emailid,
                                   estimatedeliverydate = SqlFunctions.DateName("day", o.estimatedeliverydate).Trim() + "/" + SqlFunctions.StringConvert((double)o.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.estimatedeliverydate),
                                   o.estimatedeliverytime,
                                   o.flgIsCallRequest,
                                   o.flgIsCancel,
                                   o.flgIsCancelRequest,
                                   o.flgIsPaymentDone,
                                   o.flgIsReturn,
                                   o.invoiceno,
                                   o.landmark,
                                   o.landmarkbilling,
                                   o.name,
                                   o.namebilling,
                                   orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),

                                   orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                   o.orderDispatchedDate,
                                   o.orderno,
                                   o.orderPackedDate,
                                   o.ordertype,
                                   o.paymenttype,
                                   o.phoneno,
                                   o.phonenobilling,
                                   o.pincode,
                                   o.pincodebilling,
                                   o.remark,

                                   returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                   o.shipmentstatus,
                                   o.shippingCharge,
                                   o.shippingTrackingId,
                                   o.status,
                                   o.tax,
                                   o.totalamount,
                                   o.transectiondate,
                                   o.transectionId,
                                   o.transectionRemarks



                               };
                status.lstItems = orderlst;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet]
        [Route("getallPendingorderforadmin")]
        public async Task<ResponseStatus> getallpendingorder()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                int count = appDbContex.Ordermasters.Where(a => a.status == "Pending" && a.deleted == false).ToList().Count();


                var orderlst = from o in appDbContex.Ordermasters.Where(a => a.status == "Pending" && a.deleted == false).OrderByDescending(a => a.orderdate)
                               select new
                               {
                                   o.Id,
                                   o.inCourierComId,
                                   o.address,
                                   o.addressbilling,
                                   o.bankname,
                                   o.callNumber,
                                   cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                   o.city,
                                   o.citybilling,
                                   o.createAt,
                                   o.customerid,
                                   o.deleted,
                                   deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                   o.discount,
                                   o.emailid,
                                   estimatedeliverydate = SqlFunctions.DateName("day", o.estimatedeliverydate).Trim() + "/" + SqlFunctions.StringConvert((double)o.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.estimatedeliverydate),
                                   o.estimatedeliverytime,
                                   o.flgIsCallRequest,
                                   o.flgIsCancel,
                                   o.flgIsCancelRequest,
                                   o.flgIsPaymentDone,
                                   o.flgIsReturn,
                                   o.invoiceno,
                                   o.landmark,
                                   o.landmarkbilling,
                                   o.name,
                                   o.namebilling,
                                   orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),
                                   orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                   o.orderDispatchedDate,
                                   o.orderno,
                                   o.orderPackedDate,
                                   o.ordertype,
                                   o.paymenttype,
                                   o.phoneno,
                                   o.phonenobilling,
                                   o.pincode,
                                   o.pincodebilling,
                                   o.remark,
                                   returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                   o.shipmentstatus,
                                   o.shippingCharge,
                                   o.shippingTrackingId,
                                   o.status,
                                   o.tax,
                                   o.totalamount,
                                   o.transectiondate,
                                   o.transectionId,
                                   o.transectionRemarks
                               };
                status.lstItems = orderlst;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet]
        [Route("getallorderforadmin")]
        public async Task<ResponseStatus> getallorderforadmin()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                int count = appDbContex.Ordermasters.ToList().Count();
                // int skip = (pageNo - 1) * pageSize;

                var orderlst = from o in appDbContex.Ordermasters.Where(a => a.status == "Confirm" && a.deleted == false).OrderByDescending(a => a.orderdate)
                               select new
                               {
                                   o.Id,
                                   o.inCourierComId,
                                   o.address,
                                   o.addressbilling,
                                   o.bankname,
                                   o.callNumber,
                                   cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                   o.city,
                                   o.citybilling,
                                   o.createAt,
                                   o.customerid,
                                   o.deleted,
                                   deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                   o.discount,
                                   o.emailid,
                                   estimatedeliverydate = SqlFunctions.DateName("day", o.estimatedeliverydate).Trim() + "/" + SqlFunctions.StringConvert((double)o.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.estimatedeliverydate),
                                   o.estimatedeliverytime,
                                   o.flgIsCallRequest,
                                   o.flgIsCancel,
                                   o.flgIsCancelRequest,
                                   o.flgIsPaymentDone,
                                   o.flgIsReturn,
                                   o.invoiceno,
                                   o.landmark,
                                   o.landmarkbilling,
                                   o.name,
                                   o.namebilling,
                                   orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),

                                   orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                   o.orderDispatchedDate,
                                   o.orderno,
                                   o.orderPackedDate,
                                   o.ordertype,
                                   o.paymenttype,
                                   o.phoneno,
                                   o.phonenobilling,
                                   o.pincode,
                                   o.pincodebilling,
                                   o.remark,

                                   returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                   o.shipmentstatus,
                                   o.shippingCharge,
                                   o.shippingTrackingId,
                                   o.status,
                                   o.tax,
                                   o.totalamount,
                                   o.transectiondate,
                                   o.transectionId,
                                   o.transectionRemarks



                               };
                status.lstItems = orderlst;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("getallorderlistforadmin")]
        public async Task<ResponseStatus> getallorderlistforadmin()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                int count = appDbContex.Ordermasters.ToList().Count();
                // int skip = (pageNo - 1) * pageSize;

                var orderlst = from o in appDbContex.Ordermasters.Where(a => a.deleted == false).OrderByDescending(a => a.orderdate)
                               select new
                               {
                                   o.Id,
                                   o.inCourierComId,
                                   o.address,
                                   o.addressbilling,
                                   o.bankname,
                                   o.callNumber,
                                   cancleDate = SqlFunctions.DateName("day", o.cancleDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.cancleDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.cancleDate),
                                   o.city,
                                   o.citybilling,
                                   o.createAt,
                                   o.customerid,
                                   o.deleted,
                                   deliveryDate = SqlFunctions.DateName("day", o.deliveryDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.deliveryDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.deliveryDate),
                                   o.discount,
                                   o.emailid,
                                   estimatedeliverydate = SqlFunctions.DateName("day", o.estimatedeliverydate).Trim() + "/" + SqlFunctions.StringConvert((double)o.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.estimatedeliverydate),
                                   o.estimatedeliverytime,
                                   o.flgIsCallRequest,
                                   o.flgIsCancel,
                                   o.flgIsCancelRequest,
                                   o.flgIsPaymentDone,
                                   o.flgIsReturn,
                                   o.invoiceno,
                                   o.landmark,
                                   o.landmarkbilling,
                                   o.name,
                                   o.namebilling,
                                   orderAcceptDate = SqlFunctions.DateName("day", o.orderAcceptDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderAcceptDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderAcceptDate),

                                   orderdate = SqlFunctions.DateName("day", o.orderdate).Trim() + "/" + SqlFunctions.StringConvert((double)o.orderdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.orderdate),
                                   o.orderDispatchedDate,
                                   o.orderno,
                                   o.orderPackedDate,
                                   o.ordertype,
                                   o.paymenttype,
                                   o.phoneno,
                                   o.phonenobilling,
                                   o.pincode,
                                   o.pincodebilling,
                                   o.remark,

                                   returnDate = SqlFunctions.DateName("day", o.returnDate).Trim() + "/" + SqlFunctions.StringConvert((double)o.returnDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", o.returnDate),
                                   o.shipmentstatus,
                                   o.shippingCharge,
                                   o.shippingTrackingId,
                                   o.status,
                                   o.tax,
                                   o.totalamount,
                                   o.transectiondate,
                                   o.transectionId,
                                   o.transectionRemarks



                               };
                status.lstItems = orderlst;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("monthwisesellreport")]
        public async Task<ResponseStatus> monthwisesellreport()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var data = appDbContex.Ordermasters.Where(a => a.status == "Delivered" && a.deleted == false).Select(k => new { k.orderdate.Year, k.orderdate.Month, k.totalamount }).GroupBy(x => new { x.Year, x.Month }, (key, group) => new
                {
                    year = key.Year,
                    month = key.Month,
                    amount = group.Sum(k => k.totalamount)
                }).ToList();

                status.lstItems = data;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //add new order api for website
        [HttpPost]
        [Route("addneworderfromweb")]
        public async Task<ResponseStatus> addneworderfromweb(orderRequest orderRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var guId = Guid.NewGuid();
                int? count = appDbContex.Ordermasters.ToList().Count() + 1;
                ordermaster ordermaster = new ordermaster
                {
                    Id = guId.ToString(),
                    // invoiceno = count.ToString(),
                    orderno = "OD" + count.ToString(),
                    customerid = orderRequest.customerid,
                    emailid = orderRequest.emailid,
                    name = orderRequest.name,
                    address = orderRequest.address,
                    city = orderRequest.city,
                    landmark = orderRequest.landmark,
                    pincode = orderRequest.pincode,
                    phoneno = orderRequest.phoneno,
                    namebilling = orderRequest.namebilling,
                    addressbilling = orderRequest.addressbilling,
                    citybilling = orderRequest.citybilling,
                    landmarkbilling = orderRequest.landmarkbilling,
                    pincodebilling = orderRequest.pincodebilling,
                    phonenobilling = orderRequest.phonenobilling,
                    paymenttype = orderRequest.paymenttype,
                    flgIsPaymentDone = orderRequest.flgIsPaymentDone,
                    transectionId = orderRequest.transectionId,
                    transectionRemarks = orderRequest.transectionRemarks,
                    bankname = orderRequest.bankname,
                    // transectiondate = orderRequest.tra,
                    shipmentstatus = orderRequest.shipmentstatus,
                    shippingCharge = orderRequest.shippingCharge,
                    orderdate = DateTime.UtcNow,
                    ordertype = "Cash On Delivery",
                    discount = orderRequest.discount,
                    status = orderRequest.status,
                    tax = orderRequest.tax,
                    totalamount = orderRequest.totalamount,
                    estimatedeliverydate = orderRequest.estimatedeliverydate,
                    estimatedeliverytime = orderRequest.estimatedeliverytime,
                    deleted = false,
                    createAt = DateTime.UtcNow



                };
                appDbContex.Ordermasters.Add(ordermaster);
                await appDbContex.SaveChangesAsync();

                // var people = System.Text.Json.JsonSerializer.Deserialize<List<orderdetails>>(json);


                for (int i = 0; i < orderRequest.orderdetails.Count; i++)
                {
                    var gId = Guid.NewGuid();
                    orderdetails orderdetails = new orderdetails
                    {
                        Id = gId.ToString(),
                        orderid = guId.ToString(),
                        productid = orderRequest.orderdetails[i].productid,
                        unitname = orderRequest.orderdetails[i].unitname,
                        quantity = orderRequest.orderdetails[i].quantity,
                        unitprice = orderRequest.orderdetails[i].unitprice,
                        price = orderRequest.orderdetails[i].price,
                        discountprice = orderRequest.orderdetails[i].discountprice,
                        discountper = orderRequest.orderdetails[i].discountper,
                        taxslabid = orderRequest.orderdetails[i].taxslabid,
                        totaltax = orderRequest.orderdetails[i].totaltax,
                        totalprice = orderRequest.orderdetails[i].totalprice,
                        deleted = false

                    };
                    //  memoryCache.Remove("orderlist");
                    appDbContex.Orderdetails.Add(orderdetails);
                    await appDbContex.SaveChangesAsync();
                    string productid = orderRequest.orderdetails[i].productid;
                    var product = appDbContex.Products.Where(a => a.Id == productid).SingleOrDefault();
                    if (product != null)
                    {
                        product.quantity = product.quantity - orderRequest.orderdetails[i].quantity;
                        // memoryCache.Remove("prodcutlist");
                        //  appDbContex.Update(product);
                        await appDbContex.SaveChangesAsync();
                    }
                }
                //  PushNotificationLogic pushNotificationLogic = new PushNotificationLogic();
                ////  string[] androidDeviceTocken;
                //  List<string> androidDeviceTocken = new List<string>();
                //  androidDeviceTocken.Add("d-IbdqErTbPYjkaM48fhLs:APA91bEpxaGTdvazkrJ3WN_apDaLRiHjejMY5ob6n9falIV-yXwFyM9Bn6p3y20bo5c09TaS7PU9E8zYKGprXFoYVo7gMgPAm60YiyrGfzXd8yPa3hfPc-k_v67abpNbhOlkp0AqKDzD");
                //  string notificationMessage = string.Empty;
                //  notificationMessage = "New Order Received";

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                hubContext.Clients.All.sendMessage("NewOrderReceived");

                // string estimatedate = SqlFunctions.DateName("day", orderRequest.estimatedeliverydate) + "/" + SqlFunctions.StringConvert((double)orderRequest.estimatedeliverydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", orderRequest.estimatedeliverydate);
                string estimatedate = orderRequest.estimatedeliverydate.ToString();
                estimatedate = estimatedate.Replace("12:00:00 AM", " ");

                // sendSMS.SendTextSms("Order Placed: Your orderNo :  " + "OD" + count.ToString() + " amounting to Rs." + orderRequest.totalamount + " has been received. your estimate delivery date is "+ estimatedate + " and delivery time is (7:00AM to 7:00PM), Green Shop.", "91" + orderRequest.phoneno);

                //send sms to owner 
                // sendSMS.SendTextSms("New Order Placed: we have received new order with orderNo :  " + "OD" + count.ToString() + " Amount Rs." + orderRequest.totalamount + " from " + orderRequest.name + " and Mo: " + orderRequest.phoneno + "", "919687651741");

                //  var myobj = new { Name = "Nilesh", City = "Chikhli" };


                //   await pushNotificationLogic.SendPushNotification(androidDeviceTocken, "Balaji Fruits & Vegetables", notificationMessage, myobj);
                status.status = true;
                status.message = "order place successfully!";
                status.objItem = guId.ToString();
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}