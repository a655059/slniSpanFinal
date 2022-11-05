using Microsoft.AspNetCore.SignalR;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class SelectedBiddingItemsCountdownHub :Hub
    {
        public async Task SelectedBiddingItemsCountdown(List<int> biddingIDs)
        {
            if (biddingIDs.Count > 0)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var biddings = dbContext.Biddings.ToList();
                while (true)
                {
                    List<string> remainingTimes = new List<string>();

                    foreach (var a in biddingIDs)
                    {
                        DateTime endTime = biddings.Where(i => i.BiddingId == a).Select(i => i.EndTime).FirstOrDefault();
                        TimeSpan remainingTime = endTime - DateTime.Now;
                        string time = remainingTime.Days + "天" + remainingTime.Hours + "時" + remainingTime.Minutes + "分" + remainingTime.Seconds + "秒";
                        remainingTimes.Add(time);
                    }
                    await Clients.All.SendAsync("ShowItemCountdown", remainingTimes, biddingIDs);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
