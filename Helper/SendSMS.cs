using apiGreenShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace apiGreenShop.Helper
{
    public class SendSMS
    {
        public ApplicationDbContext appDbContex { get; }
        public SendSMS()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        public void SendTextSms(string _Message, string _strMobile)
        {
            

            try
            {
                string smslink = string.Empty;
                var sms = appDbContex.SMSLinks.Where(a => a.code == "SMS").SingleOrDefault();
                if (sms != null)
                {
                    smslink = sms.link;
                }

                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (!string.IsNullOrEmpty(_strMobile.ToString()))
                {
                    WebClient client = new WebClient();

                    string to, message;
                    to = _strMobile;
                    message = _Message;
                    string baseURL = "" + smslink +" &dmobile=" + _strMobile + "&message=" + _Message + "";
                    client.OpenRead(baseURL);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return "Success";



        }


        public void sendpasswordsms(string _Message, string _strMobile)
        {


            try
            {
               

                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                // ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (!string.IsNullOrEmpty(_strMobile.ToString()))
                {
                    WebClient client = new WebClient();

                    string to, message;
                    to = _strMobile;
                    message = _Message;
                    string baseURL = "http://www.txtguru.in/imobile/api.php?username=secinverse&password=Sec@2020&source=GRNSHP&dmobile=" + _strMobile + "&dltentityid=1501441490000014972&dltheaderid=1507162280990186383&dlttempid=1507162280990186383&message=" + _Message + "";
                    // string baseURL = "" + smslink +" &dmobile=" + _strMobile + "&message=" + _Message + "";
                    client.OpenRead(baseURL);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return "Success";



        }

    }
}