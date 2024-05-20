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
    [RoutePrefix("api/unitquantityfactor")]
    public class UnitQuantityfactorController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public UnitQuantityfactorController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addunitquantityfactor")]
        public async Task<ResponseStatus> addunitquanityfactorname(unitQuantityfactorRequest unitQuantityfactorRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (unitQuantityfactorRequest.id == "0")
                {
                    var name = appDbContex.UnitQuantityFactors.Where(a => a.unitname == unitQuantityfactorRequest.unitname && a.unitfactornameid==unitQuantityfactorRequest.unitfactornameid && a.deleted == false).FirstOrDefault();
                    if (name == null)
                    {
                        var guId = Guid.NewGuid();
                        UnitQuantityFactor unitQuantityFactor = new UnitQuantityFactor
                        {
                            id = guId.ToString(),
                            unitfactornameid = unitQuantityfactorRequest.unitfactornameid,
                            unitname=unitQuantityfactorRequest.unitname,
                            quantityfactor=unitQuantityfactorRequest.quantityfactor,
                            pricefactor=unitQuantityfactorRequest.pricefactor,
                            createdAt = DateTime.Now,
                            deleted = false

                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.UnitQuantityFactors.Add(unitQuantityFactor);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Already Added!";
                    }
                }
                else
                {
                    var name = appDbContex.UnitQuantityFactors.Where(a => a.unitname == unitQuantityfactorRequest.unitname && a.unitfactornameid == unitQuantityfactorRequest.unitfactornameid && a.deleted == false && a.id != unitQuantityfactorRequest.id).SingleOrDefault();
                    if (name == null)
                    {
                        var unitfactorname = appDbContex.UnitQuantityFactors.Where(a => a.id == unitQuantityfactorRequest.id).SingleOrDefault();
                        if (unitfactorname != null)
                        {
                            unitfactorname.unitfactornameid = unitQuantityfactorRequest.unitfactornameid;
                            unitfactorname.unitname = unitQuantityfactorRequest.unitname;
                            unitfactorname.quantityfactor = unitQuantityfactorRequest.quantityfactor;
                            unitfactorname.pricefactor = unitQuantityfactorRequest.pricefactor;
                            // city.updateAt = DateTime.Now;
                            //  memoryCache.Remove("citylist");
                            // appDbContex.Update(city);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "Already Exists!";

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
        [Route("allunitquantityfactorbyId")]
        public async Task<ResponseStatus> getallunitfactorname(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                status.lstItems = from c in appDbContex.UnitQuantityFactors.Where(a => a.unitfactornameid == id && a.deleted == false).OrderByDescending(a => a.createdAt)
                                  select new
                                  {
                                      c.id,
                                      c.unitfactornameid,
                                      name= appDbContex.UnitFactorNames.Where(u => u.id == c.unitfactornameid).FirstOrDefault().unitfactorname,
                                      c.unitname,
                                      c.quantityfactor,
                                      c.pricefactor,
                                      c.deleted
                                  };


                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("allunitquantityfactorlist")]
        public async Task<ResponseStatus> getallunitfactornamelist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.UnitQuantityFactors.Where(a => a.deleted == false).OrderByDescending(a => a.createdAt).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deleteunitquantityfactor")]
        public async Task<ResponseStatus> deleteunitfactorname(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var name = appDbContex.UnitQuantityFactors.Where(a => a.id == id).SingleOrDefault();
                if (name != null)
                {
                    name.deleted = true;

                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}
