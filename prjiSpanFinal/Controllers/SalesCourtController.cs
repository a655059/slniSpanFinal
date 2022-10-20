using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.SalesCourt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SalesCourtController : Controller
    {
        iSpanProjectContext dbContext = new iSpanProjectContext();
        public static List<C關於我ViewModel> abme = new List<C關於我ViewModel>();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult 賣場(int id)
        {

            //return View();


            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == 2).FirstOrDefault();
            var Product = dbContext.Products.Where(a => a.MemberId == Seller.MemberId).ToList();
            var CourtCategory = dbContext.CustomizedCategories.Where(a => a.MemberId == Seller.MemberId).ToList();
            var MemPic = Seller.MemPic;

            var SellerNickName = Seller.NickName;
            var CourtDescription = Seller.Description;

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




                //foreach(var star in ss)
                //{
                //    star.CommentStar
                //}

                Card賣場ViewModel outCardContent = new Card賣場ViewModel
                {
                    ProductName = pd.ProductName,
                    ProductPrice = pddt.UnitPrice.ToString(),

                    //星星數演算法

                    //var star_oddt = dbContext.OrderDetails.Where()
                    //AddedTime = pd.EditTime,
                    //StarCount = star_count

                };
                CardContent.Add(outCardContent);
            }


            //把對應到同一個商品的評價星星加總
            var ss = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == 2)
                    .Select(a => (double)a.CommentStar).Sum();
            //這邊是要計算對應到的星星數量
            var ss1 = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == 2)
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

            //return View(Seller);
            return View(showsalescourt);
        }




        public IActionResult 評價()
            {//針對某特定賣家評價
            var Comment = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == 2).ToList();
            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == 2).FirstOrDefault();
            var BestComment = Comment.Where(a => a.CommentStar == 5).ToList();
            var MediumComment = Comment.Where(a => (a.CommentStar == 3 || a.CommentStar == 4)).ToList();
            var WorstComment = Comment.Where(a => (a.CommentStar == 1 || a.CommentStar == 2)).ToList();
            //計算有購買的買家數量  
            var ordtail = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == 2)
                .Select(a => a.Order.MemberId).Distinct();

            //order shipping date- receive date

            //平均出貨天數
            //var avgShippingDate1 = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == 2).Select(a => a).FirstOrDefault();
            ///*.Select(a => ((int)a.Order.ShippingDate - (int)a.Order.OrderDatetime)).Sum()*/

            ////int a = avgShippingDate1.Order.ShippingDate - avgShippingDate1.Order.OrderDatetime;

            //System.TimeSpan ts = avgShippingDate1.Order.ShippingDate - avgShippingDate1.Order.OrderDatetime;
            //int days = ts.Days;


            var Follow = dbContext.Follows.Where(a => a.MemberId == 2);



            List<string> bstcmt = new List<string>();
            List<string> mdncmt = new List<string>();
            List<string> wrtcmt = new List<string>();

            foreach (var cmt in BestComment)
            {
                bstcmt.Add(cmt.Comment1);
            }
            foreach(var cmt in MediumComment)
            {
                mdncmt.Add(cmt.Comment1);
            }
            foreach (var cmt in WorstComment)
            {
                wrtcmt.Add(cmt.Comment1);
            }

            var ss = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.Product.MemberId == 2)
                   .Select(a => (double)a.CommentStar).Sum();
            //這邊是要計算對應到的星星數量
            var ss1 = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.Product.MemberId == 2)
                .Select(a => (double)a.CommentStar);

            double star_count = ss / ss1.Count();

            List<Card評價內容ViewModel> ct = new List<Card評價內容ViewModel>();
            foreach (var comm in Comment) {
                var byname = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == 2).ToList();
                Card評價內容ViewModel Content = new Card評價內容ViewModel
                {

                    //CommentBuyer = 
                    //抓評價的買家姓名
                    //CommentBuyer = comm.OrderDetail.Order.Member.MemberAcc,
                    //ProductName = comm.OrderDetail.ProductDetail.Product.ProductName,
                    CommentContent = comm.Comment1
                };

                ct.Add(Content);
            }

            C評價ViewModel outcomment = new C評價ViewModel
            {
                StarCount = star_count,
                BestComments = bstcmt,
                MediumComments = mdncmt,
                WorstComments = wrtcmt,
                SellerPhoto = Seller.MemPic,
                //卡在計算日期差距
                //AvgShippingDate = days,
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
            return View();
        }

        public IActionResult 關於我(int id)
        {
            //var PayingMethod = dbContext.PaymentToSellers.Where(a => a.Member.MemberId == id).ToList();
            //var Delivery = dbContext.ShipperToSellers.Where(a => a.Member.MemberId == id).ToList();

            return View(abme);
        }

        public IActionResult 新增關於我()
        {
            //這是list
            var Payment = dbContext.Payments.Select(a => a.PaymentName).ToList();
            var Delivery = dbContext.Shippers.Select(a => a.ShipperName).ToList();
            var DeliveryFee = dbContext.Shippers.Select(a => a.Fee).ToList();
            //服務時間   想說在show的時候寫在畫面上
            //
            List<bool> uncheck = new List<bool>();
            foreach(var a in Payment)
            {
                uncheck.Add(false);
            }

            //這邊要抓關於我的選項方式方式
            var NewMe = new C關於我ViewModel()
            {
                PayingMethod = Payment,
                PayingMethodCheck = uncheck,
                DeliveryMethod = Delivery,
                DeliveryFee = DeliveryFee
            };


            return View(NewMe);
        }

        [HttpPost]
        public IActionResult 新增關於我(C關於我ViewModel me)
        {
            //for (int i = 0; i < me.DeliveryMethod.Count; i++)
            //{
            //    string AAA;

            //    AAA = me.DeliveryMethod[i];


            //}

            //foreach(var item in me.DeliveryMethod)
            //{
            //    string aaa;
            //    foreach(var inp in item)
            //    {
            //        aaa = inp.ToString();
            //    }
            //}


            //foreach (var item in me.DeliveryMethod)
            //{
            //    string AAA = item;
            //}
            //me.PayingMethod
            //var P

            var NewMe = new C關於我ViewModel()
            {
                PayingMethod = me.PayingMethod,
                DeliveryMethod = me.DeliveryMethod,
                SalesCourtServiceTime = me.SalesCourtServiceTime,
                NewProductOnLoad = me.NewProductOnLoad,
                NewProductCategory = me.NewProductCategory,
                SellerCategory = me.SellerCategory,
                ServiceAfterBuy = me.ServiceAfterBuy,
                Caution = me.Caution
            };
            abme.Add(NewMe);
            //return View();

            return RedirectToAction("關於我");
        }




        public IActionResult 修改關於我(int id)
        {
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
