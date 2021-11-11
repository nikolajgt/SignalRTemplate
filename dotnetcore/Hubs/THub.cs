using dotnetcore.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Hubs
{               //STRONGLY TYPED T HUB, BRUGER IKKE .SendAsync
                //MSDN: "The dynamic type enables the operations in which it occurs to bypass compile-time type checking.
                //Instead, these operations are resolved at runtime. The dynamic type simplifies access to COM APIs such
                //as the Office Automation APIs, and also to dynamic APIs such as IronPython libraries, and to the
                //HTML Document Object Model (DOM)."
    public class THub : Hub<ITHub>                          
    {
        public Task Send(string message)
        {
            return Clients.All.Send(message);
        }
    }
}
