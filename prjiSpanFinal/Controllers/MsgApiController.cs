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
using System.Xml.Linq;
using prjiSpanFinal.Models.App;


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

        public IActionResult GetBigtypeItems(int memid, int id, int page, string st, string ship, string pay, string price1, string price2, int sort, string keyword)
        {
            List<int> nst = JsonConvert.DeserializeObject<List<int>>(st);
            List<int> nship = JsonConvert.DeserializeObject<List<int>>(ship);
            List<int> npay = JsonConvert.DeserializeObject<List<int>>(pay);
            List<int> nprice1 = JsonConvert.DeserializeObject<List<int>>(price1);
            List<int> nprice2 = JsonConvert.DeserializeObject<List<int>>(price2);
            int eachpage = 10;
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            List<ViewModels.App.CShowItem> q;
            if (id==0)
            {
                q = dbcontext.Products.Where(n => (n.ProductStatusId != 1 && n.ProductStatusId != 2)).Select(n => new ViewModels.App.CShowItem()
                {
                    id = n.ProductId,
                    Name = n.ProductName,
                    Pic = n.ProductPics.FirstOrDefault().Pic,
                    Price1 = n.ProductDetails.Select(a => a.UnitPrice).Min(),
                    Price2 = n.ProductDetails.Select(a => a.UnitPrice).Max(),
                    salesVolume = dbcontext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == n.ProductId && (a.Order.StatusId == 6 || a.Order.StatusId == 7)).Select(a => a.Quantity).Sum(),
                    starCount = (dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => (int)a.CommentStar).Sum() / dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count,
                    IsFavourite = n.Likes.Where(k => k.MemberId == memid).Any(),
                    stID = n.SmallTypeId,
                    st = n.SmallType.SmallTypeName,
                    shipIDList = n.Member.ShipperToSellers.Select(s => s.ShipperId).ToList(),
                    payIDList = n.Member.PaymentToSellers.Select(s => s.PaymentId).ToList(),
                    time = n.EditTime,
                    styles = n.ProductDetails.Select(p => p.Style).ToList(),
                    selleracc = n.Member.MemberAcc,
                }).ToList();
            }
            else
            {
                q = dbcontext.Products.Where(n => n.SmallType.BigTypeId == id && (n.ProductStatusId != 1 && n.ProductStatusId != 2)).Select(n => new ViewModels.App.CShowItem()
                {
                    id = n.ProductId,
                    Name = n.ProductName,
                    Pic = n.ProductPics.FirstOrDefault().Pic,
                    Price1 = n.ProductDetails.Select(a => a.UnitPrice).Min(),
                    Price2 = n.ProductDetails.Select(a => a.UnitPrice).Max(),
                    salesVolume = dbcontext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == n.ProductId && (a.Order.StatusId == 6 || a.Order.StatusId == 7)).Select(a => a.Quantity).Sum(),
                    starCount = (dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => (int)a.CommentStar).Sum() / dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == n.ProductId).Select(a => a.CommentStar).ToList().Count,
                    IsFavourite = n.Likes.Where(k => k.MemberId == memid).Any(),
                    stID = n.SmallTypeId,
                    st = n.SmallType.SmallTypeName,
                    shipIDList = n.Member.ShipperToSellers.Select(s => s.ShipperId).ToList(),
                    payIDList = n.Member.PaymentToSellers.Select(s => s.PaymentId).ToList(),
                    time = n.EditTime,
                    styles = n.ProductDetails.Select(p => p.Style).ToList(),
                    selleracc = n.Member.MemberAcc,
                }).ToList();
            }
            if (nst.Count > 0)
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

            if (sort == 1)
            {
                q = q.OrderByDescending(p => p.time).ToList();
            }
            else if (sort == 2)
            {
                q = q.OrderBy(p => p.time).ToList();
            }
            else if (sort == 3)
            {
                q = q.OrderByDescending(p => p.Price2).ToList();
            }
            else if (sort == 4)
            {
                q = q.OrderBy(p => p.Price2).ToList();
            }
            else if (sort == 5)
            {
                q = q.OrderByDescending(p => p.Price1).ToList();
            }
            else if (sort == 6)
            {
                q = q.OrderBy(p => p.Price1).ToList();
            }
            else if (sort == 7)
            {
                q = q.OrderByDescending(p => p.salesVolume).ToList();
            }
            else if (sort == 8)
            {
                q = q.OrderBy(p => p.salesVolume).ToList();
            }
            else if (sort == 9)
            {
                q = q.OrderByDescending(p => p.starCount).ToList();
            }
            else if (sort == 10)
            {
                q = q.OrderBy(p => p.starCount).ToList();
            }

            if (keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    q = q.Where(o =>  o.Name.Contains(keys[i]) || o.selleracc.Contains(keys[i]) || o.styles.Any(str => str.Contains(keys[i]))).Select(o => o).ToList();
                }
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

        public IActionResult GetProductDetail(int memid, int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            CItemIndexViewModel q = new CItemIndexViewModel() {
                product = dbcontext.Products.Where(p => p.ProductId == id).Select(p=> new Models.App.CProduct() {Description = p.Description, ProductName = p.ProductName, ProductId = p.ProductId, MemberId = p.MemberId }).FirstOrDefault(),
                productDetails = dbcontext.ProductDetails.Where(p => p.ProductId == id).Select(p=> new Models.App.CProductDetail() { Pic = p.Pic, Quantity= p.Quantity, UnitPrice= p.UnitPrice, Style= p.Style,ProductDetailId= p.ProductDetailId }).ToList(),
                comments = dbcontext.Comments.Where(c=>c.OrderDetail.ProductDetail.ProductId == id).Select(p=> new Models.App.CComment() {cpics = dbcontext.CommentPics.Where(c => c.CommentId == p.CommentId).Select(p => p.CommentPic1).ToList(), commentacc = p.OrderDetail.Order.Member.MemberAcc,Comment1 = p.Comment1,Comment2=p.Comment2,Comment3=p.Comment3,CommentStar=p.CommentStar,CommentTime=p.CommentTime, memPic = p.OrderDetail.Order.Member.MemPic }).ToList(),
                commentCount = dbcontext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == id).ToList().Count,
                avgCommentStar = (dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == id).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == id).Select(a => (int)a.CommentStar).Sum() / dbcontext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == id).Select(a => a.CommentStar).ToList().Count,
                Islike = dbcontext.Likes.Where(k => k.MemberId == memid && k.ProductId == id).Any(),
                productPics = dbcontext.ProductPics.Where(p=>p.ProductId == id).Select(p=>p.Pic).ToList(),
                salesVolume = dbcontext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == id && (a.Order.StatusId == 6 || a.Order.StatusId == 7)).Select(a => a.Quantity).Sum(),
                seller = dbcontext.Products.Where(p => p.ProductId == id).Select(p=> new Models.App.CMemberAccount() {MemberAcc= p.Member.MemberAcc,MemberId=p.Member.MemberId,MemPic= p.Member.MemPic }).FirstOrDefault(),
                buyer = dbcontext.MemberAccounts.Where(m=>m.MemberId == memid).Select(p => new Models.App.CMemberAccount() { MemberAcc = p.MemberAcc, MemberId = p.MemberId, MemPic = p.MemPic }).FirstOrDefault(),
                sellerPayment = dbcontext.PaymentToSellers.Where(p=>p.MemberId == memid).Select(p=> new CSellerPaymentViewModel() { fee = p.Payment.Fee,payment= p.Payment.PaymentName,id=p.PaymentId}).ToList(),
                sellerShipper = dbcontext.ShipperToSellers.Where(p => p.MemberId == memid).Select(p => new CSellerShipperViewModel() { fee = p.Shipper.Fee, shipper = p.Shipper.ShipperName, id = p.ShipperId }).ToList(),
                cartcount = dbcontext.OrderDetails.Where(o => o.Order.StatusId == 1 && o.Order.MemberId == memid && o.ProductDetail.Product.MemberId == dbcontext.Products.Where(r=>r.ProductId == id).FirstOrDefault().MemberId).ToList().Count,
            };

            if (q.productPics.Count == 0)
            {
                string pName = "/img/imageNotFound.png";
                string path = _enviro.WebRootPath + pName;
                q.productPics.Add(System.IO.File.ReadAllBytes(path));
            }
            foreach (var item in q.comments)
            {
                if (item.memPic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _enviro.WebRootPath + pName;
                    item.memPic = System.IO.File.ReadAllBytes(path);
                }
            }
            if (q.seller.MemPic == null)
            {
                string pName = "/img/Member/nopicmem.jpg";
                string path = _enviro.WebRootPath + pName;
                q.seller.MemPic = System.IO.File.ReadAllBytes(path);
            }
            return Json(q);
        }
        //od = dbcontext.Orders.Where(o => o.MemberId == memid && o.StatusId == 1).FirstOrDefault();
        public IActionResult AddCart(int memid, int id,int qty)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            int orderid = 0;
            if(!dbcontext.Orders.Where(o=>o.MemberId == memid && o.StatusId == 1 && o.OrderDetails.Where(d=>d.ProductDetail.Product.Member.MemberId == dbcontext.ProductDetails.Where(p=>p.ProductDetailId == id).Select(p=>p.Product.MemberId).FirstOrDefault()).Any()).Any())
            {
                Order od = new Order() { CouponId = 1,MemberId=memid, StatusId=1,PaymentId=1,ShipperId=1, };
                dbcontext.Orders.Add(od);
                dbcontext.SaveChanges();
                orderid = od.OrderId;
            }
            else
            {
                orderid = dbcontext.Orders.Where(o => o.MemberId == memid && o.StatusId == 1).FirstOrDefault().OrderId;
            }


            if(dbcontext.OrderDetails.Where(o=>o.OrderId == orderid && o.ProductDetailId == id && o.Order.StatusId == 1).Any())
            {
                dbcontext.OrderDetails.Where(o => o.OrderId == orderid && o.ProductDetailId == id && o.Order.StatusId == 1).FirstOrDefault().Quantity += qty;
                dbcontext.SaveChanges();
            }
            else
            {
                OrderDetail orderDetail = new OrderDetail() { OrderId = orderid, ProductDetailId = id, Quantity = qty, ShippingStatusId = 1, Unitprice = dbcontext.ProductDetails.Where(p => p.ProductDetailId == id).FirstOrDefault().UnitPrice };
                dbcontext.OrderDetails.Add(orderDetail);
                dbcontext.SaveChanges();
                
            }
            return Json(dbcontext.OrderDetails.Where(o=>o.OrderId == orderid).ToList().Count);
        }

        public IActionResult ShowCart(int bid, int sid)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var q = dbcontext.Orders.Where(o => o.MemberId == bid && o.OrderDetails.FirstOrDefault().ProductDetail.Product.MemberId == sid && o.StatusId == 1).FirstOrDefault();
            var q2 = dbcontext.OrderDetails.Where(o => o.OrderId == q.OrderId).Select(p => new
            {
                pic = IsNullPic(p.ProductDetail.Product.ProductPics.FirstOrDefault()),

            });
            return Json(new { });
        }

        byte[] IsNullPic(ProductPic pic)
        {
            if(pic == null)
            {
                string pName = "/img/imageNotFound.png";
                string path = _enviro.WebRootPath + pName;
                return System.IO.File.ReadAllBytes(path);
            }
            else
            {
                return pic.Pic;
            }
        }
    }
}
