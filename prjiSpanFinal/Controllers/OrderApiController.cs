using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjiSpanFinal.ViewModels;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        iSpanProjectContext dbcontext = new iSpanProjectContext();
        private IWebHostEnvironment _enviro;
        public OrderApiController(IWebHostEnvironment p)
        {
            _enviro = p;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderListViewModel>>> Get(int id)
        //{
        //    return await dbcontext.Orders.Where(o => o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId == id || o.MemberId == id).
        //        Select(o => new OrderListViewModel()
        //        {
        //            OrderId = o.OrderId,
        //            SellerId = o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId,
        //            SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
        //            BuyerId = o.MemberId,
        //            BuyerAcc = o.Member.MemberAcc,
        //            OrderDatetime = o.OrderDatetime,
        //            RecieveAdr = o.RecieveAdr,
        //            FinishDate = o.FinishDate,
        //            CouponName = o.Coupon.CouponName,
        //            Discount = o.Coupon.Discount,
        //            IsFreeDelivery = o.Coupon.IsFreeDelivery,
        //            OrderStatusName = o.Status.OrderStatusName,
        //            ShipperName = o.Shipper.ShipperName,
        //            ShipperFee = o.Shipper.Fee,
        //            ShipperPhone = o.Shipper.Phone,
        //            PaymentDate = o.PaymentDate,
        //            ShippingDate = o.ShippingDate,
        //            ReceiveDate = o.ReceiveDate,
        //            PaymentName = o.Payment.PaymentName,
        //            PaymentFee = o.Payment.Fee,
        //            OrderMessage = o.OrderMessage,
        //            OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
        //            ProductDetailId = o.OrderDetails.Select(o => o.ProductDetailId).ToList(),
        //            Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
        //            OrderDetailReceiveDate = o.OrderDetails.Select(o => o.ReceiveDate).ToList(),
        //            ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
        //            Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
        //            ProductId = o.OrderDetails.FirstOrDefault().ProductDetail.ProductId,
        //            Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
        //            Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
        //            ProductName = o.OrderDetails.FirstOrDefault().ProductDetail.Product.ProductName,
        //        }).ToListAsync();
        //}

        
    }
}
