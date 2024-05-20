using apiGreenShop.DataModel;
using apiGreenShop.Helper;
using apiGreenShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static apiGreenShop.ApplicationUserManager;

namespace apiGreenShop.Controllers
{
    [RoutePrefix("api/GroceryRegistartion")]
    public class GroceryRegistartionController : ApiController
    {
        SendSMS sendSMS = new SendSMS();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        // private readonly IMemoryCache memoryCache;
        public ApplicationDbContext appDbContex { get; }
        public GroceryRegistartionController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        public GroceryRegistartionController(ApplicationUserManager userManager, ApplicationRoleManager roleManager,
         ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpGet]
        [Route("getAllUserList")]
        public async Task<ResponseStatus> GetAllUSer()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                var alluser = UserManager.Users.OrderByDescending(a => a.Id).ToList();
                status.lstItems = alluser;
                // status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost]
        [Route("getAllUser")]
        public async Task<ResponseStatus> GetAllUSer(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = UserManager.Users.ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var alluser = UserManager.Users.OrderByDescending(a => a.Id).Skip(skip).Take(pageSize).ToList();
                status.lstItems = alluser;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("getUserinfo")]
        public async Task<ResponseStatus> GetAllUSerinfo(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var alluser = UserManager.Users.Where(a => a.Id == id).ToList();
                status.lstItems = alluser;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPost]
        [Route("register")]

