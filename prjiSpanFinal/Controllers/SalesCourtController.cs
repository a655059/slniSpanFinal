using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.SalesCourt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjiSpanFinal.ViewModels.Member;
using prjiSpanFinal.ViewModels;
//using System.Data.Object.EntityFunctions;

namespace prjiSpanFinal.Controllers
{
    public class SalesCourtController : Controller
    {
        iSpanProjectContext dbContext = new iSpanProjectContext();
        public static List<C關於我ViewModel> abme = new List<C關於我ViewModel>();
        public int id;

        public IActionResult Index()
        {
            return View();
        }

        public void getid() {
            id = 2; //備用帳號
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                id = (JsonSerializer.Deserialize<MemberAccount>(json)).MemberId;
            }
        }

        public IActionResult 賣場()
        {

            getid();
            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            var Product = dbContext.Products.Where(a => a.MemberId == Seller.MemberId).ToList();
            var CourtCategory = dbContext.CustomizedCategories.Where(a => a.MemberId == Seller.MemberId).ToList();
            var MemPic = Seller.MemPic;

            var SellerNickName = dbContext.MemberAccounts.Where(a => a.MemberId == id).Select(a => a.NickName).FirstOrDefault();
            var CourtDescription = dbContext.MemberAccounts.Where(a => a.MemberId == id).Select(a => a.Description).FirstOrDefault();

            List<string> CategoryName = new List<string>();

            foreach (var catname in CourtCategory)
            {
                CategoryName.Add(catname.CustomizedCategoryName);
            }




            List<Card賣場ViewModel> CardContent = new List<Card賣場ViewModel>();
            foreach (var pd in Product)
            {
                var pddt = dbContext.ProductDetails.Where(a => a.ProductId == pd.ProductId).FirstOrDefault();
                var oddt = dbContext.OrderDetails.Where(a => a.ProductDetailId == pddt.ProductDetailId).Select(a => a.Comments.Select(c => c.CommentStar)).ToList();
                var soldct = dbContext.ProductDetails.Where(a => a.Product.ProductId == pd.ProductId).ToList();
                var uptime = dbContext.Products.FirstOrDefault(a => a.ProductId == pd.ProductId).EditTime;
                var starctsum = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == pd.ProductId).Select(a => (double)a.CommentStar).Sum();
                var starct = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == pd.ProductId).Select(a => (double)a.CommentStar);
                var staravg = starctsum / starct.Count();
                //var prodpic = dbContext.Products.FirstOrDefault(a => a.ProductId == pd.ProductId).ProductPics;

                var prodpic = dbContext.ProductPics.Where(a => a.ProductId == pd.ProductId).FirstOrDefault().Pic; 
           

