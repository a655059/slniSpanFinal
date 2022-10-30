using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using prjiSpanFinal.ViewModels;
using System.Text;
using System.Text.Json;
using prjiSpanFinal.ViewModels.App;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjiSpanFinal.Controllers
{
    
    public class MsgApiController : Controller
    {
        private IWebHostEnvironment _enviro;
        public MsgApiController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        [HttpGet]
        public IActionResult Getchat(string scid, string sid)
        {
            var cid = Convert.ToInt32(scid);
            var id = Convert.ToInt32(sid);
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var cl = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id) || (i.SendFrom == id && i.SendTo == cid)).ToList();

            var q = dbcontext.ChatLogs.Where(i => (i.SendFrom == cid && i.SendTo == id)).ToList();
            foreach (var item in q)
            {
                item.HaveRead = true;
            }
            dbcontext.SaveChanges();
            return Json(cl);
        }

        [HttpGet]
        public IActionResult GetmembyAcc(string acc)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var mem = dbcontext.MemberAccounts.Where(m => m.MemberAcc == acc).Select(m => new { m.MemberId, m.MemberAcc, m.MemPic }).FirstOrDefault();
            if(mem != null)
            {
                if (mem.MemPic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { mem.MemberId, mem.MemberAcc, MemPic = System.IO.File.ReadAllBytes(path) });
                }
                return Json(mem);
            }
            return Json(null);
        }
        public IActionResult GetmembyId(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var mem = dbcontext.MemberAccounts.Where(m => m.MemberId == id).Select(m => new { m.MemberId, m.MemberAcc, m.MemPic }).FirstOrDefault();
            if (mem != null)
            {
                if (mem.MemPic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { mem.MemberId, mem.MemberAcc, MemPic = System.IO.File.ReadAllBytes(path) });
                }
                return Json(mem);
            }
            return Json(null);
        }
        [HttpGet]
        public IActionResult AutoComplete(string keyword)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var memacc = dbcontext.MemberAccounts.Where(p => p.MemberAcc.Contains(keyword) && keyword != p.MemberAcc).Select(p => p.MemberAcc).Take(5);
            return Json(memacc);
        }
        [HttpGet]
        public void HaveRead(int msgid)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var chat = dbcontext.ChatLogs.Where(p => p.ChatLogId == msgid).Select(p => p).FirstOrDefault();
            chat.HaveRead = true;
            dbcontext.SaveChanges();
        }
        public IActionResult GetProductbyId(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var prod = dbcontext.Products.Where(p => p.ProductId == id).Select(p => new { p.ProductPics.FirstOrDefault().Pic,p.ProductName, p.ProductDetails.FirstOrDefault().Style, p.ProductDetails.FirstOrDefault().Quantity, p.ProductDetails.FirstOrDefault().UnitPrice }).FirstOrDefault();
            if (prod != null)
            {
                if (prod.Pic == null)
                {
                    string pName = "/img/imageNotFound.png";
                    string path = _enviro.WebRootPath + pName;
                    return Json(new { prod.ProductName, prod.UnitPrice, Pic = System.IO.File.ReadAllBytes(path),prod.Quantity,prod.Style });
                }
                return Json(prod);
            }
            return Json(null);
        }
        public IActionResult GetNotificationbyID(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Notifications.Where(n=>n.MemberId == id).OrderByDescending(o=>o.Time).Select(n=> new {n.HaveRead,n.IconType.IconPic,n.Link,n.Text,n.Time, n.TextContent }).ToList()); 
        }
        public void HaveReadAllNoti(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var a = dbcontext.Notifications.Where(n => n.MemberId == id).Select(n => n).ToList();
            foreach(var item in a)
            {
                item.HaveRead = true;
            }
            dbcontext.SaveChanges();
        }
        public void SendNoti(int type, int id, string text, string link, string content)
        {
            if(text == null)
            {
                text = "";
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            Notification a = new Notification() { MemberId = id,IconTypeId = type, Text = text, Link = link, HaveRead = false, Time = DateTime.Now , TextContent = content };
            dbcontext.Notifications.Add(a);
            dbcontext.SaveChanges();
        }

        public IActionResult LoginCheck(string txtAccount, string txtPW)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var mem = dbcontext.MemberAccounts.FirstOrDefault(m => m.MemberAcc == txtAccount);
            if (mem != null)
            {
                if (mem.MemberPw == txtPW)
                {
                    return Json(new CAppMember() {MemberAcc = mem.MemberAcc,Email = mem.Email, MemberId = mem.MemberId, MemberName = mem.Name, MemberPic = mem.MemPic, Phone = mem.Phone });
                }
            }
            return Json(null);
        }

        public IActionResult GetOrders(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var vm = dbcontext.Orders.Where(o => o.MemberId == id && o.StatusId != 1 && o.StatusId != 9).Select(o => new ViewModels.App.OrderListViewModel()
            {
                OrderId = o.OrderId,
                OrderDatetime = o.OrderDatetime,
                OrderStatusName = o.Status.OrderStatusName,
                Quantity = o.OrderDetails.Select(o => o.Quantity).FirstOrDefault(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).FirstOrDefault(),
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).FirstOrDefault(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).FirstOrDefault(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).FirstOrDefault(),
            }).ToList().OrderByDescending(o=>o.OrderDatetime);
            return Json(vm);
        }

        public IActionResult GetOrderDetail(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var vm = dbcontext.Orders.Where(o => o.OrderId == id).Select(o => new ViewModels.App.OrderDetailViewModel()
            {
                OrderId = o.OrderId,
                SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                SellerEmail = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Email,
                SellerName = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Name,
                SellerPhone = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Phone,
                BuyerAcc = o.Member.MemberAcc,
                BuyerEmail = o.Member.Email,
                BuyerName = o.Member.Name,
                BuyerPhone = o.Member.Phone,
                OrderDatetime = o.OrderDatetime,
                RecieveAdr = o.RecieveAdr,
                CouponName = o.Coupon.CouponName,
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
                Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
                ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
            }).FirstOrDefault();
            return Json(vm);
        }
        public IActionResult GetNewNotificationbyID(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Notifications.Where(n => n.MemberId == id && n.HaveRead == false).OrderByDescending(o => o.Time).Select(n => new CNotification(){ NotificationId = n.NotificationId, Text = n.Text, Time = n.Time, TextContent= n.TextContent}).ToList());
        }
    }
}
