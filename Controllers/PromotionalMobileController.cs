using apiGreenShop.DataModel;
using apiGreenShop.Helper;
using apiGreenShop.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/PromotionalMobile")]
    public class PromotionalMobileController : ApiController
    {
        SendSMS sendSMS = new SendSMS();
        public ApplicationDbContext appDbContex { get; }
        public PromotionalMobileController()
        {
            appDbContex = new ApplicationDbContext();
        }
        [Route("UploadExcel")]
        [HttpPost]
        public async Task<ResponseStatus> Upload()
        {
            ResponseStatus status = new ResponseStatus();
            string message = "";
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;


            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                string vendorId = Convert.ToString(httpRequest.Form["vendorid"]);
                Stream stream = file.InputStream;

                IExcelDataReader reader = null;

                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    message = "This file format is not supported";
                }

                DataSet excelRecords = reader.AsDataSet();
                reader.Close();

                var finalRecords = excelRecords.Tables[0];
                for (int i = 0; i < finalRecords.Rows.Count; i++)
                {
                    var mobilenos = finalRecords.Rows[i][0].ToString();

                    var mobileno = appDbContex.promotionalMobileNos.Where(a => a.mobileno == mobilenos && a.vendorId == vendorId && a.deleted == false).FirstOrDefault();
                    if (mobileno == null)
                    {
                        var guId = Guid.NewGuid();
                        PromotionalMobileNo promotionalMobileNo = new PromotionalMobileNo
                        {
                            id = guId.ToString(),
                            vendorId = vendorId,
                            mobileno = finalRecords.Rows[i][0].ToString(),
                            deleted = false
                        };


                        appDbContex.promotionalMobileNos.Add(promotionalMobileNo);
                        await appDbContex.SaveChangesAsync();

                    }
                }
                status.status = true;
                status.message = "mobile no upload successfully";
                return status;

              

            }

            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
                status.status = true;
                status.message = result.ToString();
                return status;

            }


        }
        [HttpPost]
        [Route("getPromotionamobileno")]
        public async Task<ResponseStatus> getallpromotionano(string vendorid, int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var cachekey = "orderlist";
               
                int count = appDbContex.promotionalMobileNos.ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var moblist = appDbContex.promotionalMobileNos.Where(a => a.vendorId == vendorid).OrderByDescending(a => a.vendorId).Skip(skip).Take(pageSize).ToList();
                status.lstItems = moblist;
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
        [Route("sendsms")]
        public async Task<ResponseStatus> sendsms(sendSMSRequest sendSMSRequest)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var mobileno = appDbContex.promotionalMobileNos.Where(a => a.vendorId == sendSMSRequest.vendorId).ToList();
                foreach (var PhoneNo in mobileno)
                {

                    sendSMS.SendTextSms(sendSMSRequest.message, "+91" + PhoneNo.mobileno);
                    var id = Guid.NewGuid();
                    SMSHistory sMSHistory = new SMSHistory
                    {
                        id = id.ToString(),
                        vendorId = sendSMSRequest.vendorId,
                        message = sendSMSRequest.message,
                        mobileno = PhoneNo.mobileno,
                        createddt = DateTime.Now
                    };
                    appDbContex.SMSHistories.Add(sMSHistory);
                    await appDbContex.SaveChangesAsync();
                }
                responseStatus.status = true;
                responseStatus.message = "Message sent successfully.";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("sendsmstouser")]
        public async Task<ResponseStatus> sendsmstouser(string message,string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var user = appDbContex.Users.ToList();
                foreach(var ls in user)
                {
                    sendSMS.SendTextSms(message, "+91" + ls.PhoneNumber);
                    var id = Guid.NewGuid();
                    SMSHistory sMSHistory = new SMSHistory
                    {
                        id = id.ToString(),
                        vendorId = userid,
                        message = message,
                        mobileno = ls.PhoneNumber,
                        createddt = DateTime.Now
                    };
                    appDbContex.SMSHistories.Add(sMSHistory);
                    await appDbContex.SaveChangesAsync();
                }
                status.status = true;
                status.message = "Message sent successfully.";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("smshistorybyvendorid")]
        public async Task<ResponseStatus> getsmshistorybyvendorid(string vendorid, int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = appDbContex.SMSHistories.Where(a => a.vendorId == vendorid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var moblist = appDbContex.SMSHistories.Where(a => a.vendorId == vendorid).OrderByDescending(a => a.createddt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = moblist;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}