                Card賣場ViewModel outCardContent = new Card賣場ViewModel
                {
                    ProductName = pd.ProductName,
                    ProductPrice = pddt.UnitPrice.ToString(),
                    SoldQuantity = soldct.Count(),
                    AddedTime = uptime,
                    StarCount = staravg,
                    Producpic =  prodpic,
                    //星星數演算法

                    //var star_oddt = dbContext.OrderDetails.Where()
                    //AddedTime = pd.EditTime,
                    //StarCount = star_count

                };
                CardContent.Add(outCardContent);
            }


            //把對應到同一個商品的評價星星加總
            var ss = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == id)
                    .Select(a => (double)a.CommentStar).Sum();
            //這邊是要計算對應到的星星數量
            var ss1 = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == id)
                .Select(a => (double)a.CommentStar);


            double star_count = ss / ss1.Count();

            C賣場ViewModel showsalescourt = new C賣場ViewModel
            {
                SellerName = SellerNickName,
                CourtCategoryName = CategoryName,
                CourtDescription = CourtDescription,
                CardProduct = CardContent,
                star = star_count,
                Picture = MemPic
            };

            
            return View(showsalescourt);
        }




        public IActionResult 評價()
        {//針對某特定賣家評價

            getid();

            var Comment = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id);
            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            //計算有購買的買家數量  
            var ordtail = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id)
                .Select(a => a.Order.MemberId).Distinct();

            //order shipping date- receive date

            //平均出貨天數
            var a = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id).Select(a => a.Order.ShippingDate).ToList();
            var b = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id).Select(a => a.Order.OrderDatetime).ToList();

            List<double> DateSum = new List<double>();
            double Sum = 0;

            for (int i = 0; i < a.Count; i++)
            {
                TimeSpan TS = new TimeSpan(a[i].Ticks - b[i].Ticks);
                double diff = Convert.ToDouble(TS.TotalDays);
                Sum += diff;
            }



            //string timee = tim.ToString();

            var Follow = dbContext.Follows.Where(a => a.MemberId == id);
            var weekBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now-a.CommentTime).Days <= 7);
            var monthBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now - a.CommentTime).Days <= 31 && (DateTime.Now - a.CommentTime).Days > 7);
            var halfmonthBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now - a.CommentTime).Days < 186 && (DateTime.Now - a.CommentTime).Days > 31);
            var allBestComment = Comment.ToList().Where(a => a.CommentStar == 5);

            var weekMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days <= 7);
            var monthMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days <= 31 && (DateTime.Now - a.CommentTime).Days > 7);
            var halfmonthMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days < 186 && (DateTime.Now - a.CommentTime).Days > 31);
            var allMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4));


            var weekWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days <= 7);
            var monthWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days <= 31 && (DateTime.Now - a.CommentTime).Days > 7);
            var halfmonthWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days < 186 && (DateTime.Now - a.CommentTime).Days > 31);
            var allWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2));


            #region
            BestCommentCount bstcmt = new BestCommentCount();
            MediumCommentCount mdncmt = new MediumCommentCount();
            WorstCommentCount wrtcmt = new WorstCommentCount();

           
            bstcmt.WeekCount = weekBestComment.Count();
            bstcmt.MonthCount = monthBestComment.Count();
            bstcmt.HalfYearCount = halfmonthBestComment.Count();
            bstcmt.AllCount = allBestComment.Count();

            mdncmt.WeekCount = weekMediumComment.Count();
            mdncmt.MonthCount = monthMediumComment.Count();
            mdncmt.HalfYearCount = halfmonthMediumComment.Count();
            mdncmt.AllCount = allMediumComment.Count();

            wrtcmt.WeekCount = weekWorstComment.Count();
            wrtcmt.MonthCount = monthWorstComment.Count();
            wrtcmt.HalfYearCount = halfmonthWorstComment.Count();
            wrtcmt.AllCount = allWorstComment.Count();
            #endregion

            var ss = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.Product.MemberId == id)
                   .Select(a => (double)a.CommentStar).Sum();
            //這邊是要計算對應到的星星數量
            var ss1 = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.Product.MemberId == id)
                .Select(a => (double)a.CommentStar);

            double star_count = ss / ss1.Count();

            var CmtBName = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id)
                .Select(a => a.OrderDetail.Order.Member.MemberAcc).ToList();
            var CmtPdName = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id)
                .Select(a => a.OrderDetail.ProductDetail.Product.ProductName).ToList();
            var Cmt = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id).Select(a => a.Comment1).ToList();

            List<Card評價內容ViewModel> ct = new List<Card評價內容ViewModel>();
            for (int i = 0; i < CmtBName.Count(); i++)
            {

                Card評價內容ViewModel Content = new Card評價內容ViewModel
                {
                    CommentBuyer = CmtBName[i],
                    ProductName = CmtPdName[i],
                    CommentContent = Cmt[i]
                };

                ct.Add(Content);
            }

            C評價ViewModel outcomment = new C評價ViewModel
            {
                StarCount = star_count,
                //BestCommentsCount = BestComment.Count(),
                //MediumComments = mdncmt,
                //WorstComments = wrtcmt,
                BestCommentCountMethod = bstcmt,
                MediumCommentCountMethod = mdncmt,
                WorstCommentCountMethod = wrtcmt,

                SellerPhoto = Seller.MemPic,
                //卡在計算日期差距
                AvgShippingDate = Sum / a.Count(),
                BuyerCount = ordtail.Count(),
                AddLoveCount = Follow.Count(),
                //刪資料  等資料表
                //AbandonOrder  = ,
                //評論週期 1週  1個月 半年
                //CommentPeriod = ,

                CardContent = ct
            };

            return View(outcomment);
        }

        public IActionResult 設定分類()
        {
            getid();

            CustomizedCategory csct = new CustomizedCategory {
                MemberId = id
            };
            return View(csct);
        }

        [HttpPost]
        public IActionResult 設定分類(int Memberid, string[] CustomizedCategoryName)
        {
            //CustomizedCategory newcust = new CustomizedCategory();
            //newcust.CustomizedCategoryId = custcate.CustomizedCategoryId;

            //custmizcatery.CustomizedCategory

            foreach(var ccn in CustomizedCategoryName)
            {
                CustomizedCategory newcust = new CustomizedCategory();
                newcust.MemberId = Memberid;
                newcust.CustomizedCategoryName = ccn;
                newcust.SortNumber = 0;

                dbContext.CustomizedCategories.Add(newcust);
            }

            dbContext.SaveChanges();

            return RedirectToAction("賣場");
        }

        public IActionResult 關於我()
        {
            getid();
            MemberAccount x = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();

            //if (x != null) return RedirectToAction("修改關於我");

            C關於我ViewModel outp = new C關於我ViewModel
            {
                Memberid = x.MemberId
            };

            return View(outp);
        }

        public IActionResult 新增關於我()
        {
            getid();
            MemberAccount x = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();

            //if (x != null) return RedirectToAction("修改關於我");

           C關於我ViewModel outp = new C關於我ViewModel {
               Memberid = x.MemberId
            };

            return View(outp);
        }

        [HttpPost]
        public IActionResult 新增關於我(C關於我ViewModel me,string[] ServiceTime)
        {
            var add = dbContext.MemberAccounts.FirstOrDefault(a => a.MemberId == me.Memberid);
           
            var NewMe = new C關於我ViewModel()
            {
                SalesCourtServiceTime = me.SalesCourtServiceTime,
                NewProductOnLoad = me.NewProductOnLoad,
                SellerCategory = me.SellerCategory,
                ServiceAfterBuy = me.ServiceAfterBuy,
                Caution = me.Caution
            };

            if(ServiceTime[0] == "on")
            {
                add.ServiceTime += "0";
                add.ServiceTime += ",";
                add.ServiceTime += me.SalesCourtServiceTime[0];
                add.ServiceTime += ",";
                add.ServiceTime += me.SalesCourtServiceTime[1];
                add.ServiceTime += "/";
            }
            if(ServiceTime[1] == "on")
            {
                add.ServiceTime += "1";
                add.ServiceTime += ",";
                add.ServiceTime += me.SalesCourtServiceTime[2];
                add.ServiceTime += ",";
                add.ServiceTime += me.SalesCourtServiceTime[3];
                add.ServiceTime += "/";
            }
            if(ServiceTime[2] == "on")
            {
                add.ServiceTime += "2";
                add.ServiceTime += ",";
                add.ServiceTime += ServiceTime[3];
                add.ServiceTime += "/";
            }

            add.RenewProduct = NewMe.NewProductOnLoad;
            add.SellerType = NewMe.SellerCategory;
            add.AfterSales = NewMe.ServiceAfterBuy;
            add.SellerCaution = NewMe.Caution;

            //dbContext.MemberAccounts.Add(add);
            //return View();
            dbContext.SaveChanges();
            return RedirectToAction("關於我");
            //return Content("Success", "text/plain");
        }


        public IActionResult 修改關於我()
        {
            getid();


            return View();
        }

        [HttpPost]
        public IActionResult 修改關於我(C關於我ViewModel me)
        {
            if (me != null)
            {

                return RedirectToAction("關於我");
            }

            return RedirectToAction("關於我");

        }
    }
}
