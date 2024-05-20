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
    [RoutePrefix("api/Unit")]
    public class UnitController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public UnitController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addunit")]
        public async Task<ResponseStatus> addUnit(UnitRequest unitRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (unitRequest.Id == "0")
                {
                    var unitname = appDbContex.Units.Where(a => a.name == unitRequest.name && a.deleted == false).FirstOrDefault();
                    if (unitname == null)
                    {
                        var guId = Guid.NewGuid();
                        Unit unit = new Unit
                        {
                            Id = guId.ToString(),
                            name = unitRequest.name,
                            code = unitRequest.code,
                            deleted = false,
                            createAt = DateTime.Now
                        };
                        // memoryCache.Remove("unitlist");
                        appDbContex.Units.Add(unit);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Unit Save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Unit Already Added!";
                    }
                }
                else
                {
                    var name = appDbContex.Units.Where(a => a.name == unitRequest.name && a.deleted == false && a.Id != unitRequest.Id).SingleOrDefault();
                    if (name == null)
                    {
                        var unit = appDbContex.Units.Where(a => a.Id == unitRequest.Id).SingleOrDefault();
                        if (unit != null)
                        {
                            unit.name = unitRequest.name;
                            unit.updateAt = DateTime.Now;
                            // memoryCache.Remove("unitlist");
                            // appDbContex.Update(unit);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "Unit Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "Unit Already Exists!";

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
        [Route("allunitlist")]
        public async Task<ResponseStatus> getAllUnit(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var cachekey = "unitlist";
                //bool isExist = memoryCache.TryGetValue(cachekey, out IList<Unit> unitlist);
                //if (!isExist)
                //{
                //    unitlist = await appDbContex.Units.OrderByDescending(a => a.createAt).ToListAsync();
                //    var cacheEntryOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddHours(1),
                //        Priority = CacheItemPriority.Normal,
                //        SlidingExpiration = TimeSpan.FromMinutes(5)
                //    };

                //    memoryCache.Set(cachekey, unitlist, cacheEntryOptions);
                //}
                int count = appDbContex.Units.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var unitlist = appDbContex.Units.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = unitlist;
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
