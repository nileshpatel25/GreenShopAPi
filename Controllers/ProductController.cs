using apiGreenShop.DataModel;
using apiGreenShop.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public ProductController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<ResponseStatus> addProduct(ProductRequest productRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (productRequest.Id == "0")
                {

                    var productname = appDbContex.Products.Where(a => a.code == productRequest.code && a.deleted == false).FirstOrDefault();
                    if (productname == null)
                    {
                        var guId = Guid.NewGuid();
                        Product product = new Product
                        {
                            Id = guId.ToString(),
                            categoryid = productRequest.categoryid,
                            subcategoryid = productRequest.subcategoryid,
                            subsubcategoryid = productRequest.subsubcategoryid,
                            name = productRequest.name,
                            code = productRequest.code,
                            price = productRequest.price,
                            intaxslabid = productRequest.intaxslabid,
                            unitid = productRequest.unitid,
                            //unitfactor
                            unitfactorid=productRequest.unitfactorid,

                            discountper = productRequest.discountper,
                            //wholesellerprice
                            wholesellerunitid=productRequest.wholesellerunitid,
                            wholesellerunitfactorid=productRequest.wholesellerunitfactorid,
                            wholesellerPrice=productRequest.wholesellerPrice,
                            wholesellerdiscountper=productRequest.wholesellerdiscountper,
                            wholesellerdiscountprice=productRequest.wholesellerdiscountprice,
                            //premium customer price
                            premiumunitid=productRequest.premiumunitid,
                            premiumunitfactorid=productRequest.premiumunitfactorid,
                            premiumPrice=productRequest.premiumPrice,
                            premiumdiscountper=productRequest.premiumdiscountper,
                            premiumdiscountprice=productRequest.premiumdiscountprice,

                            discription = productRequest.discription,
                            orderno = productRequest.orderno,
                            quantity = productRequest.quantity,
                            // image=categoryRequest.image,
                            deleted = false,
                            active=false,
                            createAt = DateTime.Now
                        };
                        //  memoryCache.Remove("prodcutlist");
                        appDbContex.Products.Add(product);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Product Save successfully!";
                        status.objItem = guId.ToString();
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Product Already Added!";
                    }
                }
                else
                {
                    var productname = appDbContex.Products.Where(a => a.code == productRequest.code && a.deleted == false && a.Id != productRequest.Id).SingleOrDefault();
                    if (productname == null)
                    {
                        var product = appDbContex.Products.Where(a => a.Id == productRequest.Id).SingleOrDefault();
                        if (product != null)
                        {
                            product.name = productRequest.name;
                            product.subcategoryid = productRequest.subcategoryid;
                            product.subsubcategoryid = productRequest.subsubcategoryid;
                            // category.image = categoryRequest.image;
                            product.categoryid = productRequest.categoryid;

                            product.name = productRequest.name;
                            product.code = productRequest.code;
                            product.price = productRequest.price;
                            product.intaxslabid = productRequest.intaxslabid;
                            product.unitid = productRequest.unitid;
                            product.discountper = productRequest.discountper;
                            product.discription = productRequest.discription;
                            product.orderno = productRequest.orderno;
                            product.quantity = productRequest.quantity;
                            product.updateAt = DateTime.Now;
                            //unitfactor
                            product.unitfactorid = productRequest.unitfactorid;
                            //wholesellerprice
                            product.wholesellerunitid = productRequest.wholesellerunitid;
                            product.wholesellerunitfactorid = productRequest.wholesellerunitfactorid;
                            product.wholesellerPrice = productRequest.wholesellerPrice;
                            product.wholesellerdiscountper = productRequest.wholesellerdiscountper;
                            product.wholesellerdiscountprice = productRequest.wholesellerdiscountprice;
                            //premium customer price
                            product.premiumunitid = productRequest.premiumunitid;
                            product.premiumunitfactorid = productRequest.premiumunitfactorid;
                            product.premiumPrice = productRequest.premiumPrice;
                            product.premiumdiscountper = productRequest.premiumdiscountper;
                            product.premiumdiscountprice = productRequest.premiumdiscountprice;
                            // memoryCache.Remove("prodcutlist");
                            //appDbContex.Update(product);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.objItem = product.Id;
                            status.message = "Product Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "Product Already Exists!";

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
        [Route("upload")]
        public async Task<ResponseStatus> Upload()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        string cid = Convert.ToString(httpRequest.Form["PId"]);
                        string filename = RandomNumber(1000, 50000) + postedFile.FileName;
                        var filePath1 = HttpContext.Current.Server.MapPath("~/Images/"+ filename);

                        Stream strm = postedFile.InputStream;

                        Compressimage(strm, filePath1, postedFile.FileName);
                        Product product = appDbContex.Products.Where(a => a.Id == cid).SingleOrDefault();
                        if (product != null)
                        {

                            product.image = "http://api.greenshops.in/Images/" + filename;
                            await appDbContex.SaveChangesAsync();
                        }
                    }
                    
                    status.status = true;
                    status.message = "uploaded";
                    // response = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }



                //var file = Request.Form.Files[0];

                //var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                //var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                //var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                //foreach (var header in Request.Content.Headers)
                //{
                //    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                //}

                //await content.ReadAsMultipartAsync(provider);
                //string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

                //var type = provider.FormData["type"];
                //var name = provider.FormData["name"];
                //string cid = Convert.ToString(provider.FormData["PId"]);
                //string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                //string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                //if (File.Exists(originalFileName))
                //{
                //    File.Delete(originalFileName);
                //}
                //File.Move(uploadingFileName, originalFileName);
                //Product product = appDbContex.Products.Where(a => a.Id == cid).SingleOrDefault();
                //if (product != null)
                //{

                //    product.image = "http://api.greenshops.in/Images/" + filename;
                //    await appDbContex.SaveChangesAsync();
                //}
               
                ////var httpRequest = HttpContext.Current.Request;
                ////foreach (string file in httpRequest.Files)
                ////{
                ////    var postedFile = httpRequest.Files[file];
                ////    Stream strm = filename.InputStream;
                ////    Compressimage(strm, "http://api.greenshops.in/Images/" + filename, filename);
                ////}
                    
               

                //status.status = true;
                //status.message = "uploaded";
                //// status.filePath = "http://api.clickperfect.me/Images/" + filename;


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




        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 400.0f;
                    float maxWidth = 400.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    

    //[Route("upload")]
    //[HttpPost, DisableRequestSizeLimit]
    //public async Task<string> UploadFile()
    //{
    //    try
    //    {

    //        var file = Request.Form.Files[0];
    //        string cid = Convert.ToString(Request.Form["PId"]);
    //        string folderName = Path.Combine("Content", "Images\\Product\\" + cid);
    //        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
    //        if (file.Length > 0)
    //        {
    //            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
    //            string ext = Path.GetExtension(fileName);
    //            string fullPath = Path.Combine(pathToSave, String.Concat(cid, ext));
    //            string dbPath = Path.Combine(folderName, String.Concat(cid, ext));

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
    //            Product product = await appDbContex.Products.Where(a => a.Id == cid).SingleOrDefaultAsync();
    //            if (product != null)
    //            {

    //                product.image = dbPath;
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
        [Route("productdetailsbyid")]
        public async Task<ResponseStatus> productdetailsbyid(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).FirstOrDefault();
                if (product != null)
                {
                    status.status = true;
                    status.lstItems = product;
                    return status;
                }
                else
                {
                    status.status = false;
                    status.message = "product detail not exists.";
                    return status;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("activeinactiveproduct")]
        public async Task<ResponseStatus> activeinactive(string id)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {
                    if (product.active == false)
                    {
                        product.active = true;
                        responseStatus.message = "product in Active successfully";
                    }
                    else
                    {
                        product.active = false;
                        responseStatus.message = "product Active Successfully.";
                    }
                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "product not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("updateproductprice")]
        public async Task<ResponseStatus> updateproductprice(string id, double price)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {

                    product.price = price;


                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.message = "Item price updated successfully";
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "Item not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("updateproductquantity")]
        public async Task<ResponseStatus> updateproductquantity(string id, double quantity)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {

                    product.quantity = quantity;


                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.message = "Item Quantity updated successfully";
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "Item not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        [HttpPost]
        [Route("updateproductdiscountper")]
        public async Task<ResponseStatus> updateproductdiscountper(string id, double discopuntper)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {

                    product.discountper = discopuntper;


                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.message = "Item DiscountPer updated successfully";
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "Item not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("updateproductdiscountprice")]
        public async Task<ResponseStatus> updateproductdiscountprice(string id, double discopuntprice)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {

                    product.discountprice = discopuntprice;


                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.message = "Item DiscountPrice updated successfully";
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "Item not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("deleteProduct")]
        public async Task<ResponseStatus> deleteProduct(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var product = appDbContex.Products.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {
                    product.deleted = true;
                    // memoryCache.Remove("prodcutlist");
                    //appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "Product deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Product Already Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        [HttpPost]
        [Route("AllProduct")]
        public async Task<ResponseStatus> getAllProduct(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                int count = appDbContex.Products.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var productlst = from c in appDbContex.Products.Where(b => b.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize)

                                 select new
                                 {

                                     c.name,
                                     c.unitid,
                                     unitname = appDbContex.Units.Where(u => u.Id == c.unitid).FirstOrDefault().name,
                                     c.categoryid,
                                     categoryname = appDbContex.Categories.Where(u => u.Id == c.categoryid).FirstOrDefault().name,
                                     c.active,
                                     status = (c.active == false ? "Active" : "In-Active"),
                                     c.code,
                                     c.deleted,
                                     c.price,
                                     c.discountper,
                                     c.discountprice,
                                     c.discription,
                                     c.Id,
                                     c.orderno,
                                     c.image,
                                     c.intaxslabid,
                                     slabname = appDbContex.TaxSlabMasters.Where(u => u.Id == c.intaxslabid).FirstOrDefault().stSlabName,
                                     c.quantity,
                                     c.subcategoryid,
                                     c.subsubcategoryid,
                                     c.wholesellerPrice,
                                     c.wholesellerdiscountprice,
                                     c.wholesellerdiscountper,
                                     c.premiumdiscountper,
                                     c.premiumdiscountprice,
                                     c.premiumPrice,
                                     c.unitfactorid

                                 };
                status.lstItems = productlst;
                status.objItem = count;
                status.status = true;
                return status;
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }




        [HttpGet]
        [Route("allproductlist")]
        public async Task<ResponseStatus> allproductlist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                status.lstItems = appDbContex.Products.Where(a => a.deleted == false).ToList();
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("getallproductwithquantityfactorlist")]
        public async Task<ResponseStatus> getAllCategoryproductlist(string id)
        {
            try
            {

                ResponseStatus status = new ResponseStatus();

                var productlst = from a in appDbContex.Products
                                  join p in appDbContex.UnitQuantityFactors on a.unitfactorid equals p.unitfactornameid into quantityfactor
                                 where a.Id== id

                                 select new
                                  {
                                      a.name,
                                      a.unitid,
                                      unitname = appDbContex.Units.Where(u => u.Id == a.unitid).FirstOrDefault().name,
                                      a.categoryid,
                                      categoryname = appDbContex.Categories.Where(u => u.Id == a.categoryid).FirstOrDefault().name,
                                      a.active,
                                      status = (a.active == false ? "Active" : "In-Active"),
                                      a.code,
                                      a.deleted,
                                      a.price,
                                      a.discountper,
                                      a.discountprice,
                                      a.discription,
                                      a.Id,
                                      a.orderno,
                                      a.image,
                                      a.intaxslabid,
                                      slabname = appDbContex.TaxSlabMasters.Where(u => u.Id == a.intaxslabid).FirstOrDefault().stSlabName,
                                      a.quantity,
                                      a.subcategoryid,
                                      a.subsubcategoryid,
                                     a.wholesellerPrice,
                                     a.wholesellerdiscountprice,
                                     a.wholesellerdiscountper,
                                     a.premiumdiscountper,
                                     a.premiumdiscountprice,
                                     a.premiumPrice,
                                     a.unitfactorid,
                                      subunits = quantityfactor.Select(c => new
                                      {
                                          c.id,
                                          c.unitfactornameid,
                                          unitfactorname = appDbContex.UnitFactorNames.Where(u => u.id == c.unitfactornameid).FirstOrDefault().unitfactorname,
                                          c.unitname,
                                          c.quantityfactor,
                                          c.pricefactor,
                                          c.deleted

                                      }).Where(d => d.deleted == false),
                                      a.wholesellerunitid,
                                     wholesellerunitname = appDbContex.Units.Where(u => u.Id == a.wholesellerunitid).FirstOrDefault().name,
                                     a.wholesellerunitfactorid,
                                     wholesellersubunits = quantityfactor.Select(c => new
                                     {
                                         c.id,
                                         c.unitfactornameid,
                                         unitfactorname = appDbContex.UnitFactorNames.Where(u => u.id == c.unitfactornameid).FirstOrDefault().unitfactorname,
                                         c.unitname,
                                         c.quantityfactor,
                                         c.pricefactor,
                                         c.deleted

                                     }).Where(d => d.deleted == false),
                                     a.premiumunitid,
                                     premiumunitname = appDbContex.Units.Where(u => u.Id == a.premiumunitid).FirstOrDefault().name,
                                     a.premiumunitfactorid,
                                     premiumsubunits = quantityfactor.Select(c => new
                                     {
                                         c.id,
                                         c.unitfactornameid,
                                         unitfactorname = appDbContex.UnitFactorNames.Where(u => u.id == c.unitfactornameid).FirstOrDefault().unitfactorname,
                                         c.unitname,
                                         c.quantityfactor,
                                         c.pricefactor,
                                         c.deleted

                                     }).Where(d => d.deleted == false)

                                 };

                status.lstItems = productlst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }



        [Route("UploadExcel")]
        [HttpPost]
        public async Task<ResponseStatus> Uploadexcel()
        {
            ResponseStatus status = new ResponseStatus();
            string message = "";
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;


            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                //string vendorId = Convert.ToString(httpRequest.Form["vendorid"]);
                Stream stream = file.InputStream;

                IExcelDataReader reader = null;

                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    message = "This file format is not supported";
                }

                DataSet excelRecords = reader.AsDataSet();
                reader.Close();

                var finalRecords = excelRecords.Tables[0];
                for (int i = 0; i < finalRecords.Rows.Count; i++)
                {
                    var code = finalRecords.Rows[i][4].ToString();

                    var product = appDbContex.Products.Where(a => a.code == code && a.deleted == false).FirstOrDefault();
                    if (product != null)
                    {
                        product.price = Convert.ToDouble(finalRecords.Rows[i][3].ToString());
                        product.quantity = Convert.ToDouble(finalRecords.Rows[i][4].ToString());
                        await appDbContex.SaveChangesAsync();

                    }
                }
                status.status = true;
                status.message = "Price updated successfully";
                return status;

                //if (output > 0)
                //{
                //    message = "Excel file has been successfully uploaded";
                //}
                //else
                //{
                //    message = "Excel file uploaded has fiald";
                //}

            }

            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
                status.status = true;
                status.message = result.ToString();
                return status;

            }


        }


    }
}