using apiGreenShop.DataModel;
using apiGreenShop.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
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
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public CategoryController()
        {
            appDbContex = new ApplicationDbContext();
        }
        //public CategoryController(ApplicationDbContext _appdbContext)
        //{
        //    this.appDbContex = _appdbContext;

        //}

        [HttpPost]
        [Route("addcategory")]
        public async Task<ResponseStatus> addCategory(CategoryRequest categoryRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (categoryRequest.Id == "0")
                {
                    var categoryname = appDbContex.Categories.Where(a => a.name == categoryRequest.name && a.deleted == false).FirstOrDefault();
                    if (categoryname == null)
                    {
                        var guId = Guid.NewGuid();
                        Category category = new Category
                        {
                            Id = guId.ToString(),
                            subcategoryid = categoryRequest.subcategoryid,
                            subsubcategoryid = categoryRequest.subsubcategoryid,
                            name = categoryRequest.name,
                            code = categoryRequest.code,
                            orderno = categoryRequest.orderno,
                            // image=categoryRequest.image,
                            deleted = false,
                            active = false
                            //  createAt = DateTime.Now
                        };
                        appDbContex.Categories.Add(category);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Category Save successfully!";
                        status.objItem = guId.ToString();
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Category Already Added!";
                    }
                }
                else
                {
                    var categoryname = appDbContex.Categories.Where(a => a.name == categoryRequest.name && a.Id != categoryRequest.Id && a.deleted == false).SingleOrDefault();
                    if (categoryname == null)
                    {
                        var category = appDbContex.Categories.Where(a => a.Id == categoryRequest.Id).SingleOrDefault();
                        if (category != null)
                        {
                            category.name = categoryRequest.name;
                            category.subcategoryid = categoryRequest.subcategoryid;
                            category.subsubcategoryid = categoryRequest.subsubcategoryid;
                            category.orderno = categoryRequest.orderno;
                            // category.updateAt = DateTime.Now;

                            // category.image = categoryRequest.image;

                            //appDbContex.Categories(category);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.objItem = categoryRequest.Id;
                            status.message = "Category Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "Category Already Exists!";

                    // return status;

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
        [Route("allcategory")]
        public async Task<ResponseStatus> getallcategory(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                // responseStatus.lstItems = appDbContex.Categories.ToList(); 
                int count = appDbContex.Categories.Where(b => b.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                responseStatus.lstItems = from c in appDbContex.Categories.Where(b => b.deleted == false).OrderBy(a => a.orderno).Skip(skip).Take(pageSize)
                                          select new
                                          {
                                              c.Id,
                                              c.name,
                                              c.code,
                                              c.subcategoryid,
                                              c.subsubcategoryid,
                                              c.deleted,
                                              c.image,
                                              c.orderno,
                                              c.active,
                                              category = appDbContex.Categories.Where(a => a.Id == c.subcategoryid).FirstOrDefault().name,
                                              subcategory = appDbContex.Categories.Where(a => a.Id == c.subsubcategoryid).FirstOrDefault().name,

                                          };
                responseStatus.status = true;
                responseStatus.objItem = count;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("categorylist")]
        public async Task<ResponseStatus> getAllCategorylist()
        {
            try
            {

                ResponseStatus status = new ResponseStatus();

                var categorylst = appDbContex.Categories.Where(a => a.deleted == false).ToList();
                status.lstItems = categorylst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("category")]
        public async Task<ResponseStatus> getAllCategory()
        {
            try
            {

                ResponseStatus status = new ResponseStatus();

                var categorylst = appDbContex.Categories.Where(a => (string.IsNullOrEmpty(a.subcategoryid)) && (string.IsNullOrEmpty(a.subsubcategoryid)) && a.deleted == false).ToList();
                status.lstItems = categorylst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("categoryfindbyid")]
        public async Task<ResponseStatus> getCategoryfindbyId(string id)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();

                var categorylst = appDbContex.Categories.Where(a => a.Id == id).ToList();
                responseStatus.status = true;
                responseStatus.lstItems = categorylst;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("subcategorylist")]
        public async Task<ResponseStatus> getSubCategoryList(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                var categorylst = appDbContex.Categories.Where(a => a.subcategoryid == id && (string.IsNullOrEmpty(a.subsubcategoryid)) && a.deleted == false).ToList();
                status.lstItems = categorylst;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost]
        [Route("deletecategory")]
        public async Task<ResponseStatus> deletecategory(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var category = appDbContex.Categories.Where(a => a.Id == id).SingleOrDefault();
                if (category != null)
                {
                    category.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "category deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "category not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


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
                        string cid = Convert.ToString(httpRequest.Form["CId"]);
                        string filename = RandomNumber(1000, 50000) + postedFile.FileName;
                        var filePath1 = HttpContext.Current.Server.MapPath("~/Images/" + filename);

                        Stream strm = postedFile.InputStream;

                        Compressimage(strm, filePath1, postedFile.FileName);
                        Category category = appDbContex.Categories.Where(a => a.Id == cid).SingleOrDefault();
                        if (category != null)
                        {

                            category.image = "http://api.greenshops.in/Images/" + filename;
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
                //string cid = Convert.ToString(provider.FormData["CId"]);
                //string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                //string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                //if (File.Exists(originalFileName))
                //{
                //    File.Delete(originalFileName);
                //}
                //File.Move(uploadingFileName, originalFileName);
                //Category category = appDbContex.Categories.Where(a => a.Id == cid).SingleOrDefault();
                //if (category != null)
                //{

                //    category.image = "http://api.greenshops.in/Images/" + filename;
                //    await appDbContex.SaveChangesAsync();
                //}

                //status.status = true;
                //status.message = "uploaded";
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
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 500.0f;
                    float maxWidth = 500.0f;
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



        [HttpPost]
        [Route("activeinactivecategory")]
        public async Task<ResponseStatus> activeinactive(string id)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var product = appDbContex.Categories.Where(a => a.Id == id).SingleOrDefault();
                if (product != null)
                {
                    if (product.active == false)
                    {
                        product.active = true;
                        responseStatus.message = "category in Active successfully";
                    }
                    else
                    {
                        product.active = false;
                        responseStatus.message = "category Active Successfully.";
                    }
                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                    hubContext.Clients.All.sendMessage("category Active/Inactive");
                    responseStatus.status = true;
                    responseStatus.objItem = product.Id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "category not exists!";
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("getallcategorywithproductlist")]
        public async Task<ResponseStatus> getAllCategoryproductlist()
        {
            try
            {

                ResponseStatus status = new ResponseStatus();


                var categorylst = appDbContex.Categories.Where(a => a.deleted == false && a.active==false).OrderBy(b => b.orderno).Select(a => new

                {

                    a.name,
                    a.Id,
                    a.image,
                    a.orderno,
                    a.subcategoryid,
                    a.subsubcategoryid,
                    a.code,
                    a.deleted,
                    a.active,
                    products = appDbContex.Products.Where(p => p.categoryid == a.Id).Select(c => new
                    {
                        c.name,
                        c.unitid,
                        unitname = appDbContex.Units.Where(u => u.Id == c.unitid).FirstOrDefault().name,
                        c.categoryid,
                        c.active,
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
                        c.quantity,
                        c.subcategoryid,
                        c.subsubcategoryid,
                        c.wholesellerPrice,
                        c.wholesellerdiscountprice,
                        c.wholesellerdiscountper,
                        c.premiumdiscountper,
                        c.premiumdiscountprice,
                        c.premiumPrice,
                        c.unitfactorid,
                        subunit = appDbContex.UnitQuantityFactors.Where(f => f.unitfactornameid == c.unitfactorid).Select(q => new
                        {
                            q.id,
                            q.quantityfactor,
                            q.pricefactor,
                            q.unitfactornameid,
                            q.unitname,
                            q.deleted
                        }),
                        c.wholesellerunitid,
                        wholesellerunitname = appDbContex.Units.Where(u => u.Id == c.wholesellerunitid).FirstOrDefault().name,
                        c.wholesellerunitfactorid,
                        wholesellersubunits = appDbContex.UnitQuantityFactors.Where(f => f.unitfactornameid == c.wholesellerunitfactorid).Select(q => new
                        {
                            q.id,
                            q.quantityfactor,
                            q.pricefactor,
                            q.unitfactornameid,
                            q.unitname,
                            q.deleted
                        }),
                        c.premiumunitid,
                        premiumunitname = appDbContex.Units.Where(u => u.Id == c.premiumunitid).FirstOrDefault().name,
                        c.premiumunitfactorid,
                        premiumsubunits = appDbContex.UnitQuantityFactors.Where(f => f.unitfactornameid == c.premiumunitfactorid).Select(q => new
                        {
                            q.id,
                            q.quantityfactor,
                            q.pricefactor,
                            q.unitfactornameid,
                            q.unitname,
                            q.deleted
                        })

                    }).Where(d => d.deleted == false && d.active==false).OrderBy(b => b.orderno)
                });

                status.lstItems = categorylst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet]
        [Route("categoryforweb")]
        public async Task<ResponseStatus> getAllCategoryweb()
        {
            try
            {

                ResponseStatus status = new ResponseStatus();

                var categorylst = appDbContex.Categories.Where(a => a.subcategoryid == "0" && a.subsubcategoryid == "0" && a.deleted == false && a.active == false).OrderBy(a => a.orderno).ToList();
                status.lstItems = categorylst;
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