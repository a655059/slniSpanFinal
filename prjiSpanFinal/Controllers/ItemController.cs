using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace prjiSpanFinal.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
            var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i);
            var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i);
            var sellerProducts = dbContext.Products.Where(i => i.MemberId == product.MemberId && i.ProductId != product.ProductId).Select(i => i);
            return View();
        }
        public IActionResult Description()
        {
            return PartialView("~/Views/Item/_ItemDescriptionPartial.cshtml");
        }
        public IActionResult Comment(int? id)
        {
            int buyerNum = (int)id;
            return PartialView("~/Views/Item/_ItemCommentPartial.cshtml", buyerNum);
        }
        public IActionResult BuyerCount(int? id)
        {
            return PartialView("~/Views/Item/_ItemBuyerCountPartial.cshtml", (int)id);
        }
    }
}
