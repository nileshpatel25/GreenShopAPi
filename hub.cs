using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace apiGreenShop
{
    public class hub : Hub
    {
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}
        public override Task OnConnected()
        {
           // Trace.WriteLine("Method: On Connected");
            return base.OnConnected();

        }
    }
}