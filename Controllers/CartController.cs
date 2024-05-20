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
    [RoutePrefix("api/Cart")]
    public class CartController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public CartController()
        {
            appDbContex = new ApplicationDbContext();
        }
        [HttpPost]
        [Route("addtocart")]
        public async Task<ResponseStatus> addtocart(cartRequest cartRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (cartRequest.id == "0")
                {
                    var cartname = appDbContex.Cartdetails.Where(a => a.productId == cartRequest.productId && a.browserId == cartRequest.browserId && a.httpContextSessionId == cartRequest.httpContextSessionId && a.deleted == false).FirstOrDefault();
                    if (cartname == null)
                    {
                        var guId = Guid.NewGuid();
                        cartdetails cartmaster = new cartdetails
                        {
                            id = guId.ToString(),
                            productId = cartRequest.productId,
                            customerId = cartRequest.customerId,
                            browserId = cartRequest.browserId,
                            httpContextSessionId = cartRequest.httpContextSessionId,
                            price = cartRequest.price,
                            discountper = cartRequest.discountper,
                            discountprice = cartRequest.discountprice,
                            unitprice = cartRequest.unitprice,
                            totalprice = cartRequest.totalprice,
                            quantity = cartRequest.quantity,
                            createAt = DateTime.UtcNow,

                            deleted = false

                        };
                        appDbContex.Cartdetails.Add(cartmaster);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "added successfully";
                        status.objItem = guId.ToString();
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Item Already Added!";
                    }
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
        [Route("updateQuantity")]
        public async Task<ResponseStatus> updateQuantity(cartRequest cartRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                var cartname = appDbContex.Cartdetails.Where(a => a.productId == cartRequest.productId && a.browserId == cartRequest.browserId && a.httpContextSessionId == cartRequest.httpContextSessionId && a.id == cartRequest.id && a.deleted == false).FirstOrDefault();
                if (cartname == null)
                {
                    var cart = appDbContex.Cartdetails.Where(a => a.id == cartRequest.id).SingleOrDefault();
                    if (cart != null)
                    {
                        cart.quantity = cartRequest.quantity;
                        cart.totalprice = cartRequest.totalprice;
                        // category.updateAt = DateTime.Now;

                        // category.image = categoryRequest.image;

                        //appDbContex.Categories(category);
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.objItem = cartRequest.id;
                        status.message = "item quantity Updated Successfully!";
                        return status;

                    }
                }
                else
                {
                    status.status = false;
                    status.message = "Item not exixts!";
                }
                // }

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
        [Route("deletecart")]
        public async Task<ResponseStatus> deletecart(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var cart = appDbContex.Cartdetails.Where(a => a.id == id).SingleOrDefault();
                if (cart != null)
                {

                    // appDbContex.Categories.Update(category);
                    appDbContex.Cartdetails.Remove(cart);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "deleted Successfully!";
                    return status;

                }
                status.status = false;
                status.message = "item not Exists!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpPost]
        [Route("getcartdetail")]
        public async Task<ResponseStatus> getcartdetails(getcartrequest getcartrequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var cartdetail = appDbContex.Cartdetails.Where(a => a.browserId == getcartrequest.browserId && a.httpContextSessionId == getcartrequest.httpContextSessionId).ToList();
                status.status = true;
                status.lstItems = cartdetail;
                // status.message = "Order Status Updated Successfully!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}