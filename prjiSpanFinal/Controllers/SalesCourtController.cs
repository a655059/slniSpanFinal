﻿using Microsoft.AspNetCore.Mvc;
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
            

            var SellerNickName = Seller.NickName;
            var CourtDescription = Seller.Description;
          
            List<string> CategoryName = new List<string>();

            foreach(var catname in CourtCategory)
            {
                CategoryName.Add(catname.CustomizedCategoryName);
            }
       
            


            List<Card賣場ViewModel> CardContent = new List<Card賣場ViewModel>();
            foreach (var pd in Product)
            {
                var pddt = dbContext.ProductDetails.Where(a => a.ProductId == pd.ProductId).FirstOrDefault();
                var oddt = dbContext.OrderDetails.Where(a => a.ProductDetailId == pddt.ProductDetailId).Select(a=>a.Comments.Select(c=>c.CommentStar)).ToList();


                

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



            var ss = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == 2)
                    .Select(a => (double)a.CommentStar).Sum();
            var ss1 = dbContext.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == 2)
                .Select(a => (double)a.CommentStar);


            double star_count = ss / ss1.Count();

            C賣場ViewModel showsalescourt = new C賣場ViewModel
            {
                SellerName = SellerNickName,
                CourtCategoryName = CategoryName,
                CourtDescription = CourtDescription,
                CardProduct = CardContent,
                star = star_count
            };

            //return View(Seller);
            return View(showsalescourt);
        }


        

        public IActionResult 評價() {
            return View();
        }

        public IActionResult 設定分類()
        {
            return View();
        }

        public IActionResult 關於我(int id)
        {
            var PayingMethod = dbContext.PaymentToSellers.Where(a => a.Member.MemberId == id).ToList();


            return View();
        }

        public IActionResult 新增關於我()
        {
            return View();
        }

        [HttpPost]
        public IActionResult 新增關於我(C關於我ViewModel me)
        {
            var NewMe = new C關於我ViewModel()
            {
                PayingMethod = me.PayingMethod,
                // delivery = me.DeliveryMethod,
            };
            return View();
        }

        


        public IActionResult 修改關於我(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult 修改關於我(C關於我ViewModel? me)
        {
            if(me != null)
            {

                return RedirectToAction("關於我");
            }

            return RedirectToAction("關於我");

        }
    }
}
