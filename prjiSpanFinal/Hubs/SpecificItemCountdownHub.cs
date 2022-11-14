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
        public async Task SpecificItemCountdown(/*int biddingID*/)
        {
            //if (biddingID > 0)
            //{
            //    iSpanProjectContext dbContext = new iSpanProjectContext();
            //    var bidding = dbContext.Biddings.Where(i => i.BiddingId == biddingID).Select(i => i).FirstOrDefault();
            //    DateTime endTime = bidding.EndTime;
            //    while (true)
            //    {
            //        TimeSpan remainingTime = endTime - DateTime.Now;
            //        string time = remainingTime.Days + "天" + remainingTime.Hours + "時" + remainingTime.Minutes + "分" + remainingTime.Seconds + "秒";
            //        if (DateTime.Now >= endTime)
            //        {
            //            time = "-" + time;
            //        }
            //        await Clients.All.SendAsync("ShowSpecificItemCountdown", time, biddingID);

            //        Thread.Sleep(1000);
            //    }
            //}
        }
    }
}
