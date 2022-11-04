using Microsoft.AspNetCore.SignalR;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace prjiSpanFinal.Hubs
{
    public class CountdownHub : Hub
    {
        public async Task Countdown()
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            //DateTime endTime = dbContext.Biddings.Where(i => i.BiddingId == 3).Select(i => i.EndTime).FirstOrDefault();
            
            while (true)
            {
                Thread.Sleep(1000);
                var biddings = dbContext.Biddings.Select(i => new 
                {
                    bidding = i,
                    product = i.ProductDetail.Product,
                }).ToList();
                foreach (var a in biddings)
                {
                    if (DateTime.Now >= a.bidding.StartTime && a.product.ProductStatusId == 3)
                    {
                        var product = dbContext.Products.Where(i => i.ProductId == a.product.ProductId).Select(i => i).FirstOrDefault();
                        product.ProductStatusId = 4;
                        dbContext.SaveChanges();
                        string productName = product.ProductName;
                        await Clients.All.SendAsync("ShowUploadItem", productName);
                    }
                    if (DateTime.Now > a.bidding.EndTime && a.product.ProductStatusId == 4)
                    {
                        var product = dbContext.Products.Where(i => i.ProductId == a.product.ProductId).Select(i => i).FirstOrDefault();
                        product.ProductStatusId = 1;
                        dbContext.SaveChanges();
                        string productName = product.ProductName;
                        await Clients.All.SendAsync("ShowPullItem", productName);
                    }
                }
                //TimeSpan remainingTime = endTime - DateTime.Now;
                //string time = remainingTime.Days + "天" + remainingTime.Hours + "時" + remainingTime.Minutes + "分" + remainingTime.Seconds + "秒";
                
                //await Clients.All.SendAsync("ShowUploadAndPull", );
            }

            
        }
    }
}
