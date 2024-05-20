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
    [RoutePrefix("api/unitfactorname")]
    public class UnitFactorNameController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public UnitFactorNameController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addunitfactorname")]
        public async Task<ResponseStatus> addunitfactorname(unitFactornameRequest unitFactornameRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (unitFactornameRequest.id == "0")
                {
                    var name = appDbContex.UnitFactorNames.Where(a => a.unitfactorname == unitFactornameRequest.unitfactorname && a.deleted == false).FirstOrDefault();
                    if (name == null)
                    {
                        var guId = Guid.NewGuid();
                        UnitFactorName unitFactorName = new UnitFactorName
                        {
                            id = guId.ToString(),
                            unitfactorname = unitFactornameRequest.unitfactorname,
                            createdAt=DateTime.UtcNow,
                            deleted = false
                            
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.UnitFactorNames.Add(unitFactorName);
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
                    var name = appDbContex.UnitFactorNames.Where(a => a.unitfactorname == unitFactornameRequest.unitfactorname && a.deleted == false && a.id != unitFactornameRequest.id).SingleOrDefault();
                    if (name == null)
                    {
                        var unitfactorname = appDbContex.UnitFactorNames.Where(a => a.id == unitFactornameRequest.id).SingleOrDefault();
                        if (unitfactorname != null)
                        {
                            unitfactorname.unitfactorname = unitFactornameRequest.unitfactorname;
                            
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
        [Route("allunitfactorname")]
        public async Task<ResponseStatus> getallunitfactorname(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();             
                int skip = (pageNo - 1) * pageSize;               
                status.lstItems = appDbContex.UnitFactorNames.Where(a => a.deleted == false).OrderByDescending(a => a.createdAt).Skip(skip).Take(pageSize).ToList(); 
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("allunitfactornamelist")]
        public async Task<ResponseStatus> getallunitfactornamelist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.UnitFactorNames.Where(a => a.deleted == false).OrderByDescending(a => a.createdAt).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deleteunitfactorname")]
        public async Task<ResponseStatus> deleteunitfactorname(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var name = appDbContex.UnitFactorNames.Where(a => a.id == id).SingleOrDefault();
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


        [HttpGet]
        [Route("allunitquantityfactorwithnamelist")]
        public async Task<ResponseStatus> getallunitfactornamewithlist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.UnitFactorNames.Where(a => a.deleted == false).Select(c=>new
                {
                    c.id,
                    c.unitfactorname,
                    c.deleted,
                    c.createdAt,
                    subunit=appDbContex.UnitQuantityFactors.Where(q=>q.unitfactornameid==c.id).Select(q => new
                    {
                        id = q.unitfactornameid,
                        unitfactorname = q.unitname +"_" +  q.quantityfactor                       
                      
                    })

                });
               
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
