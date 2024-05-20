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
    [RoutePrefix("api/review")]
    public class ReviewController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public ReviewController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addreview")]
        public async Task<ResponseStatus> addreview(review reviewRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (reviewRequest.id == "0")
                {
                   
                        var guId = Guid.NewGuid();
                        review review = new review
                        {
                            id = guId.ToString(),
                            productid=reviewRequest.productid,
                            starcount=reviewRequest.starcount,
                            name = reviewRequest.name,
                            reviewdetails = reviewRequest.reviewdetails,
                            deleted = false,
                            createAt = DateTime.UtcNow
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.Reviews.Add(review);
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

        [HttpPost]
        [Route("reviewbyproductid")]
        public async Task<ResponseStatus> getallreviewbyproductid(string productid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();            
              
               
                status.lstItems = appDbContex.Reviews.Where(a => a.deleted == false && a.productid==productid).OrderByDescending(a => a.createAt).ToList(); ;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deletereview")]
        public async Task<ResponseStatus> deletereview(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var review = appDbContex.Reviews.Where(a => a.id == id).SingleOrDefault();
                if (review != null)
                {
                    review.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "review deleted Successfully!";
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
