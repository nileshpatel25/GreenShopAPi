using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace apiGreenShop
{
    [HubName("chat")]
    public class hub : Hub
    {
        public string msg = string.Empty;
        public void Hello()
        {
            Clients.All.messageReceived(msg);
        }
       
    }
}