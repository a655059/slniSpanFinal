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
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using prjiSpanFinal.ViewModels.Delivery;
using System.Collections.Specialized;
using System.Web;
using System.Security.Cryptography;
using MimeKit.Utils;

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
        //將有點連結信的會員變成正式會員
        public IActionResult MemstChange()
        {
            return View();
        }
        //超過30分鐘後失效頁面
        public IActionResult MemstChangeFalse(int? memberID)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_MAILCHECK); //拿出寄信的session
            if (!String.IsNullOrWhiteSpace(jsonstring))
            {
                if (memberID != null)
                {
                    var q = db.MemberAccounts.FirstOrDefault(p => p.MemberId == memberID);
                    q.MemStatusId = 2;
                    db.SaveChanges();
                    return Content("1", "text/plain", Encoding.UTF8);
                }
                else
                {
                    return Content("2", "text/plain", Encoding.UTF8);
                }
            }
            else {
                return Content("2", "text/plain", Encoding.UTF8);
            }

        }
        //public IActionResult productShow()
        //{
        //    return PartialView("cshtml");
        //}
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginCheck(string txtAccount, string txtPW, string txtValidation)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGIN_VALIDATION))
            {
                return RedirectToAction("Login");
            }
            if (txtValidation != HttpContext.Session.GetString(CDictionary.SK_LOGIN_VALIDATION) && txtValidation != null) //暫時讓他空白不然很麻煩
            {
                return Content("11", "text/plain", Encoding.UTF8);
            }

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
            return Content("0", "text/plain", Encoding.UTF8);
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
                if (mem.IsAcceptAd==true) {
                    #region
                    ////如果訂閱則寄信
                    //MimeMessage message = new MimeMessage();
                    //BodyBuilder builder = new BodyBuilder();
                    ////var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\蝦到爆.png");
                    ////==>這裡可以放入圖片路徑

                    //builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/Subscription.cshtml");
                    ////=>內容
                    //message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
                    //message.To.Add(new MailboxAddress(mem.Name,mem.Email));
                    //message.Subject = "[C#蝦到爆商城(ShopDaoBao)]最爆的商品都在這"; //==>標題
                    //message.Body = builder.ToMessageBody();
                    //using (SmtpClient client = new SmtpClient())
                    //{
                    //    client.Connect("smtp.outlook.com", 25, MailKit.Security.SecureSocketOptions.StartTls);
                    //    client.Authenticate("ShopDaoBao@outlook.com", "SDB20221013");
                    //    client.Send(message);
                    //    client.Disconnect(true);
                    //}
                    #endregion
                    acc.IsAcceptAd = true;
                }
                else {
                    acc.IsAcceptAd = false;
                }
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
        public IActionResult SendNewspaper(IFormFile newsimgphoto)
        {
            //後台新增發送電子報，另做一個view裡面可以輸入電子報要附的圖片
            try
            {
                iSpanProjectContext db = new iSpanProjectContext();
                var memName = db.MemberAccounts.Where(p => p.IsAcceptAd == true).Select(p=>p.Name);
                var memEmail= db.MemberAccounts.Where(p => p.IsAcceptAd == true).Select(p=>p.Email);
                MimeMessage message = new MimeMessage();
                BodyBuilder builder = new BodyBuilder();
                string picture = _host.ContentRootPath;
                //var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\活動.png");
                var image = builder.LinkedResources.Add(picture + @"wwwroot\img\活動.png");

                //==>這裡可以放入圖片路徑

                string urll = $"{Request.Scheme}://{Request.Host}";
                //builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/Subscription.cshtml");

                builder.HtmlBody = $"<h1>瘋蝦Shoppping:雙11重量出擊</h1><h3>限定1111加碼超值優惠趁早!</h3><br/>" +$"<a href='{urll}'>請點選以下連結進入賣場</a><br/>" +
                                   $"請注意，由於部分信箱可能有收不到站方通知信件的情況，所以也請您不吝多留意「垃圾郵件夾」。<br/>" +
                                   $"※此封郵件為系統自動發送，請勿直接回覆此郵件。 <br/>Regards,<br/>ShopDaoBao(蝦到爆) Customer Service";
                //=>內容


                message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
                //foreach (var item in memEmail)
                //{
                //        message.To.Add(new MailboxAddress("會員",item));
                //}
                message.To.Add(new MailboxAddress("會員", "Wang20221101@gmail.com"));
                message.Subject = "[C#蝦到爆商城(ShopDaoBao)]最爆都在這"; //==>標題
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
                return Content("OK", "text/plain", Encoding.UTF8);
            }
            catch
            {
                return Content("Err", "text/plain", Encoding.UTF8);
            }

        }
        public IActionResult Newspaper()
        {
            return View();
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

            //寄信功能(驗證連結設定失效/找到點連結的會員並修改MemStatusId=2存進資料庫，並將頁面導向登入)
            if (String.IsNullOrWhiteSpace(memberac.Name))
            {
                memberac.Name = "會員";
            }
            try
            {
                MimeMessage message = new MimeMessage();
                BodyBuilder builder = new BodyBuilder();
                //var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\蝦到爆.png");
                //==>這裡可以放入圖片路徑
                var memID = db.MemberAccounts.FirstOrDefault(p => p.MemberAcc == memberac.MemberAcc).MemberId;
                string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
                StringBuilder sb = new StringBuilder();
                Random rand = new Random();
                int index;
                for (int i = 0; i < 5; i++)
                {
                    index = rand.Next(0, s.Length);
                    sb.Append(s[index]);
                }
                HttpContext.Session.SetString(CDictionary.SK_MAILCHECK, sb.ToString());
                string urll = $"{Request.Scheme}://{Request.Host}/Member/MemstChange/id?key={memID}";
                //builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/ChangePwMail.cshtml");
                builder.HtmlBody = $"<p>尊敬的會員您好：此封為驗證信。請於20分鐘內驗證完成。<br/><a href='{urll}'>請點選以下連結</a>。如果未有成為正式會員的需求，請忽略此信件。</p><br/>" +
                                   $"請注意，由於部分信箱可能有收不到站方通知信件的情況，所以也請您不吝多留意「垃圾郵件夾」。<br/>" +
                                   $"※此封郵件為系統自動發送，請勿直接回覆此郵件。 <br/>Regards,<br/>ShopDaoBao(蝦到爆) Customer Service";

                //=>內容

                message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
                message.To.Add(new MailboxAddress(memberac.Name, memberac.Email));
                message.Subject = "[C#蝦到爆商城(ShopDaoBao)]正式會員驗證信"; //==>標題
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

                //return Content("OK", "text/plain", Encoding.UTF8);
                return RedirectToAction("LoginSuccess");

            }
            catch {
                return RedirectToAction("EmailFalse"); ;
            }

        }
        public IActionResult EmailFalse()
        {
            return View();
        }
        //訂閱電子報
        public IActionResult Subscription()
        {
            return View();
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

        public IActionResult deletLike(int[] PdID, string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
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
                foreach(var item in PdID)
                {
                var DeltlikeID = dbcontext.Likes.Where(p => p.ProductId == item).Select(p => p).FirstOrDefault() ;
                dbcontext.Likes.Remove(DeltlikeID);
                
                }
                //var mylikeID = dbcontext.Likes.Where(p => p.LikeId == likeID).Select(p => p.LikeId).ToList();
                dbcontext.SaveChanges();

                return Json(new LikeSortReq().MyLikeSortItems(filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages, memID));

            }


        }
        public IActionResult creat50Like() 
        {
            iSpanProjectContext db = new iSpanProjectContext();

            string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;
            Product pro = new Product();
            if (memID!=0)
            {
                var pdID = db.Products.Where(p=>p.ProductStatusId==0).Take(50);
                foreach(var item in pdID)
                {
                    Like like = new Like();
                    like.MemberId = memID;
                    like.ProductId = item.ProductId;
                    db.Likes.Add(like);
                    //db.SaveChanges();
                }
                db.SaveChanges();
                return Content("OK", "text/plain", Encoding.UTF8);
            }
            else {
                return Content("Err", "text/plain", Encoding.UTF8);
            }
        }

        public IActionResult AllLike( string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;
            return Json(new LikeSortReq().MyLikeSortItems(filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages, memID));
        }
        public IActionResult SearchLike(string[] filter, int priceMin, int priceMax, int SortOrder,string keyword)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;
            return Json(new LikeSortReq().LikeSearchItem(filter.Select(o => Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, memID, keyword));
        }
        //按讚換頁
        public IActionResult Page(int priceMin, int priceMax, int SortOrder, string keyword,int pages, int eachpage, int[] PdID)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            string jsonsting = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int memID = JsonSerializer.Deserialize<MemberAccViewModel>(jsonsting).MemberId;
            List<Product> LikeProduct = new List<Product>();
            LikeProduct = dbcontext.Likes.Where(p => p.MemberId == memID).Select(p =>p.Product).ToList();
            List<MyLikeShowItem> res = new List<MyLikeShowItem>();

            foreach (var item in LikeProduct)//將所有like LikeProduct做viewmodel需要內容的填入
            {
                decimal x = dbcontext.ProductDetails.Where(p => p.Quantity > 0 && p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                decimal y = dbcontext.ProductDetails.Where(p => p.Quantity > 0 && p.ProductId == item.ProductId).OrderByDescending(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                byte[] pic = dbcontext.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                int sales = dbcontext.OrderDetails.Where(o => o.Order.StatusId == 7 && o.ProductDetail.ProductId ==item.ProductId).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o => o);
                List<decimal> dlist = new List<decimal>();
                if (x == y)
                    dlist.Add(x);
                else
                {
                    dlist.Add(x);
                    dlist.Add(y);
                }
                MyLikeShowItem a = new MyLikeShowItem();
                //a.Like.Product.ProductId = item.ProductId;
                a.Product = item;
                a.Price = dlist;
                if (pic != null)
                    a.Pic = pic;
                a.salesVolume = sales;

                res.Add(a);//viewmodel res現在有值了(list)

            }
            #region
            //LikeProduct = dbcontext.Likes.Where(p => p.MemberId == memID).Select(p => new MyLikeShowItem() { salesVolume = sales }).ToList();
            //List<Product> ListPD = new List<Product>();
            //if (!String.IsNullOrWhiteSpace(keyword))
            //{
            //    //keyword.Trim();
            //    //string[] keys = keyword.Split(" ");
            //    foreach (var item in res)
            //    {
            //        if (item.Product.ProductName.Contains(keyword) || item.Product.Description.Contains(keyword))
            //        {
            //            ListPD.Add(item);
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var item in LikeProduct)
            //    {
            //        ListPD.Add(item);
            //    }
            //}//有關鍵字或全部的lispd
            #endregion
            if (SortOrder == 1)
            {
                res = res.ToList();
            }
            else if (SortOrder == 3) { res = res.OrderByDescending(s => s.salesVolume).ToList(); }
            else if (SortOrder == 4) { res = res.OrderByDescending(p => p.Price.Max()).ToList(); }
            else if(SortOrder== 5) { res = res.OrderBy(p => p.Price.Min()).ToList(); }
            else { res = res.ToList(); }
            #region
            //switch (SortOrder)
            //{
            //    case 1:
            //        res=res.ToList();
            //        break;
            //    case 3:
            //        //熱銷排序
            //        res = res.OrderByDescending(s => s.salesVolume).ToList();
            //        break;
            //    case 4:
            //        //價高排序
            //        //list = (new MyLikeFactory()).SearchItem(ListPD);
            //        res = res.OrderByDescending(p => p.Price.Max()).ToList();
            //        break;
            //    case 5:
            //        //價低排序
            //        //list = (new MyLikeFactory()).SearchItem(ListPD);
            //        res = res.OrderBy(p => p.Price.Min()).ToList();
            //        break;
            //    default:
            //        res = res.ToList();
            //        break;
            //}
            #endregion
            //價格排序
            List<MyLikeShowItem> list1;
            List<MyLikeShowItem> list2;
            list1 = res.Where(p => p.Price.Count == 1).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax).ToList();
            list2 = res.Where(p => p.Price.Count == 2).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax || p.Price[1] >= priceMin && p.Price[1] <= priceMax).ToList();
            list1.AddRange(list2);
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    list1 = list1.Where(o => o.Product.ProductName.Contains(keys[i]) || o.Product.Description.Contains(keys[i])).Select(o => o).ToList();
                }
            }
            if (PdID!= null) 
            {
                foreach (var item in PdID)
                {
                    var DeltlikeID = dbcontext.Likes.Where(p => p.ProductId == item).Select(p => p).FirstOrDefault();
                    dbcontext.Likes.Remove(DeltlikeID);

                }
                //var mylikeID = dbcontext.Likes.Where(p => p.LikeId == likeID).Select(p => p.LikeId).ToList();
                dbcontext.SaveChanges();
            }
            int count = list1.Count;
            return Json(new { list1= list1.Skip((pages - 1) * eachpage).Take(eachpage).ToList(), count });
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
        //驗證碼
        private string RandomCode(int length)
        {
            string s = "0123456789zxcvbnmasdfghjklqwertyuiop";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            int index;
            for (int i = 0; i < length; i++)
            {
                index = rand.Next(0, s.Length);
                sb.Append(s[index]);
            }
            HttpContext.Session.SetString(CDictionary.SK_LOGIN_VALIDATION, sb.ToString());
            return sb.ToString();
        }
        private void PaintInterLine(Graphics g, int num, int width, int height)
        {
            Random r = new Random();
            int startX, startY, endX, endY;
            for (int i = 0; i < num; i++)
            {
                startX = r.Next(0, width);
                startY = r.Next(0, height);
                endX = r.Next(0, width);
                endY = r.Next(0, height);
                g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
            }
        }

        public IActionResult GetValidateCode()
        {
            byte[] data = null;
            string code = RandomCode(5);
            TempData["code"] = code;
            MemoryStream ms = new MemoryStream();
            using (Bitmap map = new Bitmap(100, 40))
            {
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(code, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    PaintInterLine(g, 10, map.Width, map.Height);
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            data = ms.GetBuffer();
            return File(data, "image/jpeg");
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
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            return View();
        }
        public IActionResult GetSortedFollow(int sort, int pages, int eachpage, string keyword, DateTime startdate, DateTime enddate)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            //var q = dbcontext.Follows.Where(f => f.MemberId == id).Select(f => new
            //{
            //    name = f.FollowedMem.Products.OrderByDescending(p => p.EditTime).FirstOrDefault().ProductName,
            //    acc = f.FollowedMem.MemberAcc,
            //    time = f.FollowedMem.Products.OrderByDescending(p => p.EditTime).FirstOrDefault().EditTime,
            //    id = f.FollowedMem.MemberId,
            //    pid = f.FollowedMem.Products.OrderByDescending(p => p.EditTime).FirstOrDefault().ProductId,
            //    pic = f.FollowedMem.MemPic,
            //    count = 0
            //}).ToList();
            var q = dbcontext.Follows.Where(f => f.MemberId == id).Select(f => new FollowViewModel()
            {
                acc = f.FollowedMem.MemberAcc,
                id = f.FollowedMem.MemberId,
                pic = f.FollowedMem.MemPic,
            }).ToList();
            foreach(var f in q)
            {
                if(dbcontext.Products.Where(p=>p.MemberId == f.id).Any())
                {
                    var a = dbcontext.Products.Where(p => p.MemberId == f.id && p.ProductStatusId == 0).OrderByDescending(p => p.EditTime).FirstOrDefault();
                    f.time = a.EditTime;
                    f.name = a.ProductName;
                    f.pid = a.ProductId;
                }
                else
                {
                    f.time = new DateTime(1999,1,1);
                    f.name = "暫無商品";
                    f.pid = 0;
                }
                if(f.pic == null)
                {
                    string pName = "/img/Member/nopicmem.jpg";
                    string path = _host.WebRootPath + pName;
                    byte[] content = System.IO.File.ReadAllBytes(path);
                    f.pic = content;
                }
            }    
            if (sort == 0)
            {
                q = q.OrderBy(o => o.acc).ToList();
            }
            else if(sort == 1)
            {
                q = q.OrderByDescending(o => o.acc).ToList();
            }
            else if(sort == 2)
            {
                q = q.OrderByDescending(o => o.time).ToList();
            }
            else if(sort == 3)
            {
                q = q.OrderBy(o => o.time).ToList();
            }
            if (keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    q = q.Where(o => o.acc.Contains(keys[i]) || o.name.Contains(keys[i])).Select(o => o).ToList();
                }
            }

            return Json(q.Skip((pages - 1) * eachpage).Take(eachpage).ToList());
        }

        public IActionResult WriteUnFollow(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int myid = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            dbcontext.Follows.Remove(dbcontext.Follows.Where(f => f.MemberId == myid && f.FollowedMemId == id).FirstOrDefault());
            dbcontext.SaveChanges();
            return Json("0");
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
            //memberac = mem.memACC;

            //var mempass = "";
            var memacc = db.MemberAccounts.FirstOrDefault(p => p.Email == mem.PhoneMail || p.Phone == mem.PhoneMail).MemberAcc;//找到忘記會員帳號
            var memEmail= db.MemberAccounts.FirstOrDefault(p => p.Email == mem.PhoneMail || p.Phone == mem.PhoneMail).Email;//找到忘記會員信箱
            var memName= db.MemberAccounts.FirstOrDefault(p => p.Email == mem.PhoneMail || p.Phone == mem.PhoneMail).Name;
            if (String.IsNullOrWhiteSpace(memName)) 
            {
                memName = "會員";
            }
            //驗證信箱或手機存不存在
            if (!String.IsNullOrEmpty(memacc))
            {
                var Newpassword = new MemberAccViewModel().PWHasH();
                MemberAccount acc = db.MemberAccounts.FirstOrDefault(p => p.MemberAcc == memacc);
                acc.MemberPw = Newpassword;
                db.SaveChanges();
                MimeMessage message = new MimeMessage();
                BodyBuilder builder = new BodyBuilder();
                //var image = builder.LinkedResources.Add(@"C:\Users\Student\source\repos\slniSpanFinal\prjiSpanFinal\wwwroot\img\蝦到爆.png");
                //==>這裡可以放入圖片路徑

                //builder.HtmlBody = System.IO.File.ReadAllText("./Views/Member/ChangePwMail.cshtml");
                builder.HtmlBody = $"<p>尊敬的會員您好：您新的密碼為{Newpassword}。<br/>請以新密碼登入並修改您的舊密碼。如果未有忘記密碼的需求，請忽略此信件。</p><br/>"+
                                   $"請注意，由於部分信箱可能有收不到站方通知信件的情況，所以也請您不吝多留意「垃圾郵件夾」。<br/>"+
                                   $"※此封郵件為系統自動發送，請勿直接回覆此郵件。 <br/>Regards,<br/>ShopDaoBao(蝦到爆) Customer Service";
                //=>內容

                message.From.Add(new MailboxAddress("蝦到爆商城", "ShopDaoBao@outlook.com"));
                message.To.Add(new MailboxAddress(memName, memEmail));
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
        public IActionResult Balance()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login");   //如果沒有登入則要求登入
            }
            iSpanProjectContext db = new iSpanProjectContext();
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            MyBalance ViewMondel = new MyBalance()
            {
                Balance = db.MemberAccounts.Where(m => m.MemberId == id).Select(m => m.Balance).FirstOrDefault(),
            };
            return View(ViewMondel);
        }
        public IActionResult BalanceCharge(string pay)
        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            int money = Convert.ToInt32(pay);

            MemberAccount user = _context.MemberAccounts.Where(m => m.MemberId == id).FirstOrDefault();
            user.Balance += money;
            BalanceRecord record = new BalanceRecord()
            {
                MemberId = id,
                Amount = money,
                Reason = "網頁儲值",
                Record = DateTime.Now,
            };
            _context.BalanceRecords.Add(record);
            _context.SaveChanges();
            return Json(user.Balance);
        }
        public IActionResult BalanceInfo(int status,int nowpages)
        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            List<BalanceRecordViewModel> res;
            res = (new BalanceFactory()).fBalanceRecordFilter( id, status, nowpages);
            
            return Json(res);
        }
        public IActionResult OPayCheckout(string[] checkoutItems)
        {
            string[] stringArr = Guid.NewGuid().ToString().Split('-');
            string tradeNo = stringArr[2] + stringArr[3] + stringArr[4];
            string tradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            string itemName = checkoutItems[0];
            int totalAmount = Convert.ToInt32(checkoutItems[1]);
            
            itemName = itemName.TrimEnd('#');
            string clientBackURL = $"{Request.Scheme}://{Request.Host}/Member/IsExistBalanceRecordSession";
            NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["HashKey"] = "5294y06JbISpM5x9";
            parameters["ChoosePayment"] = "Credit";
            parameters["ClientBackURL"] = $"{Request.Scheme}://{Request.Host}/Member/IsExistBalanceRecordSession";    //完成後跳回去的頁面
            parameters["CreditInstallment"] = "";
            parameters["EncryptType"] = "1";
            parameters["InstallmentAmount"] = "";
            parameters["ItemName"] = itemName;
            parameters["MerchantID"] = "2000132";
            parameters["MerchantTradeDate"] = tradeDate;
            parameters["MerchantTradeNo"] = tradeNo;
            parameters["PaymentType"] = "aio";
            parameters["Redeem"] = "";
            parameters["ReturnURL"] = "https://developers.opay.tw/AioMock/MerchantReturnUrl";
            parameters["StoreID"] = "";
            parameters["TotalAmount"] = totalAmount.ToString();
            parameters["TradeDesc"] = "建立信用卡測試訂單";
            parameters["HashIV"] = "v77hoKGq4kWxNNIS";
            string checkMacValue = parameters.ToString();
            checkMacValue = checkMacValue.Replace("=", "%3d").Replace("&", "%26");
            using var hash = SHA256.Create();
            var byteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(checkMacValue.ToLower()));
            checkMacValue = Convert.ToHexString(byteArray).ToUpper();


            COPayParametersViewModel cOPayParameters = new COPayParametersViewModel
            {
                tradeNO = tradeNo,
                tradeDate = tradeDate,
                totalAmount = totalAmount,
                itemName = itemName,
                clientBackURL = clientBackURL,
                checkMacValue = checkMacValue
            };
            return Json(cOPayParameters);
        }
        public IActionResult SetBalanceRecordToSession(string Balance)
        {
            HttpContext.Session.SetString(CDictionary.SK_BALANCE, Balance);
            return Content("1");
        }

        public IActionResult IsExistBalanceRecordSession()
        {

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_BALANCE))
            {
                int money = Convert.ToInt32(HttpContext.Session.GetString(CDictionary.SK_BALANCE));
                int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
                MemberAccount user = _context.MemberAccounts.Where(m => m.MemberId == id).FirstOrDefault();
                user.Balance += money;

                BalanceRecord data = new BalanceRecord()
                {
                    MemberId = id,
                    Amount = money,
                    Reason = "網頁儲值",
                    Record = DateTime.Now,
                };
                _context.BalanceRecords.Add(data);
                _context.SaveChanges();


                HttpContext.Session.Remove(CDictionary.SK_BALANCE);
                return RedirectToAction("Balance", "Member");
            }
            else
            {
                return RedirectToAction("Balance", "Member");
            }
        }

        public IActionResult TakeTradeFee(int id)
        {
            int total = _context.OrderDetails.Where(o => o.OrderId == id).Select(p => Convert.ToInt32(Math.Ceiling(Convert.ToInt32(p.Unitprice) * p.Quantity*p.Order.Coupon.Discount))).Sum(p => p);
            total += _context.Orders.Where(m => m.OrderId == id).Select(p => p.Payment.Fee + p.Shipper.Fee).FirstOrDefault();
            int tradefee = Convert.ToInt32(Math.Ceiling(total * 0.05));

            TradeFeeList fee = new TradeFeeList
            {
                Date = DateTime.Now,
                OrderId = id,
                Fee = tradefee,
                Total = total,
            };
            _context.TradeFeeLists.Add(fee);
            try 
            {
                _context.SaveChanges();
                int sellerid = _context.OrderDetails.Where(m => m.OrderId == id).Select(m => m.ProductDetail.Product.MemberId).FirstOrDefault();
                int[] res = { sellerid, total, tradefee };
                return Json(res);
            } 
            catch 
            {
                return Json("error");
            }
            
        }
        public IActionResult BalanceDemo1()
        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            for (int i = 0; i < 10; i++)
            {
                BalanceRecord brA = new BalanceRecord
                {
                    MemberId = id,
                    Amount = 1000,
                    Reason = "Demo加錢",
                    Record = DateTime.Now,
                };
                BalanceRecord brM = new BalanceRecord
                {
                    MemberId = id,
                    Amount = -1000,
                    Reason = "Demo扣錢",
                    Record = DateTime.Now,
                };
                _context.BalanceRecords.Add(brA);
                _context.BalanceRecords.Add(brM);
            }
            MemberAccount acc = _context.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            acc.Balance += 10000;
            _context.SaveChanges();
            return Json(0);
        }
    }
}
