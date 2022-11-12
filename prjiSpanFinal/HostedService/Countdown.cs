using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using prjiSpanFinal.Hubs;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace prjiSpanFinal.HostedService
{
    public class Countdown : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<CountdownHub> _hubContext;
        public Countdown(IServiceScopeFactory serviceScopeFactory, IHubContext<CountdownHub> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hubContext = hubContext;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(BackgroundSearch, null, 0, 1000);
            return Task.CompletedTask;
        }

        async void BackgroundSearch(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<iSpanProjectContext>();
                var biddings = dbContext.Biddings.Select(i => new
                {
                    bidding = i,
                    seller = i.ProductDetail.Product.Member,
                    product = i.ProductDetail.Product,
                }).ToList();
                foreach (var a in biddings)
                {
                    if (DateTime.Now >= a.bidding.StartTime && a.product.ProductStatusId == 3)
                    {
                        var product = dbContext.Products.Where(i => i.ProductId == a.product.ProductId).Select(i => i).FirstOrDefault();
                        product.ProductStatusId = 4;
                        dbContext.SaveChanges();
                        await _hubContext.Clients.All.SendAsync("ShowUploadItem");
                        //string productName = product.ProductName;
                        //int sellerID = product.MemberId;
                        
                    }
                    if (DateTime.Now >= a.bidding.EndTime && a.product.ProductStatusId == 4)
                    {
                        var product = dbContext.Products.Where(i => i.ProductId == a.product.ProductId).Select(i => i).FirstOrDefault();
                        product.ProductStatusId = 1;
                        dbContext.SaveChanges();
                        await _hubContext.Clients.All.SendAsync("ShowUploadItem");
                        string productName = product.ProductName;

                        var biddingResult = dbContext.BiddingDetails.Where(i => i.BiddingId == a.bidding.BiddingId).OrderByDescending(i => i.Price).Select(i => new
                        {
                            topPrice = i.Price,
                            productDetailId = i.Bidding.ProductDetailId,
                            getItemMember = i.Member
                        }).FirstOrDefault();
                        if (biddingResult == null)
                        {
                            int sellerID = a.seller.MemberId;
                        }
                        else
                        {
                            var isRepeatOrder = dbContext.OrderDetails.Where(i => i.ProductDetailId == biddingResult.productDetailId && i.Unitprice == biddingResult.topPrice && i.Quantity == 1 && i.Order.MemberId == biddingResult.getItemMember.MemberId).FirstOrDefault();
                            if (isRepeatOrder == null)
                            {
                                var isExistOrder = dbContext.OrderDetails.Where(i => i.Order.MemberId == biddingResult.getItemMember.MemberId && i.Order.StatusId == 1 && i.ProductDetail.Product.MemberId == a.seller.MemberId).FirstOrDefault();
                                if (isExistOrder == null)
                                {
                                    Order order = new Order
                                    {
                                        MemberId = biddingResult.getItemMember.MemberId,
                                        RecieveAdr = "",
                                        CouponId = 1,
                                        StatusId = 1,
                                        ShipperId = 1,
                                        PaymentId = 1,
                                        OrderMessage = "",
                                    };
                                    dbContext.Orders.Add(order);
                                    dbContext.SaveChanges();
                                    int newOrderID = dbContext.Orders.Where(i => i.MemberId == biddingResult.getItemMember.MemberId).OrderByDescending(i => i.OrderId).Select(i => i.OrderId).FirstOrDefault();
                                    OrderDetail orderDetail = new OrderDetail
                                    {
                                        OrderId = newOrderID,
                                        ProductDetailId = biddingResult.productDetailId,
                                        Quantity = 1,
                                        ShippingStatusId = 1,
                                        Unitprice = biddingResult.topPrice
                                    };
                                    dbContext.OrderDetails.Add(orderDetail);
                                    dbContext.SaveChanges();
                                    
                                }
                                else
                                {
                                    var orderID = dbContext.OrderDetails.Where(i => i.Order.MemberId == biddingResult.getItemMember.MemberId && i.Order.StatusId == 1 && i.ProductDetail.Product.MemberId == a.seller.MemberId).Select(i => i.OrderId).FirstOrDefault();
                                    OrderDetail orderDetail = new OrderDetail
                                    {
                                        OrderId = orderID,
                                        ProductDetailId = biddingResult.productDetailId,
                                        Quantity = 1,
                                        ShippingStatusId = 1,
                                        Unitprice = biddingResult.topPrice
                                    };
                                    dbContext.OrderDetails.Add(orderDetail);
                                    dbContext.SaveChanges();
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
