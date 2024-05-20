using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace apiGreenShop.Helper
{
    public class SendSMS
    {
        public void SendTextSms(string _Message, string _strMobile)
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
                    //string baseURL = "http://www.txtguru.in/imobile/api.php?username=secinverse&password=Sec@2020&source=BALVEG&dmobile=" + _strMobile + "&message=" + _Message + "";
                    //client.OpenRead(baseURL);
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