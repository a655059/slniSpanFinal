
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
using prjiSpanFinal.Models.CategoryItemSort;
//using System.Data.Object.EntityFunctions;

namespace prjiSpanFinal.Controllers
{
    public class SalesCourtController : Controller
    {
        iSpanProjectContext dbContext = new iSpanProjectContext();
        public static List<C關於我ViewModel> abme = new List<C關於我ViewModel>();
        List<Product> listprod;

        public int id;

        public IActionResult Index()
        {
            return View();
        }


        public SalesCourtController()
        {
            listprod = new List<Product>();

        }

        public int getid()
        {
            id = 2; //備用帳號
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                id = (JsonSerializer.Deserialize<MemberAccount>(json)).MemberId;
            }

            return id;
        }


        public IActionResult 賣場(int id)
        {
            int memid = getid();

            if(id == 0)     id = getid();
            //getid();
            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            var Product = dbContext.Products.Where(a => a.MemberId == Seller.MemberId).ToList();
            var CourtCategory = dbContext.CustomizedCategories.Where(a => a.MemberId == Seller.MemberId).ToList();
            var MemPic = Seller.MemPic;

            var SellerNickName = dbContext.MemberAccounts.Where(a => a.MemberId == id).Select(a => a.MemberAcc).FirstOrDefault();
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
                //var soldct = dbContext.ProductDetails.Where(a => a.Product.ProductId == pd.ProductId).ToList();
                int sales = dbContext.OrderDetails.Where(o => o.Order.StatusId == 7 && o.ProductDetail.ProductId == pd.ProductId).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o => o);
                var uptime = dbContext.Products.FirstOrDefault(a => a.ProductId == pd.ProductId).EditTime;
                var starctsum = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == pd.ProductId).Select(a => (double)a.CommentStar).Sum();
                var starct = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == pd.ProductId).Select(a => (double)a.CommentStar);
                var staravg = starctsum / starct.Count();
                //var prodpic = dbContext.Products.FirstOrDefault(a => a.ProductId == pd.ProductId).ProductPics;

                //var prodpic = dbContext.ProductPics.Where(a => a.ProductId == pd.ProductId).FirstOrDefault().Pic;


                Card賣場ViewModel outCardContent = new Card賣場ViewModel
                {
                    ProductName = pd.ProductName,
                    ProductPrice = pddt.UnitPrice.ToString(),
                    SoldQuantity = sales,
                    AddedTime = uptime,
                    StarCount = staravg,
                    //Producpic = prodpic,
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


            listprod = dbContext.Products.Where(p => p.SmallTypeId == id && p.ProductStatusId == 0).ToList();




            C賣場ViewModel showsalescourt = new C賣場ViewModel
            {
                SellerName = SellerNickName,
                CourtCategoryName = CategoryName,
                CourtDescription = CourtDescription,
                //CardProduct = CardContent,
                star = star_count,
                Picture = MemPic,
                SellerId = id,
                MemberId = memid
            };

            return View(showsalescourt);
        }



        public IActionResult 評價(int id)
        {//針對某特定賣家評價



            var Comment = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id);
            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            //計算有購買的買家數量  
            var ordtail = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id)
                .Select(a => a.Order.MemberId).Distinct();

            //order shipping date- receive date
            //平均出貨天數       order中的 shippingday-orderdatetime
            var a = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id).Select(a => a.Order.ShippingDate).ToList();
            var b = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.MemberId == id).Select(a => a.Order.PaymentDate).ToList();

            List<double> DateSum = new List<double>();
            double SumShipDays = 0;

            for (int i = 0; i < a.Count; i++)
            {
                TimeSpan TS = new TimeSpan(a[i].Ticks - b[i].Ticks);
                double diff = 0;
                if (TS.TotalDays >= 0)
                {
                    diff = Convert.ToDouble(TS.TotalDays);
                }
                SumShipDays += diff;
            }


            var Follow = dbContext.Follows.Where(a => a.MemberId == id);
            #region
            //每個不同時段評價計數
            var weekBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now - a.CommentTime).Days <= 7);
            var monthBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now - a.CommentTime).Days <= 31);
            var halfmonthBestComment = Comment.ToList().Where(a => a.CommentStar == 5 && (DateTime.Now - a.CommentTime).Days < 186);
            var allBestComment = Comment.ToList().Where(a => a.CommentStar == 5);

            var weekMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days <= 7);
            var monthMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days <= 31);
            var halfmonthMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4) && (DateTime.Now - a.CommentTime).Days < 186);
            var allMediumComment = Comment.ToList().Where(a => (a.CommentStar == 3 || a.CommentStar == 4));


            var weekWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days <= 7);
            var monthWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days <= 31);
            var halfmonthWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2) && (DateTime.Now - a.CommentTime).Days < 186 );
            var allWorstComment = Comment.ToList().Where(a => (a.CommentStar == 1 || a.CommentStar == 2));

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

            //買家評論卡片內容
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
                Memberid = id,
                StarCount = star_count,

                BestCommentCountMethod = bstcmt,
                MediumCommentCountMethod = mdncmt,
                WorstCommentCountMethod = wrtcmt,

                SellerPhoto = Seller.MemPic,
                //卡在計算日期差距
                AvgShippingDate = SumShipDays / a.Count(),
                BuyerCount = ordtail.Count(),
                AddLoveCount = Follow.Count(),
                //刪資料  等資料表
                //AbandonOrder  = ,

                CardContent = ct
            };


            return View(outcomment);
        }

        public IActionResult 設定分類()
        {
            id = getid();
            CustomizedCategory csct = new CustomizedCategory
            {
                MemberId = id
            };
            return View(csct);
        }

        [HttpPost]
        public IActionResult 設定分類(int Memberid, string[] CustomizedCategoryName, int[] SortNumber)
        {
            CustomizedCategoryName = CustomizedCategoryName.Distinct().ToArray();
            var cname = dbContext.CustomizedCategories.Where(c => c.MemberId == Memberid).ToList();

            for (int i = 1; i < CustomizedCategoryName.Length + 1; i++)
            {      //這邊判斷是否是之前有的類別就不加入
                if (cname.Select(o => o.CustomizedCategoryName).Contains(CustomizedCategoryName[i - 1]))
                {
                    cname.Where(o => o.CustomizedCategoryName == CustomizedCategoryName[i - 1]).FirstOrDefault().SortNumber = i;
                }
                else
                {
                    CustomizedCategory a = new CustomizedCategory() { CustomizedCategoryName = CustomizedCategoryName[i - 1], MemberId = Memberid, SortNumber = i };
                    dbContext.CustomizedCategories.Add(a);
                }
            }

            dbContext.SaveChanges();

            return View("賣場", new { id = Memberid });
          
        }

        public IActionResult AddCustomProduct(int id) {
            var cust = dbContext.Products.Where(a => a.MemberId == id).ToList();

            foreach (var item in cust) {
                if (item.ProductName.Contains("食用花") || item.ProductName.Contains("無農藥新鮮薄荷葉")
                    || item.ProductName.Contains("隨機綜合生菜") || item.ProductName.Contains("新鮮無農藥芝麻葉")) {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("有機")).CustomizedCategoryId;

                    item.CustomizedCategoryId = name;
                }


                else if (item.ProductName.Contains("小葉紅酸膜 / 盒裝") || item.ProductName.Contains("平葉巴西里/義大利巴西里 / 歐芹 (500g/包）")
                    || item.ProductName.Contains("苜蓿芽 盒裝") || item.ProductName.Contains("新鮮無農藥綠捲鬚"))
                {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("卷鬚")).CustomizedCategoryId;
                    item.CustomizedCategoryId = name;
                }

                else if (item.ProductName.Contains("新鮮捲葉巴西里") || item.ProductName.Contains("綜合生菜盒 /健康蔬菜盒")
                   || item.ProductName.Contains("紅橡木生菜 500g") || item.ProductName.Contains("紅火焰生菜 300g"))
                {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("葉菜")).CustomizedCategoryId;
                    item.CustomizedCategoryId = name;
                }

                else if (item.ProductName.Contains("香茅 1000g&500g") || item.ProductName.Contains("無農藥紅捲鬚 300g")
                   || item.ProductName.Contains("新鮮綠火焰生菜300g") || item.ProductName.Contains("香料新鮮無農藥咖喱葉50g/包"))
                {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("熱火焰")).CustomizedCategoryId;
                    item.CustomizedCategoryId = name;
                }
                else if (item.ProductName.Contains("無農藥茵陳蒿/絨蒿葉") || item.ProductName.Contains("香料奧勒岡 50g/包。下單前請先詢問")
                   || item.ProductName.Contains("紅包生每份500g ± 20g") || item.ProductName.Contains("南薑"))
                {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("茼蒿")).CustomizedCategoryId;
                    item.CustomizedCategoryId = name;
                }
                else if (item.ProductName.Contains("泰國檸檬葉100g/包") || item.ProductName.Contains("無農藥貝比生菜（大盒）")
                   || item.ProductName.Contains("皺葉萵苣（1000g/包）") || item.ProductName.Contains("櫻桃蘿蔔 1 盒裝 300g"))
                {
                    var name = dbContext.CustomizedCategories.FirstOrDefault(a => a.CustomizedCategoryName.Equals("雷夢")).CustomizedCategoryId;
                    item.CustomizedCategoryId = name;
                }
            }

            dbContext.SaveChanges();

            return Json(cust);
        }

        public IActionResult 關於我(int id)
        {
            if(id.ToString().Equals(""))      id = getid();

            //MemberAccount x = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();


            //var servicetime = dbContext.MemberAccounts.FirstOrDefault(a => a.MemberId == id);
            //string[] words = servicetime.ServiceTime.Split('/');

            C關於我ViewModel me = new C關於我ViewModel();
            #region
            //抓賣場服務時間內容  
            //foreach (var word in words)
            //{

            //    string[] onerow = word.Split(',');
            //    //onerow 一次可能帶出  只有  星期   時間    休息
            //    if (onerow[0] != null)
            //    {

            //        if (onerow[0] == "0")
            //        {     //星期

            //            if (onerow[1] != null)
            //            {
            //                me.weekDown = onerow[1];
            //            }
            //            if (onerow[2] != null)
            //            {
            //                me.weekUp = onerow[2];
            //            }

            //        }
            //        if (onerow[0] == "1")
            //        {     //時間
            //            if (onerow[1] != null)
            //            {
            //                me.timeDown = onerow[1];
            //            }
            //            if (onerow[2] != null)
            //            {
            //                me.timeUp = onerow[2];
            //            }
            //        }
            //        if (onerow[0] == "2")
            //        {     //每週
            //            if (onerow[1] != null)
            //            {
            //                me.takebreak = onerow[1];
            //            }
            //        }
            //    }

            //}
            #endregion

            C關於我ViewModel outp = new C關於我ViewModel
            {
                Memberid = id,
            };

            return View(outp);
        }

        public IActionResult 新增關於我()
        {

            MemberAccount x = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();

            //if (x != null) return RedirectToAction("修改關於我");


            C關於我ViewModel outp = new C關於我ViewModel
            {
                Memberid = x.MemberId
            };

            return View(outp);
        }

        [HttpPost]
        public IActionResult 新增關於我(C關於我ViewModel me, string[] ServiceTime)
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

            //如果有要新增或修改賣場服務時間的欄位    就要先把內容清空  再把值加入
        


            dbContext.SaveChanges();
            return RedirectToAction("關於我");
            //return Content("Success", "text/plain");
        }


        public IActionResult 修改關於我()
        {

            id = getid();

            var servicetime = dbContext.MemberAccounts.FirstOrDefault(a => a.MemberId == id);
            C關於我ViewModel me = new C關於我ViewModel();
            me.Memberid = id;

            #region
            //if (servicetime.ServiceTime != null)  //當有內容
            //{
            //    string[] words = servicetime.ServiceTime.Split('/');

            //    //抓賣場服務時間內容  
            //    foreach (var word in words)
            //    {

            //        string[] onerow = word.Split(',');
            //        //onerow 一次可能帶出  只有  星期   時間    休息
            //        if (onerow[0] != null)
            //        {
            //            if (onerow[0] == "0")
            //            {     //星期

            //                if (onerow[1] != null)
            //                {
            //                    me.weekDown = onerow[1];
            //                }
            //                if (onerow[2] != null)
            //                {
            //                    me.weekUp = onerow[2];
            //                }

            //            }
            //            if (onerow[0] == "1")
            //            {     //時間
            //                if (onerow[1] != null)
            //                {
            //                    me.timeDown = onerow[1];
            //                }
            //                if (onerow[2] != null)
            //                {
            //                    me.timeUp = onerow[2];
            //                }
            //            }
            //            if (onerow[0] == "2")
            //            {     //每週
            //                if (onerow[1] != null)
            //                {
            //                    me.takebreak = onerow[1];
            //                }
            //            }
            //        }

            //    }

            //}

            #endregion





            return View(me);
        }

        
        public IActionResult AlterMe(int Memberid,string asd)
        {
            var add = dbContext.MemberAccounts.FirstOrDefault(a => a.MemberId == Memberid);
            add.Description = asd;
                      
            
            dbContext.SaveChanges();

            //return Json(add);
           return Json("1");

        }

        public IActionResult 編輯賣場資訊()
        {
            id = getid();


            Card賣場ViewModel card = new Card賣場ViewModel()
            {
                MemberId = id,
            };

            return View(card);
        }



        public IActionResult GetCustCategory(int id)
        {
            return Json(dbContext.CustomizedCategories.Where(c => c.MemberId == id).Select(c => new { c.Products.Count, c.CustomizedCategoryName, c.SortNumber }).OrderBy(o => o.SortNumber).ToList());
        }

        //public IActionResult GetBestSell(int id)
        //{
        //    return Json(dbContext.Products.Where(a => a.MemberId == id).Select(b => new
        //    {
        //        link = "/Item/Index?id=" + b.ProductId,
        //        pic = b.ProductPics.FirstOrDefault().Pic,
        //        name = b.ProductName,
        //        price1 = b.ProductDetails.Select(a => a.UnitPrice).Min(),
        //        price2 = b.ProductDetails.Select(a => a.UnitPrice).Max(),
        //        star = (dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == b.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == b.ProductId).Select(a => (int)a.CommentStar).Sum() / dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == b.ProductId).Select(a => a.CommentStar).ToList().Count,
        //        sales = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == b.ProductId).Select(a => a.Quantity).Sum(),
        //        upload = b.EditTime,
        //    }).OrderByDescending(a => a.sales).ToList());

        //} 



        public IActionResult GetComment(int id, int mode, int pages, int eachpage,int buyerorseller)
        {
            // 買家對  訂單評價    

            if (buyerorseller == 1)
            {
                var q = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id).Select(p => new
                {
                    link = "/Item/Index?id=" + p.OrderDetail.ProductDetail.Product.ProductId,
                    buyername = p.OrderDetail.Order.Member.MemberAcc,
                    productname = p.OrderDetail.ProductDetail.Product.ProductName,
                    commentcontent = p.Comment1,
                    commentstar = p.CommentStar,
                }).ToList();


                if (mode == 0)
                {
                }

                else if (mode == 1)
                {
                    q = q.ToList();
                }
                //優良
                else if (mode == 2)
                {
                    q = q.Where(a => a.commentstar == 5).ToList();
                }
                //普通
                else if (mode == 3)
                {
                    q = q.Where(a => a.commentstar == 4 || a.commentstar == 3).ToList();
                }
                //差勁
                else
                {
                    q = q.Where(a => a.commentstar == 2 || a.commentstar == 1).ToList();
                }

                int count = q.Count();

                return Json(new { list = q.Skip((pages - 1) * eachpage).Take(eachpage).ToList(), count });

            }

            //賣家對買家的評價
            else if(buyerorseller ==2){

                var q = dbContext.CommentForCustomers.Where(a => a.Order.MemberId == id).Select(p => new
                {
                    link = "/SalesCourt/賣場?id=" + p.Order.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberId,
                    buyername = p.Order.Member.MemberAcc,   /*我的名字*/
                    sellername = p.Order.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                    commentcontent = p.Comment,
                    commentstar = p.CommentStar,
                }).ToList();
                //var q = dbContext.Orders.Where(a => a.MemberId == id).Select(p => new
                //{
                //    buyername = p.Member.MemberAcc,
                //    sellername = p.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                //    commentcontent = "",
                //    commentstar = 5,
                //}).ToList();
                //var q = dbContext.OrderDetails.Where(a => a.or == id).Select(p => new
                //{
                //    //link = "/Item/Index?id=" + p.OrderDetail.ProductDetail.Product.ProductId,
                //    buyername = p.Order.Member.MemberAcc,
                //    sellername = p.ProductDetail.Product.Member.MemberAcc,
                //    commentcontent = p.Order.CommentForCustomers.Select(b => b.Comment).FirstOrDefault(),
                //    commentstar = p.Order.CommentForCustomers.Select(b => b.CommentStar).FirstOrDefault(),
                //}).ToList();

                if (mode == 0)
                {
                }

                else if (mode == 1)
                {
                    q = q.ToList();
                }
                //優良
                else if (mode == 2)
                {
                    
                    q = q.Where(a => a.commentstar == 5).ToList();
                    //foreach(var item in q)
                    //q = q.Where(a => a.commentstar.c).ToList();
                }
                //普通
                else if (mode == 3)
                {
                    q = q.Where(a => a.commentstar == 4 || a.commentstar == 3).ToList();
                    //q = q.Where(a => a.commentstar == 4 || a.commentstar == 3).ToList();
                }
                //差勁
                else
                {
                    q = q.Where(a => a.commentstar == 2 || a.commentstar == 1).ToList();
                    //q = q.Where(a => a.commentstar == 2 || a.commentstar == 1).ToList();
                }

                int count = q.Count();

                return Json(new { list = q.Skip((pages - 1) * eachpage).Take(eachpage).ToList(), count });
            }

            else
            {
                return Json(0);
            }
        }

        
        public IActionResult GetCoupon(int couponid) {
            id = getid();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                var q = dbContext.CouponWallets.Where(a => a.MemberId == id && a.CouponId == couponid).FirstOrDefault();
                if (q == null)
                {
                    CouponWallet couponwallet = new CouponWallet
                    {
                        MemberId = id,
                        CouponId = couponid,
                        IsExpired = false,
                    };
                    dbContext.CouponWallets.Add(couponwallet);
                    dbContext.SaveChanges();
                    return Json("1");
                }

                else
                {
                    return Json("2");
                }

            }


            else {
                return Json("0");
            }
        }
        public IActionResult GetCustomize(int id)
        {
            var q = dbContext.CustomizedCategories.Where(a => a.MemberId == id).Select(p => new
            {
                name = p.CustomizedCategoryName,
                count = p.Products.Where(a => a.CustomizedCategoryId == p.CustomizedCategoryId).Count(),

            }).ToList();

            //q = q.Where(a => a.name == custname).ToList();

            return Json(q);
        }

        public IActionResult GetproductCard(int id)
        {
            var q = dbContext.Products.Where(a => a.ProductId == id).Select(p => new
            {
                link = "/Item/Index?id=" + p.ProductId,
                pic = p.ProductPics.FirstOrDefault().Pic,
                name = p.ProductName,
                price1 = p.ProductDetails.Select(a => a.UnitPrice).Min(),
                price2 = p.ProductDetails.Select(a => a.UnitPrice).Max(),
                star = (dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => (int)a.CommentStar).Sum() / dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count,
                sales = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.Quantity).Sum(),
                upload = p.EditTime,
                customizename = p.CustomizedCategory.CustomizedCategoryName,

            }).ToList();

            return Json(q);
        }

        public IActionResult GetCard(int id)
        {
            var q = dbContext.Products.Where(a => a.MemberId == id).Select(p => new
            {
                link = "/Item/Index?id=" + p.ProductId,
                pic = p.ProductPics.FirstOrDefault().Pic,
                productid = p.ProductId,
                name = p.ProductName,
                price1 = p.ProductDetails.Select(a => a.UnitPrice).Min(),
                price2 = p.ProductDetails.Select(a => a.UnitPrice).Max(),
                star = (dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => (int)a.CommentStar).Sum() / dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count,
                sales = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.Quantity).Sum(),
                upload = p.EditTime,
                customizename = p.CustomizedCategory.CustomizedCategoryName,
                isfeatureproduct = p.IsFeaturedProduct,

            }).ToList();

            return Json(q);
        }

        public IActionResult GetAvgStar(int id)
        {
            var allstar = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id).Select(a => (double)a.CommentStar).Sum();
            var allstarcount = dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.MemberId == id).Count();

            var avgstar = allstar / allstarcount;

            return Json(avgstar);
        }
        public IActionResult GetAlterMe(int id) {
            var q = dbContext.MemberAccounts.FirstOrDefault(a => a.MemberId == id).Description;
            return Json(q);

        }

        public IActionResult deleteCustName(int id,string custname)
        {
            
            var q = dbContext.CustomizedCategories.Where(a => a.MemberId == id && a.CustomizedCategoryName == custname).FirstOrDefault();
            var product = dbContext.Products.Where(a => a.CustomizedCategoryId == q.CustomizedCategoryId).ToList();
            foreach(var prd in product)
            {
                prd.CustomizedCategoryId = 1;
            }
            if (q != null) dbContext.CustomizedCategories.Remove(q);

            dbContext.SaveChanges();
            
            return Json(q);
        }

        public IActionResult getPay(int id) {
            //抓 個人的 運送方式   付費方式
                        
            var payment = dbContext.PaymentToSellers.Where(a => a.MemberId == id).Select(p => new { 
                
                payid = p.PaymentId,                
                payname = p.Payment.PaymentName,

            }).ToList();

            int count = payment.Count();
            return Json(new {list = payment,count});
        }

        public IActionResult getShip(int id)
        {
            var shipper = dbContext.ShipperToSellers.Where(a => a.MemberId == id).Select(p => new { 
                shipid = p.ShipperId,
                shipname = p.Shipper.ShipperName,
            }).ToList();
            int count = shipper.Count();
            return Json(new {list = shipper , count});
        }

        

        public IActionResult GetItems(int id, int mode, int pages, int eachpage, string keyword, string customname)
        {
            var q = dbContext.Products.Where(a => a.MemberId == id && a.ProductStatusId != 1 && a.ProductStatusId != 2).Select(p => new
            {
                link = "/Item/Index?id=" + p.ProductId,
                pic = p.ProductPics.FirstOrDefault().Pic,
                name = p.ProductName,
                price1 = p.ProductDetails.Select(a => a.UnitPrice).Min(),
                price2 = p.ProductDetails.Select(a => a.UnitPrice).Max(),
                star = (dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count == 0) ? 0 : dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => (int)a.CommentStar).Sum() / dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.CommentStar).ToList().Count,
                sales = dbContext.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.Quantity).Sum(),
                upload = p.EditTime,
                customizename = p.CustomizedCategory.CustomizedCategoryName,

            }).ToList();

            if (mode == 0)
            {

            }
            //綜和排序
            else if (mode == 1)
            {

            }
            //最新商品
            else if (mode == 2)
            {

                q = q.OrderByDescending(a => a.upload).ToList();
            }
            //熱銷商品
            else if (mode == 3)
            {

                q = q.OrderByDescending(a => a.sales).ToList();
            }
            //價格排序  由高到低
            else if (mode == 4)
            {

                q = q.OrderByDescending(a => a.price1).ToList();
            }
            //價格排序  由低到高
            else if (mode == 5)
            {

                q = q.OrderBy(a => a.price1).ToList();
            }
            else
            {

            }
            //找關鍵字
            if (keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    q = q.Where(a => a.name.Contains(keys[i]) || a.price1.ToString().Contains(keys[i])
                    || a.price2.ToString().Contains(keys[i]) || a.sales.ToString().Contains(keys[i])).ToList();
                }
            }
            //判別類別名稱相同
            if (customname != null)
            {
                q = q.Where(a => a.customizename == customname).ToList();
            }
            int count = q.Count();
            return Json(new { list = q.Skip((pages - 1) * eachpage).Take(eachpage).ToList(), count });
        }

        public IActionResult GetChoice(int productid) {

            var choice = dbContext.Products.FirstOrDefault(a => a.ProductId == productid).IsFeaturedProduct;
            return Json(choice);
        }


        public IActionResult WriteChoice(int productid,bool change)
        {
            var choice = dbContext.Products.FirstOrDefault(a => a.ProductId == productid);
            choice.IsFeaturedProduct = change;
            dbContext.SaveChanges();
            return Json(choice);
        }


        public IActionResult WriteFollow(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int myid = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            if (myid == id)
            {
                return Json("999");
            }
            if (dbContext.Follows.Where(f => f.MemberId == myid && f.FollowedMemId == id).Any())
            {
                dbContext.Follows.Remove(dbContext.Follows.Where(f => f.MemberId == myid && f.FollowedMemId == id).FirstOrDefault());
                dbContext.SaveChanges();
                return Json("0");
            }
            else
            {
                dbContext.Follows.Add(new Follow() { FollowedMemId = id, MemberId = myid });
                dbContext.SaveChanges();
                return Json("1");
            }
        }
    }
}

