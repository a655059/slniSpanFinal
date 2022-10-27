using Microsoft.AspNetCore.Http;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.OrderReq2
{
    public class OrderSortReq
    {
        public List<OrderListViewModel> SortTab(int sort, int tab, int id, int pages, int eachpage) 
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            int ordercount = dbcontext.Orders.Where(o => o.MemberId == id && o.StatusId != 1 && o.StatusId != 9).Count();
            List<OrderListViewModel> q = dbcontext.Orders.Select(o => new OrderListViewModel()
            {
                OrderId = o.OrderId,
                SellerId = o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId,
                SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                BuyerId = o.MemberId,
                BuyerAcc = o.Member.MemberAcc,
                OrderDatetime = o.OrderDatetime,
                //RecieveAdr = o.RecieveAdr,
                //FinishDate = o.FinishDate,
                //CouponName = o.Coupon.CouponName,
                //Discount = o.Coupon.Discount,
                IsFreeDelivery = o.Coupon.IsFreeDelivery,
                OrderStatusName = o.Status.OrderStatusName,
                ShipperStatusId = o.StatusId,
                //ShipperName = o.Shipper.ShipperName,
                ShipperFee = o.Shipper.Fee,
                //ShipperPhone = o.Shipper.Phone,
                //PaymentDate = o.PaymentDate,
                //ShippingDate = o.ShippingDate,
                //ReceiveDate = o.ReceiveDate,
                //PaymentName = o.Payment.PaymentName,
                PaymentFee = o.Payment.Fee,
                BuyerCommentId = o.OrderDetails.Select(o => o.Comments.Count).ToList(),
                SellerCommentId = o.CommentForCustomers.Count,
                //OrderMessage = o.OrderMessage,
                //OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
                //ProductDetailId = o.OrderDetails.Select(o => o.ProductDetailId).ToList(),
                Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
                //OrderDetailReceiveDate = o.OrderDetails.Select(o => o.ReceiveDate).ToList(),
                //ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
                ProductId = o.OrderDetails.FirstOrDefault().ProductDetail.ProductId,
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
                OrderCount = ordercount,
            }).ToList();
            if (tab == 0)
            {
                q = q.Where(o => o.BuyerId == id && o.ShipperStatusId != 1 && o.ShipperStatusId != 9).ToList();
            }
            else
            {
                q = q.Where(o => o.BuyerId == id && o.ShipperStatusId == tab && o.ShipperStatusId != 1 && o.ShipperStatusId != 9).ToList();
            }
            if(sort == 0)
            {
                q = q.OrderByDescending(o => o.OrderDatetime).ToList();
            }
            else if(sort == 1)
            {
                q = q.OrderBy(o => o.OrderDatetime).ToList();
            }
            else if(sort == 2)
            {
                q = q.OrderByDescending(o => o.PaymentFee + o.ShipperFee + o.Quantity.Select((Value, index) => Value * Convert.ToInt32(o.Unitprice[index])).Sum()).ToList();
            }
            else if (sort == 3)
            {
                q = q.OrderBy(o => o.PaymentFee + o.ShipperFee + o.Quantity.Select((Value, index) => Value * Convert.ToInt32(o.Unitprice[index])).Sum()).ToList();
            }
            return q.Skip((pages - 1) * eachpage).Take(eachpage).ToList();

        }
        public List<OrderListViewModel> SearchOrder(string keyword, DateTime startdate, DateTime enddate, int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            List<OrderListViewModel> q = dbcontext.Orders.Where(o=>o.OrderDatetime >= startdate && o.OrderDatetime <= enddate && o.StatusId != 1 && o.StatusId != 9).Select(o => new OrderListViewModel()
            {
                OrderId = o.OrderId,
                SellerId = o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId,
                SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                BuyerId = o.MemberId,
                BuyerAcc = o.Member.MemberAcc,
                OrderDatetime = o.OrderDatetime,
                //RecieveAdr = o.RecieveAdr,
                //FinishDate = o.FinishDate,
                //CouponName = o.Coupon.CouponName,
                //Discount = o.Coupon.Discount,
                IsFreeDelivery = o.Coupon.IsFreeDelivery,
                OrderStatusName = o.Status.OrderStatusName,
                ShipperStatusId = o.StatusId,
                //ShipperName = o.Shipper.ShipperName,
                ShipperFee = o.Shipper.Fee,
                //ShipperPhone = o.Shipper.Phone,
                //PaymentDate = o.PaymentDate,
                //ShippingDate = o.ShippingDate,
                //ReceiveDate = o.ReceiveDate,
                //PaymentName = o.Payment.PaymentName,
                PaymentFee = o.Payment.Fee,
                //OrderMessage = o.OrderMessage,
                //OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
                //ProductDetailId = o.OrderDetails.Select(o => o.ProductDetailId).ToList(),
                Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
                //OrderDetailReceiveDate = o.OrderDetails.Select(o => o.ReceiveDate).ToList(),
                //ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
                ProductId = o.OrderDetails.FirstOrDefault().ProductDetail.ProductId,
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
            }).Where(o => o.BuyerId == id).ToList();
            if(keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for(int i = 0; i < keys.Length; i++)
                {
                    q = q.Where(o=>o.OrderStatusName.Contains(keys[i]) || o.ProductName.Contains(keys[i]) || o.SellerAcc.Contains(keys[i]) || o.Style.Any(str => str.Contains(keys[i]))).Select(o => o).ToList();
                }
            }
            return q;
        }
    }
}
