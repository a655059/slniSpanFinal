using Microsoft.AspNetCore.SignalR;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class SpecificItemCountdownHub : Hub
    {
        public async Task SpecificItemCountdown(int biddingID)
        {
            
            if (biddingID > 0)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                DateTime endTime = dbContext.Biddings.Where(i => i.BiddingId == biddingID).Select(i => i.EndTime).FirstOrDefault();
                while (true)
                {
                    TimeSpan remainingTime = endTime - DateTime.Now;
                    string time = remainingTime.Days + "天" + remainingTime.Hours + "時" + remainingTime.Minutes + "分" + remainingTime.Seconds + "秒";
                    await Clients.All.SendAsync("ShowSpecificItemCountdown", time);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
