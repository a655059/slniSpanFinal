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
using Newtonsoft.Json;


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
        public IActionResult Signin(string txtName, string txtAccount, string txtPW, string txtPhone, string txtEmail)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            if (dbcontext.MemberAccounts.Where(m=>m.MemberAcc == txtAccount).Any())
            {
                return Json("1");
            }
            if (dbcontext.MemberAccounts.Where(m => m.Phone == txtPhone).Any())
            {
                return Json("2");
            }
            if (dbcontext.MemberAccounts.Where(m => m.Email == txtEmail).Any())
            {
                return Json("3");
            }
            string pName = "/img/Member/nopicmem.jpg";
            string path = _enviro.WebRootPath + pName;
            byte[] content = System.IO.File.ReadAllBytes(path);
            dbcontext.MemberAccounts.Add(new MemberAccount() {MemberAcc = txtAccount, MemberPw = txtPW, Phone = txtPhone, Email = txtEmail, MemPic = content, RegionId= 1, MemStatusId = 1,Name = txtName });
            dbcontext.SaveChanges();
            return Json("0");
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

        public IActionResult GetOrdersSeller(int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var vm = dbcontext.Orders.Where(o => o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId == id && o.StatusId != 1 && o.StatusId != 9).Select(o => new ViewModels.App.OrderListViewModel()
            {
                OrderId = o.OrderId,
                OrderDatetime = o.OrderDatetime,
                OrderStatusName = o.Status.OrderStatusName,
                Quantity = o.OrderDetails.Select(o => o.Quantity).FirstOrDefault(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).FirstOrDefault(),
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).FirstOrDefault(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).FirstOrDefault(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).FirstOrDefault(),
            }).ToList().OrderByDescending(o => o.OrderDatetime);
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

        public IActionResult GetBigtypeItems(int memid, int id, int page, string st, string ship, string pay, string price1, string price2)
        {
            List<int> nst = JsonConvert.DeserializeObject<List<int>>(st);
            List<int> nship = JsonConvert.DeserializeObject<List<int>>(ship);
            List<int> npay = JsonConvert.DeserializeObject<List<int>>(pay);
            List<int> nprice1 = JsonConvert.DeserializeObject<List<int>>(price1);
            List<int> nprice2 = JsonConvert.DeserializeObject<List<int>>(price2);
            int eachpage = 2;
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            List<ViewModels.App.CShowItem> q = dbcontext.Products.Where(n => n.SmallType.BigTypeId == id && (n.ProductStatusId != 1 && n.ProductStatusId != 2)).Select(n => new ViewModels.App.CShowItem()
            {
                id = n.ProductId,
                Name = n.ProductName,
                Pic = n.ProductPics.FirstOrDefault().Pic,
                Price1 = n.ProductDetails.Select(a => a.UnitPrice).Min(),
                Price2 = n.ProductDetails.Select(a => a.UnitPrice).Max(),
                salesVolume = dbcontext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.Quantity).Sum(),
                starCount = (dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => (int)a.CommentStar).Sum() / dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count,
                IsFavourite = n.Likes.Where(k => k.MemberId == memid).Any(),
                stID = n.SmallTypeId,
                st = n.SmallType.SmallTypeName,
                shipIDList = n.Member.ShipperToSellers.Select(s => s.ShipperId).ToList(),
                payIDList = n.Member.PaymentToSellers.Select(s => s.PaymentId).ToList(),
            }).ToList();

            if(nst.Count > 0)
            {
                q=q.Where(p => nst.Contains(p.stID)).ToList();
            }

            if(nship.Count > 0)
            {
                q=q.Where(p => p.shipIDList.Intersect(nship).Any()).ToList();
            }

            if (npay.Count > 0)
            {
                q=q.Where(p => p.payIDList.Intersect(npay).Any()).ToList();
            }

            if (nprice1.Count > 0)
            {
                List<ViewModels.App.CShowItem> q1 = new List<ViewModels.App.CShowItem>();
                for(int i=0;i< nprice1.Count;i++)
                {
                    q1 = q1.Union(q.Where(p => p.Price1 >= nprice1[i] && p.Price1 <= nprice2[i] || p.Price2 >= nprice1[i] && p.Price2 <= nprice2[i]).ToList()).ToList();
                }
                q = q1;
            }

            List<int> quantiles = new List<int>();
            if(q.Count >0)
            {
                var temp = q.Select(t => (t.Price1 + t.Price2) / 2).OrderBy(y => y).ToList();
                quantiles.Add(Convert.ToInt32(Math.Round(temp[Convert.ToInt32(Math.Floor((decimal)temp.Count / (decimal)4))])));
                quantiles.Add(Convert.ToInt32(Math.Round(temp[Convert.ToInt32(Math.Floor((decimal)temp.Count * 2 / (decimal)4))])));
                quantiles.Add(Convert.ToInt32(Math.Round(temp[Convert.ToInt32(Math.Floor((decimal)temp.Count * 3 / (decimal)4))])));
                quantiles.Add(Convert.ToInt32(Math.Ceiling(temp.Max())));
            }

            foreach(var item in q)
            {
                item.stList = q.Select(a => a.st).Distinct().ToList();
                item.stIDList = q.Select(a => a.stID).Distinct().ToList();
                item.quantiles = quantiles;
            }
            q = q.Skip((page - 1) * eachpage).Take(eachpage).ToList();
            foreach(var item in q)
            {
                if(item.Pic == null)
                {
                    string pName = "/img/imageNotFound.png";
                    string path = _enviro.WebRootPath + pName;
                    item.Pic = System.IO.File.ReadAllBytes(path);
                }
            }
            return Json(q);
        }
    }
}
