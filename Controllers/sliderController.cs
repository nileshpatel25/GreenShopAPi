using apiGreenShop.DataModel;
using apiGreenShop.Models;
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
    [RoutePrefix("api/slider")]
    public class sliderController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public sliderController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addbanner")]
        public async Task<ResponseStatus> addbanner(sliderRequest sliderRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (sliderRequest.id == "0")
                {
                    var guid = Guid.NewGuid();
                    slider slider = new slider
                    {
                        id = guid.ToString(),
                        code = sliderRequest.code,
                        deleted = false,
                        active = true,
                        fromdate = sliderRequest.fromdate,
                        todate = sliderRequest.todate,
                        orderno = sliderRequest.orderno,
                        createAt = DateTime.UtcNow
                    };
                    // memoryCache.Remove("bannerlist");
                    appDbContex.Sliders.Add(slider);
                    await appDbContex.SaveChangesAsync();
                    status.objItem = guid.ToString();
                    status.status = true;
                    status.message = "Banner details save successfully.";
                    return status;

                }
                else
                {
                    var slider = appDbContex.Sliders.Where(a => a.id == sliderRequest.id).FirstOrDefault();
                    if (slider != null)
                    {
                        slider.code = sliderRequest.code;
                        slider.fromdate = sliderRequest.fromdate;
                        slider.todate = sliderRequest.todate;
                        slider.orderno = sliderRequest.orderno;
                        slider.updateAt = DateTime.UtcNow;

                    }
                    // memoryCache.Remove("bannerlist");
                    //appDbContex.Update(slider);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.objItem = sliderRequest.id;
                    status.message = "Banner details updated successfully";
                    return status;
                }
                //  return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("allbannerlist")]
        public async Task<ResponseStatus> allbannerlist(int pageNo, int pageSize)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var cachekey = "bannerlist";
              
                int count = appDbContex.Sliders.Where(a => a.deleted == false).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var bannerlst = appDbContex.Sliders.Where(a => a.deleted == false).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = bannerlst;
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
        [Route("allactivebannerlist")]
        public async Task<ResponseStatus> allactivebannerlist()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
              
                var bannerlst = appDbContex.Sliders.Where(a => a.deleted == false && a.active==true).OrderByDescending(a => a.createAt).ToList();
                status.lstItems = bannerlst;              
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("activateinactivatesbanner")]
        public async Task<ResponseStatus> activateslider(string id)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {

                var slider = appDbContex.Sliders.Where(a => a.id == id).SingleOrDefault();
                if (slider != null)
                {
                    if (slider.active == false)
                    {
                        slider.active = true;
                        responseStatus.message = "Activate slider successfully";
                    }
                    else
                    {
                        slider.active = false;
                        responseStatus.message = "inactivate slider successfully.";
                    }
                    // memoryCache.Remove("prodcutlist");
                    //  appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();
                    responseStatus.status = true;
                    responseStatus.objItem = slider.id;
                    return responseStatus;

                }
                responseStatus.status = false;
                responseStatus.message = "slider not exists!";
                return responseStatus;
                                            
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deletebanner")]
        public async Task<ResponseStatus> inactivateslider(string id)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            try
            {
                var slider = appDbContex.Sliders.Where(a => a.id == id).SingleOrDefault();
                if (slider != null)
                {
                    slider.deleted = true;
                    slider.updateAt = DateTime.UtcNow;
                    //  memoryCache.Remove("bannerlist");
                    await appDbContex.SaveChangesAsync();

                }
                responseStatus.message = "Banner deleted successfully";
                responseStatus.status = true;

                return responseStatus;
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
                //var file = Request.Form.Files[0];
                // var file = Request.Form.Files[0];


                //var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                //var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                //var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                //foreach (var header in Request.Content.Headers)
                //{
                //    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                //}

                //await content.ReadAsMultipartAsync(provider);
                //string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();
                //var guId = Guid.NewGuid();
                //var type = provider.FormData["type"];
                //var name = provider.FormData["name"];
                ////  string pid = Convert.ToString(provider.FormData["SId"]);
                //string id = Convert.ToString(provider.FormData["SId"]);
                //if (!string.IsNullOrEmpty(id))
                //{
                //    id = Convert.ToString(provider.FormData["SId"]);
                //}
                //else
                //{
                //    id = guId.ToString();
                //}
                ////  string cid = Convert.ToString(provider.FormData["PId"]);
                //string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                //string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                //if (File.Exists(originalFileName))
                //{
                //    File.Delete(originalFileName);
                //}
                //File.Move(uploadingFileName, originalFileName);
                //  productimages productimages = appDbContex.Productimages.Where(a => a.id == id).SingleOrDefault();


                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var guId = Guid.NewGuid();
                        string id = Convert.ToString(httpRequest.Form["SId"]);
                        if (!string.IsNullOrEmpty(id))
                        {
                            id = Convert.ToString(httpRequest.Form["SId"]);
                        }
                        else
                        {
                            id = guId.ToString();
                        }
                        string filename = RandomNumber(1000, 50000) + postedFile.FileName;
                        var filePath1 = HttpContext.Current.Server.MapPath("~/Images/" + filename);

                        Stream strm = postedFile.InputStream;

                        Compressimage(strm, filePath1, postedFile.FileName);
                        slider slider = appDbContex.Sliders.Where(a => a.id == id).SingleOrDefault();
                        if (slider != null)
                        {

                            slider.image = "http://api.greenshops.in/Images/" + filename; 
                            // memoryCache.Remove("bannerlist");
                            await appDbContex.SaveChangesAsync();
                        }
                        else
                        {
                            slider slider1 = new slider
                            {
                                id = id,
                                image = "http://api.greenshops.in/Images/" + filename,
                                deleted = false,
                                active = false,
                                createAt = DateTime.UtcNow

                            };
                            appDbContex.Sliders.Add(slider);
                            //  memoryCache.Remove("bannerlist");
                            await appDbContex.SaveChangesAsync();


                        }
                    }
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


        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {


            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 900.0f;
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
        //    ResponseStatus status = new ResponseStatus();
        //    try
        //    {

        //        var file = Request.Form.Files[0];
        //        var guId = Guid.NewGuid();
        //        string sid = Convert.ToString(Request.Form["SId"]);
        //        if (!string.IsNullOrEmpty(sid))
        //        {
        //            sid = Convert.ToString(Request.Form["SId"]);
        //        }
        //        else
        //        {
        //            sid = guId.ToString();
        //        }


        //        string folderName = Path.Combine("Content", "Images\\Slider\\" + sid);
        //        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            string ext = Path.GetExtension(fileName);
        //            string fullPath = Path.Combine(pathToSave, String.Concat(sid, ext));
        //            string dbPath = Path.Combine(folderName, String.Concat(sid, ext));

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
        //            slider slider = await appDbContex.Sliders.Where(a => a.id == sid).SingleOrDefaultAsync();
        //            if (slider != null)
        //            {

        //                slider.image = dbPath;
        //                memoryCache.Remove("bannerlist");
        //                await appDbContex.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                slider slider1 = new slider
        //                {
        //                    id = sid,
        //                    image = dbPath,
        //                    deleted = false,
        //                    active = false,
        //                    createAt = DateTime.Now

        //                };
        //                appDbContex.Add(slider);
        //                memoryCache.Remove("bannerlist");
        //                await appDbContex.SaveChangesAsync();



        //            }





        //        }


        //        return "Success";

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //}
    }
}
