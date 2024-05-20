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
    [RoutePrefix("api/Discount")]
    public class DiscountController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public DiscountController()
        {
            appDbContex = new ApplicationDbContext();
        }
        [HttpPost]
        [Route("adddiscount")]
        public async Task<ResponseStatus> addDiscount(discountRequest discountRequest)
        {
            ResponseStatus response = new ResponseStatus();
            try
            {
                if (discountRequest.id == "0")
                {
                    var id = Guid.NewGuid();
                    DiscountMaster discountMaster = new DiscountMaster
                    {
                        id = id.ToString(),
                        vendorid = discountRequest.vendorid,
                        title = discountRequest.title,
                        discounton = discountRequest.discounton,
                        type = discountRequest.type,
                        couponcode = discountRequest.couponcode,
                        couponmaxuse = discountRequest.couponmaxuse,
                        discountper = discountRequest.discountper,
                        discountamt = discountRequest.discountamt,
                        maxdiscountamt = discountRequest.maxdiscountamt,
                        fromdate = discountRequest.fromdate,
                        todate = discountRequest.todate,
                        // menuspecific= discountRequest.menuspecific,
                        //userspecifics= discountRequest.userspecifics,
                        conditionaldiscount = discountRequest.conditionaldiscount,
                        mindiscountamt = discountRequest.mindiscountamt,
                        status = discountRequest.status,
                        createdAt = DateTime.UtcNow,
                        deleted = false

                    };
                    appDbContex.DiscountMasters.Add(discountMaster);
                    await appDbContex.SaveChangesAsync();

                    for (int i = 0; i < discountRequest.discountMenuSpecifics.Count; i++)
                    {
                        var dmid = Guid.NewGuid();
                        DiscountMenuSpecific discountMenuSpecific = new DiscountMenuSpecific
                        {
                            id = dmid.ToString(),
                            discountid = id.ToString(),
                            vendorid = discountRequest.vendorid,
                            productid = discountRequest.discountMenuSpecifics[i].id,
                            name = discountRequest.discountMenuSpecifics[i].name

                        };
                        appDbContex.DiscountMenuSpecifics.Add(discountMenuSpecific);
                        await appDbContex.SaveChangesAsync();
                    }
                    for (int i = 0; i < discountRequest.discountUserSpecifics.Count; i++)
                    {
                        var dmid = Guid.NewGuid();
                        DiscountUserSpecific discountUserSpecific = new DiscountUserSpecific
                        {
                            id = dmid.ToString(),
                            discountid = id.ToString(),
                            vendorid = discountRequest.discountUserSpecifics[i].vendorid,
                            PhoneNumber = discountRequest.discountUserSpecifics[i].PhoneNumber,
                            name = discountRequest.discountUserSpecifics[i].name

                        };
                        appDbContex.DiscountUserSpecifics.Add(discountUserSpecific);
                        await appDbContex.SaveChangesAsync();
                    }
                    response.status = true;
                    response.message = "Discount details added successfully.";
                    response.objItem = id;
                    return response;
                }
                else
                {
                    var discount = appDbContex.DiscountMasters.Where(a => a.id == discountRequest.id).SingleOrDefault();
                    if (discount != null)
                    {

                        discount.vendorid = discountRequest.vendorid;
                        discount.title = discountRequest.title;
                        discount.discounton = discountRequest.discounton;
                        discount.type = discountRequest.type;
                        discount.couponcode = discountRequest.couponcode;
                        discount.couponmaxuse = discountRequest.couponmaxuse;
                        discount.discountper = discountRequest.discountper;
                        discount.discountamt = discountRequest.discountamt;
                        discount.maxdiscountamt = discountRequest.maxdiscountamt;
                        discount.fromdate = discountRequest.fromdate;
                        discount.todate = discountRequest.todate;
                        // discount.menuspecific = discountRequest.menuspecific;
                        // discount.userspecifics = discountRequest.userspecifics;
                        discount.conditionaldiscount = discountRequest.conditionaldiscount;
                        discount.mindiscountamt = discountRequest.mindiscountamt;
                        discount.status = discountRequest.status;
                        discount.createdAt = DateTime.UtcNow;
                        discount.deleted = false;

                        await appDbContex.SaveChangesAsync();


                        var discountmenu = appDbContex.DiscountMenuSpecifics.Where(a => a.discountid == discountRequest.id).ToList();
                        if (discountmenu != null)
                        {
                            foreach(var ls in discountmenu)
                            {
                                appDbContex.DiscountMenuSpecifics.Remove(ls);
                                await appDbContex.SaveChangesAsync();
                            }
                           
                        }
                        var discountuser = appDbContex.DiscountUserSpecifics.Where(a => a.discountid == discountRequest.id).ToList();
                        if (discountuser != null)
                        {
                            foreach (var ls in discountuser)
                            {
                                appDbContex.DiscountUserSpecifics.Remove(ls);
                                await appDbContex.SaveChangesAsync();
                            }
                        }

                        for (int i = 0; i < discountRequest.discountMenuSpecifics.Count; i++)
                        {
                            var dmid = Guid.NewGuid();
                            DiscountMenuSpecific discountMenuSpecific = new DiscountMenuSpecific
                            {
                                id = dmid.ToString(),
                                discountid = discountRequest.id.ToString(),
                                vendorid = discountRequest.vendorid,
                                productid = discountRequest.discountMenuSpecifics[i].id,
                                name = discountRequest.discountMenuSpecifics[i].name

                            };
                            appDbContex.DiscountMenuSpecifics.Add(discountMenuSpecific);
                            await appDbContex.SaveChangesAsync();
                        }
                        for (int i = 0; i < discountRequest.discountUserSpecifics.Count; i++)
                        {
                            var dmid = Guid.NewGuid();
                            DiscountUserSpecific discountUserSpecific = new DiscountUserSpecific
                            {
                                id = dmid.ToString(),
                                discountid = discountRequest.id.ToString(),
                                vendorid = discountRequest.discountUserSpecifics[i].id,
                                PhoneNumber = discountRequest.discountUserSpecifics[i].PhoneNumber,
                                name = discountRequest.discountUserSpecifics[i].name

                            };
                            appDbContex.DiscountUserSpecifics.Add(discountUserSpecific);
                            await appDbContex.SaveChangesAsync();
                        }

                        response.status = true;
                        response.objItem = discountRequest.id;
                        response.message = "Discount details Updated Successfully!";
                        return response;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("alldiscountbyvendorid")]
        public async Task<ResponseStatus> getalldiscount(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                // responseStatus.lstItems = appDbContex.Categories.ToList();   
                int count = appDbContex.DiscountMasters.Where(b => b.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                responseStatus.lstItems = appDbContex.DiscountMasters.Where(b => b.deleted == false).OrderBy(a => a.createdAt).Skip(skip).Take(pageSize).Select(d=>new {
d.conditionaldiscount,
d.couponcode,
d.couponmaxuse,
d.createdAt,
d.deleted,
d.discountamt,
d.discounton,
d.discountper,
d.fromdate,
d.id,
d.maxdiscountamt,
d.mindiscountamt,
d.status,
d.title,
d.todate,
d.type,
d.vendorid,
                    DiscountMenuSpecifics=appDbContex.DiscountMenuSpecifics.Where(m=>m.discountid==d.id).Select(m=>new {
                        Id = m.productid,
                        m.name
                    }),
                    DiscountUserSpecifics=appDbContex.DiscountUserSpecifics.Where(u=>u.discountid==d.id).Select(u=>new
                    {
                        Id = u.vendorid,
                        u.PhoneNumber
                    })
                });
               // responseStatus.disountMenu = appDbContex.DiscountMenuSpecifics.Where(a => a.vendorid == vendorid).ToList();
                //responseStatus.discountUser = appDbContex.DiscountUserSpecifics.Where(a => a.vendorid == vendorid).ToList();
                responseStatus.status = true;
                responseStatus.objItem = count;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("alldiscountvendorid")]
        public async Task<ResponseStatus> alldiscount(string vendorid)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                // responseStatus.lstItems = appDbContex.Categories.ToList();   

                responseStatus.lstItems = appDbContex.DiscountMasters.Where(b => b.vendorid == vendorid && b.deleted == false).OrderBy(a => a.createdAt).ToList();
                responseStatus.status = true;
                //responseStatus.objItem = count;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("deletediscount")]
        public async Task<ResponseStatus> deletediscount(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var discount = appDbContex.DiscountMasters.Where(a => a.id == id).SingleOrDefault();
                if (discount != null)
                {
                    discount.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "discount deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "discount not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        //[HttpPost]
        //[Route("")]


    }
}