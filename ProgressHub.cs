using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace apiGreenShop
{
    public class ProgressHub : Hub
    {
        public string msg = string.Empty;
        public int count = 0;

        public void CallLongOperation()
        {
            Clients.Caller.sendMessage(msg);
        }
    }
}