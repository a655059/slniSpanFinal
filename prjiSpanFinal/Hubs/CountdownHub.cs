using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class CountdownHub : Hub
    {
        public async Task Countdown(int biddingID)
        {
            
            await Clients.All.SendAsync("", biddingID);
        }
    }
}
