using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class OrderDetailViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return View(dbcontext.Orders.Where(o => o.OrderId == id).Select(o => new OrderDetailViewModel()
                {
                    OrderId = o.OrderId,
                    SellerId = o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId,
                    SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                    BuyerId = o.MemberId,
                    BuyerAcc = o.Member.MemberAcc,
                    OrderDatetime = o.OrderDatetime,
                    RecieveAdr = o.RecieveAdr,
                    FinishDate = o.FinishDate,
                    CouponName = o.Coupon.CouponName,
                    Discount = o.Coupon.Discount,
                    IsFreeDelivery = o.Coupon.IsFreeDelivery,
                    OrderStatusName = o.Status.OrderStatusName,
                    ShipperStatusId = o.StatusId,
                    ShipperName = o.Shipper.ShipperName,
                    ShipperFee = o.Shipper.Fee,
                    ShipperPhone = o.Shipper.Phone,
                    PaymentDate = o.PaymentDate,
                    ShippingDate = o.ShippingDate,
                    ReceiveDate = o.ReceiveDate,
                    PaymentName = o.Payment.PaymentName,
                    PaymentFee = o.Payment.Fee,
                    OrderMessage = o.OrderMessage,
                    OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
                    ProductDetailId = o.OrderDetails.Select(o => o.ProductDetailId).ToList(),
                    Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
                    OrderDetailReceiveDate = o.OrderDetails.Select(o => o.ReceiveDate).ToList(),
                    ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
                    Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
                    ProductId = o.OrderDetails.FirstOrDefault().ProductDetail.ProductId,
                    Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
                    Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
                    ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
            }).ToList());
        }
    }
}
