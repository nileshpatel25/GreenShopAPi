using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace apiGreenShop.Helper
{
    public class PushNotificationLogic
    {
        public PushNotificationLogic()
        {

        }
        public class Message
        {
            public string[] registration_ids { get; set; }
            public Notification notification { get; set; }
            public object data { get; set; }
        }
        public class Notification
        {
            public string title { get; set; }
            public string text { get; set; }
        }

        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static string ServerKey = "AAAAMEM_WAE:APA91bF7yU5CjWAoP1n0ooq5Q0KzC91lVjaivQl0iYF4jObNKxupzEzJNTjidxoMzfNCVHNZSaHdJJs1tI9xuv5qn69dY_eVLAyArJEZ5oWPrx6fS95sbi0Y15tT1suBLTO07KpMDOkw";
        public async Task<bool> SendPushNotification(List<string> androidDeviceTocken, string title, string body, object data)
        {

            //deviceTokens: An array of strings, each string represents a FCM token provided by Firebase on each app - installation.This is going to be the list of app - installations that the notification is going to send.
            //title: It’s the bold section of a notification.
            //body: It represents “Message text” field of the Firebase SDK, this is the message you want to send to the users.
            //data: These is a dynamic object, it can be whatever you want because this object is going to be used as additional information you want to send to the app, it’s like hidden information. For example an action you want to execute when the user presses on the notification or an id of some product.


            string[] deviceTokens = new string[androidDeviceTocken.Count];
            deviceTokens = androidDeviceTocken.ToArray();

            bool sent = false;
            // var ServerKey = "AIzaSyBqVyZG2xWq3_2Tng4JCz9XXyL6KxY3_4Q";
            var messageInformation = new Message()
            {
                notification = new Notification()
                {
                    title = title,
                    text = body
                },
                data = data,
                registration_ids = deviceTokens
            };
            //Object to JSON STRUCTURE => using Newtonsoft.Json;
            string jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(messageInformation);

            // Create request to Firebase API
            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
            request.Headers.TryAddWithoutValidation("Authorization", "key =" + ServerKey);
            request.Content = new StringContent(jsonMessage, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = await client.SendAsync(request);
                sent = sent && result.IsSuccessStatusCode;
            }

            return sent;




        }

    }
}