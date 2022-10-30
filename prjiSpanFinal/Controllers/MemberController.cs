using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewComponents;
using prjiSpanFinal.ViewModels.Member;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using prjiSpanFinal.Models.OrderReq2;
using Org.BouncyCastle.Utilities;
using prjiSpanFinal.Models.LikeReq;
using prjiSpanFinal.ViewModels.Home;
using prjiSpanFinal.Models.CategoryItemSort;
using prjiSpanFinal.ViewModels.Category;
using prjiSpanFinal.ViewModels.Header;

namespace prjiSpanFinal.Controllers
{
    public class MemberController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly iSpanProjectContext _context;
        
        public MemberController(iSpanProjectContext context, IWebHostEnvironment host) 
        {
            _host = host;
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }
        //public IActionResult productShow()
        //{
        //    return PartialView("cshtml");
        //}
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginCheck(string txtAccount, string txtPW)
        {
            var mem = _context.MemberAccounts.FirstOrDefault(m => m.MemberAcc == txtAccount);
            if (mem != null)
            {
                if (mem.MemberPw == txtPW && txtAccount == "admin")
                {
                    string jsonUser = JsonSerializer.Serialize(mem);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, jsonUser);
                    return Content("2", "text/plain", Encoding.UTF8);
                }
                else if (mem.MemberPw == txtPW)
                {
                    string jsonUser = JsonSerializer.Serialize(mem);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, jsonUser);
                    return Content("1", "text/plain", Encoding.UTF8);
                }
            }
            return Content("0", "text/plain", Encoding.UTF8); ;
        }
        public IActionResult LoginSuccess()
        {
            return View();
        }


        public IActionResult Edit()
        {
            iSpanProjectContext db = new iSpanProjectContext();
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");   //如果沒有登入則要求登入
            }
            else
            {
                string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
                int memID = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件
                var mem = db.MemberAccounts.FirstOrDefault(m => m.MemberId==memID);
                //MemberAccViewModel qq = new MemberAccViewModel();
                //qq.memACC = mem;
                //qq.regionName = db.RegionLists.FirstOrDefault(p => p.RegionId == mem.RegionId).RegionName;
                //var countryID = db.RegionLists.FirstOrDefault(p => p.RegionId == mem.RegionId).CountryId;
                //qq.countryName = db.CountryLists.FirstOrDefault(p => p.CountryId == countryID).CountryName;
                return View(mem);
            }
        }
        [HttpPost]
        public IActionResult Edit(MemberAccViewModel mem, IFormFile File1,string countryName)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");   //如果沒有登入則要求登入
            }
            else
            {
                iSpanProjectContext db = new iSpanProjectContext();
                MemberAccount acc = db.MemberAccounts.FirstOrDefault(p => p.MemberId == mem.MemberId);
                //acc = mem.memACC;
                acc.Birthday = mem.Birthday;
                if (mem.gender == "female")
                {
                    acc.Gender = 2;
                }
                else if (mem.gender == "male")
                {
                    acc.Gender = 1;
                }
                else
                {
                    acc.Gender = 0;
                }
                if (mem.TW == "tw")
                {
                    acc.IsTw = true;
                }
                else
                {
                    acc.IsTw = false;
                }
                if (mem.File1 != null) //如果有上傳照片
                {
                    byte[] imgByte = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        File1.CopyTo(memoryStream);
                        imgByte = memoryStream.ToArray();
                    }
                    acc.MemPic = imgByte;
                }
                if (mem.regionName != null)
                {
                    int countryid = db.CountryLists.FirstOrDefault(p => p.CountryName == countryName).CountryId;
                    acc.RegionId = db.RegionLists.FirstOrDefault(p => p.RegionName == mem.regionName && p.CountryId== countryid).RegionId;
                }
                if (mem.Name == null)
                {
                    acc.Name = "";
                }
                else { acc.Name = mem.Name; }

                if(mem.NickName == null) { acc.NickName = ""; }
                else { acc.NickName = mem.NickName; }

                if(mem.Address == null) { acc.Address = ""; }
                else { acc.Address = mem.Address; }

                if (mem.BackUpEmail == null) { acc.BackUpEmail = ""; }
                else { acc.BackUpEmail = mem.BackUpEmail; }

                if (mem.Email == null) { acc.Email = ""; }
                else { acc.Email = mem.Email; }

                if (mem.Phone == null) { acc.Phone = ""; }
                else { acc.Phone = mem.Phone; }
                db.SaveChanges();
                HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);
                return RedirectToAction("Login");
                //刷新
                //acc = db.MemberAccounts.FirstOrDefault(p => p.MemberId == mem.MemberId);
                //string json = JsonSerializer.Serialize(acc);
                //HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, json); //塞新資料到session
                //return View(acc);
            }

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MemberAccViewModel mem , IFormFile File1, string countryName)
        {
            //城市country要傳遞進來才能找到正確region


            iSpanProjectContext db = new iSpanProjectContext();
            MemberAccount memberac = new MemberAccount();
            memberac = mem.memACC;//將物件memberac指向mem.memACC才可以透過viewmodel的地區string抓到id
                                  //將檔案轉成二進位
            if (mem.File1 != null) 
            {                
            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                File1.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            mem.MemPic = imgByte;
            }
            if (mem.regionName!=null)
            {
                int countryid = db.CountryLists.FirstOrDefault(p => p.CountryName == countryName).CountryId;
                memberac.RegionId = db.RegionLists.FirstOrDefault(p => p.RegionName == mem.regionName &&p.CountryId== countryid).RegionId;
            }

            if (mem.gender == "female")
            {
                memberac.Gender = 2;
            }
            else if (mem.gender == "male")
            {
                memberac.Gender = 1;
            }
            else
            {
                memberac.Gender = 0;
            }
            if (mem.TW == "tw")
            {
                memberac.IsTw = true;
            }
            else
            {
                memberac.IsTw = false;
            }
            db.MemberAccounts.Add(memberac);
            db.SaveChanges();
            //return Content("OK", "text/plain", Encoding.UTF8);
            return RedirectToAction("LoginSuccess");
        }
        public IActionResult CheckName(string name)
        {
            var exists = _context.MemberAccounts.Any(m => m.Name == name);
            return Content(exists.ToString(), "text/plain");
        }
        public IActionResult CheckAccount(string acc)
        {
            var exists = _context.MemberAccounts.Any(m => m.MemberAcc == acc);
            return Content(exists.ToString(), "text/plain");
        }
        public IActionResult CheckEmail(string Email)
        {
            var exists = _context.MemberAccounts.Any(m => m.Email == Email);
            return Content(exists.ToString(), "text/plain");
        }
        public IActionResult CheckPhone(string Phone)
        {
            var exists = _context.MemberAccounts.Any(m => m.Phone == Phone);
            return Content(exists.ToString(), "text/plain");
        }

        public IActionResult ShowPhoto(int id)
        {
            MemberAccount member = _context.MemberAccounts.Find(id);
            MemberAccount mymempic = new MemberAccount();
            if (member.MemPic != null)
            {
            byte[] content = member.MemPic;
            return File(content, "image/jpeg");
            }
            else
            {
            string pName = "/img/Member/nopicmem.jpg";
            string path = _host.WebRootPath + pName;
            byte[] content = System.IO.File.ReadAllBytes(path);
            return File(content, "image/jpg");
            }


        }

        public IActionResult Like()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");   //如果沒有登入則要求登入
            }
            else
            {
                iSpanProjectContext dbcontext = new iSpanProjectContext();
                string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;
                
                var mylike = dbcontext.Likes.Where(p => p.MemberId == memID).Select(p => p.Product).ToList();
                if (mylike != null)
                {
                var list = (new MyLikeFactory()).toShowItem(mylike);
                    var mylikecategorylist = new MyLikeCategoryIndex();
                    mylikecategorylist.MyLikeShowItem = list;
                    return View(mylikecategorylist);
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
        }

        public IActionResult deletLike(int likeID, string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");
            }
            else
            {
                iSpanProjectContext dbcontext = new iSpanProjectContext();
                string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;

                var mylikeID = dbcontext.Likes.Where(p => p.LikeId == likeID).Select(p => p.LikeId).ToList();
                dbcontext.Remove(mylikeID);
                dbcontext.SaveChanges();

                return Json(new LikeSortReq().MyLikeSortItems(filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages, memID));

            }


        }

        public IActionResult AllLike( string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;

            return Json(new LikeSortReq().MyLikeSortItems(filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages, memID));
        }
        //public IActionResult SortOrder(int BigTypeId, string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        //{

        //    return Json(new SortRequest().SortItems(BigTypeId, filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages));
        //}
        public IActionResult Coupon()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;

            List<CouponViewModel> listCvm = (new CouponFactory()).cGetCoupon(id);
            MyCouponViewModel ViewModel = new MyCouponViewModel()
            {
                ListCoupon = listCvm,
                MemberID = id
            };
            return View(ViewModel);
        }
        public IActionResult getCoupons(int filter ,int sort)
        {
            int MemID = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            return Json(((new CouponFactory()).fReturnCoupon(filter, sort, MemID)).ToArray());
        }
        public IActionResult getCouponsByCode(string code)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;

            int check = (new CouponFactory()).getCouponByCodeCheck(code, id);            
            switch (check)
            {
                case 0:
                    return Json(0);
                case 1:
                    return Json(1);
                case 2:
                    return Json(2);
                case 3:
                    return Json(3);
                default:
                    Coupon c = dbcontext.Coupons.Where(c => c.CouponCode == code).FirstOrDefault();
                    if (c != null)
                    {
                        CouponWallet cw = new CouponWallet()
                        {
                            CouponId = c.CouponId,
                            IsExpired = false,
                            MemberId = id,
                        };
                        dbcontext.CouponWallets.Add(cw);
                        dbcontext.SaveChanges();
                    }
                    return Json(999);
            }

        }
        public IActionResult Order()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            return View();
        }
        public IActionResult SortOrder(int sort, int tab, int pages, int eachpage, string keyword, DateTime startdate, DateTime enddate)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            return Json(new OrderSortReq().SortTab(sort, tab, id, pages, eachpage, keyword, startdate, enddate.AddDays(1)));
        }

        public IActionResult WriteArgue(int id, int type,int reason, string keyword, List<byte[]> pics)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            if(keyword == null)
            {
                keyword = "";
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            Argument a = new Argument() { OrderId = id,ArgumentTypeId = type, ArgumentReasonId = reason, ReasonText = keyword };
            dbcontext.Arguments.Add(a);
            dbcontext.SaveChanges();

            Order b = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
            b.StatusId = 8;

            foreach(var item in pics)
            {
                ArguePic p = new ArguePic() { ArguePic1 = item, ArguementId = a.ArgumentId };
                dbcontext.ArguePics.Add(p);
            }

            dbcontext.SaveChanges();
            return Json("1");
        }

        public IActionResult WriteReceive(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }

            iSpanProjectContext dbcontext = new iSpanProjectContext();
            Order b = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
            b.StatusId = 6;
            b.ReceiveDate = DateTime.Now;
            var q = dbcontext.OrderDetails.Where(o => o.OrderId == id);
            foreach(var item in q)
            {
                item.ShippingStatusId = 5;
            }
            dbcontext.SaveChanges();
            return Json("1");
        }

        public IActionResult FollowCenter()
        {
            return View();
        }

        public IActionResult OrderDetail(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var vm = dbcontext.Orders.Where(o => o.OrderId == id).Select(o => new OrderDetailViewModel()
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
            return View(vm);
        }
        public IActionResult forgetPw() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult forgetPw(MemberAccViewModel mem)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            MemberAccount memberac = new MemberAccount();
            memberac = mem.memACC;
            var mempass = "";
           mempass = db.MemberAccounts.FirstOrDefault(p => p.Email == mem.PhoneMail || p.Phone == mem.PhoneMail).MemberPw;
            if (!String.IsNullOrEmpty(mempass)) { 
            MimeMessage message = new MimeMessage();
            BodyBuilder builder = new BodyBuilder();
            //var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\蝦到爆.png");
            //==>這裡可以放入圖片路徑

            builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/ChangePwMail.cshtml");
            builder.HtmlBody = "您的新密碼為:" + mempass + "\n" + $"當前時間:{DateTime.Now:yyyy-MM-dd HH:mm:ss}"+"請使用新密碼重新登入";
                //=>內容

                message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
            message.To.Add(new MailboxAddress("詹勳棋", "harry80011@gmail.com"));
            message.Subject = "[C#蝦到爆商城(ShopDaoBao)]忘記密碼通知信"; //==>標題
            message.Body = builder.ToMessageBody();


            using (SmtpClient client = new SmtpClient())
            {
                client.Connect("smtp.outlook.com", 25, MailKit.Security.SecureSocketOptions.StartTls);
                //第二個參數是port
                //outlook.com smtp.outlook.com port:25
                //yahoo smtp.mail.yahoo.com.tw port:465
                //gmail smtp.gmail.com port:587
                client.Authenticate("ShopDaoBao@outlook.com", "SDB20221013");
                client.Send(message);
                client.Disconnect(true);
            }
                return RedirectToAction("Login");
            }
            
            return RedirectToAction("forgetPw");
        }
        
        public IActionResult ChangePw()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");
            }
            else
            {
                iSpanProjectContext db = new iSpanProjectContext();
                string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
                int memID = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件                
                string dbPW = db.MemberAccounts.FirstOrDefault(m => m.MemberId == memID).MemberPw;
                if (dbPW!=null)
                {
                    return View();
                }
                return RedirectToAction("Index","home");
            }

        }
        //目前密碼
        public IActionResult CheckNowPW(string txtPW)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                iSpanProjectContext db = new iSpanProjectContext();
                string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
                int memID = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件                
                string dbPW = db.MemberAccounts.FirstOrDefault(m => m.MemberId == memID).MemberPw;
                if (dbPW == txtPW)
                {
                    return Content("True", "text/plain", Encoding.UTF8);
                }
                return Content("False", "text/plain", Encoding.UTF8);
            }

        }
        //新密碼
        public IActionResult ChangeNewPW(string txtNewPW)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");
            }
            else
            {
                if (txtNewPW != null)
                {
                    iSpanProjectContext db = new iSpanProjectContext();
                    string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                    int memID = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId;
                    var mem = db.MemberAccounts.FirstOrDefault(m => m.MemberId == memID);
                    mem.MemberPw = txtNewPW;
                    db.SaveChanges();
                    return Content("OK", "text/plain", Encoding.UTF8);
                }
                return Content("NO", "text/plain", Encoding.UTF8);
            }

        }


        public IActionResult City()
        {
            var cities = _context.CountryLists.Select(a => a.CountryName).Distinct();
            return Json(cities);
        }
        public IActionResult getCityID(string city)
        {
            var sites = _context.CountryLists.Where(a => a.CountryName == city).Select(a => a.CountryId).Distinct();
            return Json(sites);
        }
        public IActionResult Site(int site)
        {
            var sites = _context.RegionLists.Where(a => a.CountryId == site).Select(a => a.RegionName).Distinct();
            return Json(sites);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(CDictionary.SK_LOGINED_USER);
            return RedirectToAction("Index", "Home");
        }
    }
}
