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
    [RoutePrefix("api/Taxslab")]
    public class TaxslabController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public TaxslabController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addtaxslabdetail")]
        public async Task<ResponseStatus> addTaxSlabDetails(TaxSlabRequest taxSlabRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (taxSlabRequest.Id == "0")
                {

                    var SlabName = appDbContex.TaxSlabMasters.Where(a => a.stSlabName == taxSlabRequest.stSlabName && a.deleted == false).FirstOrDefault();
                    if (SlabName == null)
                    {
                        var guId = Guid.NewGuid();
                        TaxSlabMaster taxSlabMaster = new TaxSlabMaster
                        {
                            Id = guId.ToString(),
                            stSlabName = taxSlabRequest.stSlabName,
                            CGSTHome = taxSlabRequest.CGSTHome,
                            SGSTHome = taxSlabRequest.SGSTHome,
                            IGSTHome = taxSlabRequest.IGSTHome,
                            CGST = taxSlabRequest.CGST,
                            SGST = taxSlabRequest.SGST,
                            IGST = taxSlabRequest.IGST,
                            deleted = false,
                            createAt = DateTime.UtcNow
                        };
                        //   memoryCache.Remove("taxslablist");
                        appDbContex.TaxSlabMasters.Add(taxSlabMaster);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Tax Slab Already Added!";
                    }
                }
                else
                {
                    var slabname = appDbContex.TaxSlabMasters.Where(a => a.stSlabName == taxSlabRequest.stSlabName && a.deleted == false && a.Id != taxSlabRequest.Id).SingleOrDefault();
                    if (slabname == null)
                    {
                        var slab = appDbContex.TaxSlabMasters.Where(a => a.Id == taxSlabRequest.Id).SingleOrDefault();
                        if (slab != null)
                        {

                            slab.stSlabName = taxSlabRequest.stSlabName;

                            slab.CGSTHome = taxSlabRequest.CGSTHome;
                            slab.SGSTHome = taxSlabRequest.SGSTHome;
                            slab.IGSTHome = taxSlabRequest.IGSTHome;
                            slab.CGST = taxSlabRequest.CGST;
                            slab.SGST = taxSlabRequest.SGST;
                            slab.IGST = taxSlabRequest.IGST;
                            slab.updateAt = DateTime.UtcNow;
                            //memoryCache.Remove("taxslablist");
                            //appDbContex.Update(slab);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "TaxSlab Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "TaxSlab Already Exists!";

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
        [Route("alltaxsablist")]
        public async Task<ResponseStatus> getAllTaxSlab(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var cachekey = "taxslablist";
                //bool isExist = memoryCache.TryGetValue(cachekey, out IList<TaxSlabMaster> taxslablst);
                //if (!isExist)
                //{
                //    taxslablst = await appDbContex.TaxSlabMasters.OrderByDescending(a => a.createAt).ToListAsync();
                //    var cacheEntryOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddHours(1),
                //        Priority = CacheItemPriority.Normal,
                //        SlidingExpiration = TimeSpan.FromMinutes(5)
                //    };

                //    memoryCache.Set(cachekey, taxslablst, cacheEntryOptions);
                //}
                int count = appDbContex.TaxSlabMasters.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var taxslablst = appDbContex.TaxSlabMasters.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = taxslablst;
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
