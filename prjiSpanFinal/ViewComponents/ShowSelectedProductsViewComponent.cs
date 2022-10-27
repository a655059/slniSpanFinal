using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ShowSelectedProductsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<int> productIDs)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            List<CItemIndexSellerProductViewModel> cItemIndexSellerProductList = new List<CItemIndexSellerProductViewModel>();
            foreach (var a in productIDs)
            {
                var product = dbContext.ProductDetails.Where(i => i.ProductId == a).Select(i => new
                {
                    productID = i.ProductId,
                    productName = i.Product.ProductName,
                    price = i.UnitPrice,
                }).ToList();
                byte[] productPic = dbContext.ProductPics.Where(i => i.ProductId == a).Select(i => i.Pic).FirstOrDefault();
                int maxPrice = Convert.ToInt32(product.Max(i => i.price));
                int minPrice = Convert.ToInt32(product.Min(i => i.price));
                string price = "";
                double starCount = 0;
                int salesVolume = 0;
                if (minPrice == maxPrice)
                {
                    price = $"${minPrice}";
                }
                else
                {
                    price = $"${minPrice} - ${maxPrice}";
                }
                var comment = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.ProductId == a).Select(i => i);
                if (comment.Count() > 0)
                {
                    starCount = comment.Average(i => i.CommentStar);
                }
                var orderDetail = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == a && i.Order.StatusId == 7).Select(i => i);
                if (orderDetail.Count() > 0)
                {
                    salesVolume = orderDetail.Sum(i => i.Quantity);
                }

                CItemIndexSellerProductViewModel cItemIndexSellerProduct = new CItemIndexSellerProductViewModel
                {
                    productID = product[0].productID,
                    productName = product[0].productName,
                    productPic = productPic,
                    price = price,
                    starCount = starCount,
                    salesVolume = salesVolume
                };
                cItemIndexSellerProductList.Add(cItemIndexSellerProduct);
            }
            var x = cItemIndexSellerProductList.OrderByDescending(i => i.starCount).Select(i => i).ToList();
            return View(x);
        }
    }
}
