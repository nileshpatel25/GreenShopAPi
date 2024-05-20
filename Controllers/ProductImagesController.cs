using apiGreenShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/ProductImages")]
    public class ProductImagesController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public ProductImagesController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }


        [HttpPost]
        [Route("upload")]
        public async Task<ResponseStatus> Upload()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                //var file = Request.Form.Files[0];
                // var file = Request.Form.Files[0];


                var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                await content.ReadAsMultipartAsync(provider);
                string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

                var type = provider.FormData["type"];
                var name = provider.FormData["name"];
                string pid = Convert.ToString(provider.FormData["PId"]);
                string id = Convert.ToString(provider.FormData["id"]);
                if (id == "undefined")
                {
                    var guId = Guid.NewGuid();
                    id = guId.ToString();
                }
                //  string cid = Convert.ToString(provider.FormData["PId"]);
                string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
                productimages productimages = appDbContex.Productimages.Where(a => a.id == id).SingleOrDefault();
                if (productimages != null)
                {

                    productimages.image = "http://api.greenshops.in/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }
                else
                {

                    productimages productimages1 = new productimages()
                    {
                        id = id,
                        image = "http://api.greenshops.in/Images/" + filename,
                        productid = pid,
                        deleted = false,
                        createAt = DateTime.Now

                    };
                    appDbContex.Productimages.Add(productimages1);
                    await appDbContex.SaveChangesAsync();
                }


                status.status = true;
                status.message = "uploaded";
                // status.filePath = "http://api.clickperfect.me/Images/" + filename;


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.ToString();
            }
            return status;
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        //[Route("upload")]
        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<string> UploadFile()
        //{
        //    try
        //    {

        //        var file = Request.Form.Files[0];
        //        string pid = Convert.ToString(Request.Form["PId"]);
        //        string id = Convert.ToString(Request.Form["id"]);

        //        if (id == "undefined")
        //        {
        //            var guId = Guid.NewGuid();
        //            id = guId.ToString();
        //        }

        //        string folderName = Path.Combine("Content", "Images\\Product\\" + id);
        //        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            string ext = Path.GetExtension(fileName);
        //            string fullPath = Path.Combine(pathToSave, String.Concat(id, ext));
        //            string dbPath = Path.Combine(folderName, String.Concat(id, ext));

        //            FileInfo fl = new FileInfo(fullPath);
        //            if (fl.Exists)
        //            {
        //                fl.Delete();
        //            }

        //            if (!Directory.Exists(pathToSave))
        //            {
        //                Directory.CreateDirectory(pathToSave);
        //            }

        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //                await file.CopyToAsync(stream);
        //            }
        //            productimages productimages = await appDbContex.Productimages.Where(a => a.id == id).SingleOrDefaultAsync();
        //            if (productimages != null)
        //            {

        //                productimages.image = dbPath;
        //                await appDbContex.SaveChangesAsync();
        //            }
        //            else
        //            {

        //                productimages productimages1 = new productimages()
        //                {
        //                    id = id,
        //                    image = dbPath,
        //                    productid = pid,
        //                    deleted = false,
        //                    createAt = DateTime.Now

        //                };
        //                appDbContex.Add(productimages1);
        //                await appDbContex.SaveChangesAsync();
        //            }


        //        }


        //        return "success";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "error";
        //    }
        //}

        [HttpPost]
        [Route("allimagebyproductid")]
        public async Task<ResponseStatus> allimagesbyproductid(string id)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                responseStatus.lstItems = (from c in appDbContex.Productimages.Where(a => a.productid == id && a.deleted == false)
                                           select new
                                           {
                                               c.id,
                                               c.image,
                                               c.productid,
                                               name = appDbContex.Products.Where(a => a.Id == c.productid).FirstOrDefault().name,
                                               c.deleted
                                           });
                responseStatus.status = true;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deleteProductimage")]
        public async Task<ResponseStatus> deleteProductimage(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var productimage = appDbContex.Productimages.Where(a => a.id == id).SingleOrDefault();
                if (productimage != null)
                {
                    productimage.deleted = true;
                    //appDbContex.Update(productimage);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "Image deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Image Already Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}