        public async Task<ResponseStatus> Register(RegisterRequest registerRequest)
        {
            ResponseStatus status = new ResponseStatus();


            //var user = new Applicationuser
            //{
            //    UserName = registerRequest.phonenumber,
            //    PhoneNumber = registerRequest.phonenumber,
            //    Email = registerRequest.email             

            //};
            var user = new ApplicationUser() { UserName = registerRequest.phonenumber, firstname = registerRequest.name, Email = registerRequest.email, PhoneNumber = registerRequest.phonenumber, source = registerRequest.source, readonlypassword=registerRequest.password };
            var result = await UserManager.CreateAsync(user, registerRequest.password);
            var role = await RoleManager.FindByNameAsync("User");
            if (result.Succeeded)
            {
                // SendEmail sendEmail = new SendEmail();
                //sendEmail.sendemail(registerRequest.Email);
                // await signInManager.SignInAsync(user, isPersistent: false);
                IdentityResult roleresult = null;
                roleresult = await UserManager.AddToRoleAsync(user.Id, role.Name);

                var sms = appDbContex.Smsconfigurations.Where(a => a.code == "REGI" && a.deleted == false).SingleOrDefault();
                if(sms !=null)
                {
                    sendSMS.SendTextSms(sms.body.ToString(), "+91" + registerRequest.phonenumber);
                }


                status.message = "Registration successfully!";
                status.status = true;
                return status;
            }
            List<string> errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.ToString());
            }
            status.status = false;
            status.message = errors[0].ToString();
            return status;
            //  return BadRequest(new { message = errors });

        }
        //Push Notification ids
        [HttpPost]
        [Route("addpushtokenid")]
        public async Task<ResponseStatus> addpushtoken(PushnotificationidRequest pushnotificationidRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var pushid = appDbContex.Pushnotificationids.Where(a => a.userid == pushnotificationidRequest.userid && a.pushId == pushnotificationidRequest.pushId).SingleOrDefault();
                if (pushid == null)
                {
                    var guId = Guid.NewGuid();
                    pushnotificationids pushnotificationids = new pushnotificationids
                    {
                        id = guId.ToString(),
                        userid = pushnotificationidRequest.userid,
                        pushId = pushnotificationidRequest.pushId,
                        createAt = DateTime.Now
                    };
                    appDbContex.Pushnotificationids.Add(pushnotificationids);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Save successfully!";
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createrole")]
        public async Task<ResponseStatus> CreateRole(createrolerequest createRoleRequest)
        {
            ResponseStatus status = new ResponseStatus();
            IdentityRole role = new IdentityRole(createRoleRequest.RoleName);
            IdentityResult result = await RoleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                status.status = true;
                status.message = "Role created successfully!";
                return status;

            }
            status.status = false;
            status.message = "role already exists!";
            return status;

        }

        [HttpPost]
        [Route("addaddress")]
        public async Task<ResponseStatus> addaddress(address addressrequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var guId = Guid.NewGuid();
                Addresses addresses = new Addresses
                {
                    id = guId.ToString(),
                    userid = addressrequest.userid,
                    address = addressrequest.addres,
                    address2 = addressrequest.address2,
                    landmark = addressrequest.landmark,
                    city = addressrequest.city,
                    pincode = addressrequest.pincode,
                    remark = addressrequest.remark,
                    deleted = false,
                    creatAt = DateTime.Now
                };
                // memoryCache.Remove("citylist");
                appDbContex.Addresses.Add(addresses);
                await appDbContex.SaveChangesAsync();
                status.status = true;
                status.message = "Save successfully!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("profileupdate")]
        public async Task<ResponseStatus> profileupdate(profileRequest profileRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                var user = UserManager.FindById(profileRequest.id);
                //  var guId = Guid.NewGuid();

                user.Id = profileRequest.id;
                user.firstname = profileRequest.firstname;
                user.lastname = profileRequest.lastname;
                user.address = profileRequest.address;
                user.address2 = profileRequest.address2;
                user.landmark = profileRequest.landmark;
                user.city = profileRequest.city;
                user.pincode = profileRequest.pincode;
                user.othercontactno = profileRequest.othercontactno;


                // memoryCache.Remove("citylist");
                //   appDbContex.Addresses.Add(addresses);
                await UserManager.UpdateAsync(user);

                status.status = true;
                status.message = "Profile updated successfully!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("uploadprofilepic")]
        public async Task<ResponseStatus> Upload()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                //var file = Request.Form.Files[0];

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
                string cid = Convert.ToString(provider.FormData["Id"]);
                string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
                ApplicationUser user = appDbContex.Users.Where(a => a.Id == cid).SingleOrDefault();
                if (user != null)
                {

                    user.profilepic = "http://api.greenshops.in/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "Profile pic updated successfully";
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

        //[HttpGet]
        //[Route("getuserclaims")]
        //public Accountmodel GetUserclaims()
        //{
        //    var identityClaims = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identityClaims.Claims;
        //    Accountmodel model = new Accountmodel()
        //    {
        //        id = identityClaims.FindFirst("id").Value,
        //        email = identityClaims.FindFirst("email").Value,
        //        phonenumebr = identityClaims.FindFirst("phonenumebr").Value,
        //    };
        //    return model;
        //}

        [HttpPost]
        [Route("sendotp")]
        public async Task<ResponseStatus> sendotp(string mobileno)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                int otpvalue = RandomNumber(100000, 999999);
                sendSMS.SendTextSms("Your OTP Number is "+ otpvalue + ", Green Shop.", "91" + mobileno);
                HttpContext.Current.Session["OTP"] = otpvalue;
                HttpContext.Current.Session["MOBILENO"] = mobileno;
                responseStatus.message = "OTP sent sucsessfully.";
                responseStatus.status = true;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("verifyotp")]
        public async Task<ResponseStatus> verifyotp(string otpvalue)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                if(HttpContext.Current.Session["OTP"].ToString()== otpvalue.ToString())
                {
                    responseStatus.status = true;
                    responseStatus.message = "OTP verify successfully";
                   
                }
                else
                {
                    responseStatus.status = false;
                    responseStatus.message = "Invalid OTP";
                }

                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost]
        [Route("resetpassword")]
        public async Task<ResponseStatus> resetpassword(string password)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                var user = await UserManager.FindByNameAsync(HttpContext.Current.Session["MOBILENO"].ToString());
                if(user!=null)
                {
                  
                    //IdentityResult result = await UserManager.AddPasswordAsync(user.Id, password);

                    //if (!result.Succeeded)
                    //{

                    // }

                    UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(appDbContex);
                    UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
                    //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
                    //await store.SetPasswordHashAsync(user, hashedNewPassword);
                    //await store.UpdateAsync(user);

                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
                    ApplicationUser cUser = await store.FindByIdAsync(user.Id);
                    await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                    await store.UpdateAsync(cUser);

                    responseStatus.status = true;
                    responseStatus.message = "Password reset sucessfully";
                }           




                return responseStatus;
            }
            catch (Exception ex)
              {

                throw ex;
            }

        }


        [HttpPost]
        [Route("getpassword")]
        public async Task<ResponseStatus> getpassword(string mobileno)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                var user = await UserManager.FindByNameAsync(mobileno);
                if (user != null)
                {
                    string strpassword = user.readonlypassword;
                    responseStatus.status = true;
                    responseStatus.message = "your password has been sent you by text message.";
                    sendSMS.SendTextSms("Your Password is : " + strpassword + ", Green Shop.", "91" + mobileno);
                }
                 else
                {
                    responseStatus.status = false;
                    responseStatus.message = "Mobile no not registrated,Please create your account";
                }
               
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      




    }
}