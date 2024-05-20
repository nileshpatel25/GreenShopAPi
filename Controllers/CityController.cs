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
    [RoutePrefix("api/City")]
    public class CityController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public CityController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("AddCity")]
        public async Task<ResponseStatus> addCity(CityRequest cityRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (cityRequest.Id == "0")
                {
                    var cityname = appDbContex.Cities.Where(a => a.name == cityRequest.name && a.deleted == false).FirstOrDefault();
                    if (cityname == null)
                    {
                        var guId = Guid.NewGuid();
                        City city = new City
                        {
                            Id = guId.ToString(),
                            name = cityRequest.name,
                            code = cityRequest.code,
                            deleted = false,
                            createAt = DateTime.UtcNow
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.Cities.Add(city);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "City Already Added!";
                    }
                }
                else
                {
                    var name = appDbContex.Cities.Where(a => a.name == cityRequest.name && a.deleted == false && a.Id != cityRequest.Id).SingleOrDefault();
                    if (name == null)
                    {
                        var city = appDbContex.Cities.Where(a => a.Id == cityRequest.Id).SingleOrDefault();
                        if (city != null)
                        {
                            city.name = cityRequest.name;
                            city.code = cityRequest.code;
                            // city.updateAt = DateTime.Now;
                            //  memoryCache.Remove("citylist");
                            // appDbContex.Update(city);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "City Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "City Already Exists!";

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
        [Route("AllCity")]
        public async Task<ResponseStatus> getAllCity(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                //var cachekey = "citylist";
                //bool isExist = memoryCache.TryGetValue(cachekey, out IList<City> citylst);
                //if (!isExist)
                //{
                //    citylst = await appDbContex.Cities.OrderByDescending(a => a.createAt).ToListAsync();
                //    var cacheEntryOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddHours(1),
                //        Priority = CacheItemPriority.Normal,
                //        SlidingExpiration = TimeSpan.FromMinutes(5)
                //    };

                //    memoryCache.Set(cachekey, citylst, cacheEntryOptions);
                //}
                int count = appDbContex.Cities.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var citylst = appDbContex.Cities.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = citylst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("AllCityList")]
        public async Task<ResponseStatus> getAllCityList()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.Cities.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deletecity")]
        public async Task<ResponseStatus> deletecites(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var city = appDbContex.Cities.Where(a => a.Id == id).SingleOrDefault();
                if (city != null)
                {
                    city.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "city deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "city not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


    }
}
