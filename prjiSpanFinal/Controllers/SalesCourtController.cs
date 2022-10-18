using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SalesCourtController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
            iSpanProjectContext dbContext = new iSpanProjectContext();

            var Seller = dbContext.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            var SellerNickName = dbContext.MemberAccounts.Where(a => a.MemberId == id).Select(a => a.NickName);
            var CourtCategoryName = dbContext.CustomizedCategories.Where(a => a.MemberId == Seller.MemberId).FirstOrDefault();
            //var CourtDescription = dbContext.      這行好像因為關於我所以沒一個特別欄位
            //var PrimeGoodsName = dbContext.       精選商品要怎麼設定 待明天討論

            var ProductName = dbContext.Products.Where(a => a.MemberId == Seller.MemberId).Select(a => a.ProductName).ToList();
            
        }

        public IActionResult 賣場()
        {
            return View();
        }


        public IActionResult 關於我()
        {
            return View();
        }

        public IActionResult 評價() {
            return View();
        }

        public IActionResult 設定分類()
        {
            return View();
        }

        public IActionResult 修改關於我()
        {
            return View();
        }
    }
}
