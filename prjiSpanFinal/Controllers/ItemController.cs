using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
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
            var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i).ToList();
            var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i).ToList();
            var sellerProducts = dbContext.Products.Where(i => i.MemberId == product.MemberId && i.ProductId != product.ProductId).Select(i => i).ToList();
            List<CItemIndexSellerProductViewModel> sellerProductList = new List<CItemIndexSellerProductViewModel>();
            foreach (var p in sellerProducts)
            {
                string productName = p.ProductName;
                byte[] productPic = dbContext.ProductPics.Where(i => i.ProductId == p.ProductId).Select(i => i.Picture).FirstOrDefault();
                var prices = dbContext.ProductDetails.Where(i => i.ProductId == p.ProductId).Select(i => i.UnitPrice).ToList();
                decimal maxPrice = prices.Max();
                decimal minPrice = prices.Min();
                string price = "1";
                if (maxPrice == minPrice)
                {
                    price = $"${minPrice.ToString("0")}";
                }
                else
                {
                    price = $"${minPrice.ToString("0")} - ${maxPrice.ToString("0")}";
                }
                double starCount = dbContext.Comments.Where(i => i.ProductId == p.ProductId).Average(i=>i.Star);
                int salesVolume = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == p.ProductId && i.Order.StatusId == 6).Sum(i => i.Quantity);


                CItemIndexSellerProductViewModel sellerProduct = new CItemIndexSellerProductViewModel
                {
                    productName = productName,
                    productPic = productPic,
                    price = price,
                    starCount = starCount,
                    salesVolume = salesVolume
                };
                sellerProductList.Add(sellerProduct);
            }
            sellerProductList = sellerProductList.OrderByDescending(i => i.starCount).ToList();
            CItemIndexViewModel itemIndex = new CItemIndexViewModel
            {
                product = product,
                productDetails = productDetails,
                productPics = productPics,
                sellerProducts = sellerProductList
            };

            return View(itemIndex);
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
