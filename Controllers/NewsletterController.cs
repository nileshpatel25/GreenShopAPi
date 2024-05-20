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
    [RoutePrefix("api/newsletter")]
    public class NewsletterController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public NewsletterController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addnewsletter")]
        public async Task<ResponseStatus> addreview(newslatter newslatterrequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (newslatterrequest.id == "0")
                {

                    var guId = Guid.NewGuid();
                    newslatter newslatter = new newslatter
                    {
                        id = guId.ToString(),
                        emailid= newslatterrequest.emailid,
                        active=false,
                        deleted = false,
                        createAt = DateTime.UtcNow
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.Newslatters.Add(newslatter);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Save successfully!";
                    // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                    return status;

                }


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.Message;
                throw ex;
            }
            return status;
        }

        [HttpGet]
        [Route("allnewslatter")]
        public async Task<ResponseStatus> getallreviewbyproductid()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.Newslatters.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).ToList(); ;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("unsubcribenewsletter")]
        public async Task<ResponseStatus> unsbcribenewsletter(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var review = appDbContex.Newslatters.Where(a => a.id == id).SingleOrDefault();
                if (review != null)
                {
                    review.active = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "unsubscribe Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "record not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
