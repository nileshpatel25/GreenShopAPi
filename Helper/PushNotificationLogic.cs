using apiGreenShop.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using PushSharp.Google;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace apiGreenShop.Helper
{
    public class PushNotificationLogic
    {

        private string FIREBASE_URL = "https://green-shop-5382b.firebaseio.com";
        private string KEY_SERVER;
        public ApplicationDbContext appDbContex { get; }
        public PushNotificationLogic(String key_server)
        {
            this.KEY_SERVER = key_server;
            appDbContex = new ApplicationDbContext();
        }

        public dynamic SendPush(PushMessage message)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FIREBASE_URL);
            request.Method = "POST";
            request.Headers.Add("Authorization", "key=" + this.KEY_SERVER);
            request.ContentType = "application/json";
            string json = JsonConvert.SerializeObject(message);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                StreamReader read = new StreamReader(response.GetResponseStream());
                String result = read.ReadToEnd();
                read.Close();
                response.Close();
                dynamic stuff = JsonConvert.DeserializeObject(result);
                return stuff;
            }
            else
            {
                throw new Exception("An error has occurred when try to get server response: " + response.StatusCode);
            }
        }

        public class PushMessage
        {
            private string _to;
            private PushMessageData _notification;

            private dynamic _data;
            private dynamic _click_action;
            public dynamic data
            {
                get { return _data; }
                set { _data = value; }
            }

            public string to
            {
                get { return _to; }
                set { _to = value; }
            }
            public PushMessageData notification
            {
                get { return _notification; }
                set { _notification = value; }
            }

            public dynamic click_action
            {
                get
                {
                    return _click_action;
                }

                set
                {
                    _click_action = value;
                }
            }
        }

        public class PushMessageData
        {
            private string _title;
            private string _text;
            private string _sound = "default";
            private string _click_action;
            public string sound
            {
                get { return _sound; }
                set { _sound = value; }
            }

            public string title
            {
                get { return _title; }
                set { _title = value; }
            }
            public string body
            {
                get { return _text; }
                set { _text = value; }
            }

            public string click_action
            {
                get
                {
                    return _click_action;
                }

                set
                {
                    _click_action = value;
                }
            }
        }

        public bool SendPushNotification(string _deviceToken, string message, string title, string notificationCategory)
        {
            bool result = false;
            try
            {
                PushNotificationLogic push = new PushNotificationLogic("AAAA6rvh0qA:APA91bFuP8qIu2lfgu2HK0yc8w3c_iDtb6TdeA8dLYk4yuXcY3FoRiLa_6_jfldgkmd_YaKvnDLKHl5-nKqgjed3attjsHa_elVdFZLZo2LRlqPZh7lrbWVCQ1QdGYC2UsSxcEtZmNTZ");
                push.SendPush(new PushMessage()
                {

                    to = _deviceToken, //for a topic to": "/topics/foo-bar"
                    notification = new PushMessageData
                    {
                        title = title,
                        body = message

                    },
                    data = new
                    {
                        title = title,
                        body = message
                    }
                });

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        public string ExcutePushNotification(string title, string msg, string fcmToken, object data)
        {

            var serverKey = "AAAA6rvh0qA:APA91bFuP8qIu2lfgu2HK0yc8w3c_iDtb6TdeA8dLYk4yuXcY3FoRiLa_6_jfldgkmd_YaKvnDLKHl5-nKqgjed3attjsHa_elVdFZLZo2LRlqPZh7lrbWVCQ1QdGYC2UsSxcEtZmNTZ";
            var senderId = "1008174486176";


            var result = "-1";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";


            var payload = new
            {
                notification = new
                {
                    title = title,
                    body = msg,
                    sound = "default"
                },

                data = new
                {
                    info = data
                },
                to = fcmToken,
                priority = "high",
                content_available = true,

            };


            var serializer = new JavaScriptSerializer();

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }






        public void sendpushnotificationtouser(string message, string userid)
        {
            PushNotificationLogic pushNotificationLogic = new PushNotificationLogic("");
            var myobj = new { Name = "GreenShop", City = "Valsad" };
            var user = appDbContex.Pushnotificationids.Where(p => p.userid == userid).ToList();
            if (user != null)
            {
                foreach (var ls in user)
                {
                    ExcutePushNotification("Green Shop", message, ls.pushId, myobj);
                }
            }
        }





    }
}