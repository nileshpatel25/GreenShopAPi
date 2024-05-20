using apiGreenShop.DataModel;
using apiGreenShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/SmsConfiguration")]
    public class SmsConfigurationController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public SmsConfigurationController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addsmsconfiguration")]
        public async Task<ResponseStatus> addsmsconfiguration(smsconfigRequest smsconfigRequest)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                if (smsconfigRequest.id == "0")
                {
                    var sms = appDbContex.Smsconfigurations.Where(a => a.code == smsconfigRequest.code && a.deleted == false).FirstOrDefault();
                    if (sms == null)
                    {
                        var guId = Guid.NewGuid();
                        smsconfiguration smsconfigurations = new smsconfiguration
                        {
                            id = guId.ToString(),
                            name = smsconfigRequest.name,
                            code = smsconfigRequest.code,
                            subject = smsconfigRequest.subject,
                            body = smsconfigRequest.body,
                            deleted = false,
                            createAt = DateTime.Now
                        };
                        //  memoryCache.Remove("smslist");
                        appDbContex.Smsconfigurations.Add(smsconfigurations);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Save successfully!";
                        return status;
                    }
                    status.status = false;
                    status.message = "code already exists!";
                    return status;
                }
                else
                {
                    var smsconfig = appDbContex.Smsconfigurations.Where(a => a.id != smsconfigRequest.id && a.code == smsconfigRequest.code && a.deleted == false).SingleOrDefault();
                    if (smsconfig == null)
                    {
                        var smsconfiguration = appDbContex.Smsconfigurations.Where(a => a.id == smsconfigRequest.id).FirstOrDefault();
                        if (smsconfiguration != null)
                        {
                            smsconfiguration.name = smsconfigRequest.name;
                            //smsconfiguration.code = smsconfigRequest.code;
                            smsconfiguration.subject = smsconfigRequest.subject;
                            smsconfiguration.body = smsconfigRequest.body;
                            smsconfiguration.updateAt = DateTime.Now;
                        }
                        // memoryCache.Remove("smslist");
                        //appDbContex.Update(smsconfiguration);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "SMS configuration Updated Successfully!";
                        return status;
                    }
                    status.status = false;
                    status.message = "code already exists!";
                    return status;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("gelallsmsconfig")]
        public async Task<ResponseStatus> getallsmsconfigeList(int pageNo, int pageSize)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
              
               
                int count = appDbContex.Smsconfigurations.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var smsconfigList = appDbContex.Smsconfigurations.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = smsconfigList;
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