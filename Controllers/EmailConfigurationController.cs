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
    [RoutePrefix("api/EmailConfiguration")]
    public class EmailConfigurationController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public EmailConfigurationController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addemailconfiguration")]
        public async Task<ResponseStatus> addemailconfiguration(emailconfigurationRequest emailconfigurationRequest)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                if (emailconfigurationRequest.id == "0")
                {
                    var email = appDbContex.Emailconfigurations.Where(a => a.code == emailconfigurationRequest.code && a.deleted == false).FirstOrDefault();
                    if (email == null)
                    {
                        var guId = Guid.NewGuid();
                        Emailconfigurations emailconfiguration = new Emailconfigurations
                        {
                            id = guId.ToString(),
                            name = emailconfigurationRequest.name,
                            code = emailconfigurationRequest.code,
                            subject = emailconfigurationRequest.subject,
                            body = emailconfigurationRequest.body,
                            deleted = false,
                            createAt = DateTime.Now
                        };
                        // memoryCache.Remove("emaillist");
                        appDbContex.Emailconfigurations.Add(emailconfiguration);
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
                    var emailconfig = appDbContex.Emailconfigurations.Where(a => a.id != emailconfigurationRequest.id && a.code == emailconfigurationRequest.code && a.deleted == false).SingleOrDefault();
                    if (emailconfig == null)
                    {
                        var emailconfiguration = appDbContex.Emailconfigurations.Where(a => a.id == emailconfigurationRequest.id).FirstOrDefault();
                        if (emailconfiguration != null)
                        {
                            emailconfiguration.name = emailconfigurationRequest.name;
                            // emailconfiguration.code = emailconfigurationRequest.code;
                            emailconfiguration.subject = emailconfigurationRequest.subject;
                            emailconfiguration.body = emailconfigurationRequest.body;
                            emailconfiguration.updateAt = DateTime.Now;
                        }
                        //  memoryCache.Remove("emaillist");
                        //  appDbContex.Update(emailconfiguration);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Email configuration Updated Successfully!";
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
        [Route("gelallemailconfig")]
        public async Task<ResponseStatus> getallemailconfigeList(int pageNo, int pageSize)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var cachekey = "emaillist";
                //bool isExist = memoryCache.TryGetValue(cachekey, out IList<Emailconfigurations> emailconfigList);
                //if (!isExist)
                //{
                //    emailconfigList = await appDbContex.Emailconfigurations.OrderByDescending(a => a.createAt).ToListAsync();
                //    var cacheEntryOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddHours(1),
                //        Priority = CacheItemPriority.Normal,
                //        SlidingExpiration = TimeSpan.FromMinutes(5)
                //    };

                //    memoryCache.Set(cachekey, emailconfigList, cacheEntryOptions);
                //}
                int count = appDbContex.Emailconfigurations.ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var emailconfigList = appDbContex.Emailconfigurations.OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = emailconfigList;
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
