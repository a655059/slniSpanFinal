using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
                CProductViewModel cProduct = new CProductViewModel();
                cProduct.product = product;
                

                var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i);
                List<CProductDetailViewModel> cProductDetails = new List<CProductDetailViewModel>();
                foreach (var i in productDetails)
                {
                    CProductDetailViewModel cProductDetail = new CProductDetailViewModel();
                    cProductDetail.productDetail = i;
                    cProductDetails.Add(cProductDetail);
                }

                var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i);
                List<CProductPicViewModel> cProductPics = new List<CProductPicViewModel>();
                foreach (var i in productPics)
                {
                    CProductPicViewModel cProductPic = new CProductPicViewModel();
                    cProductPic.productPic = i;
                    cProductPics.Add(cProductPic);
                }

                var smallType = dbContext.SmallTypes.Where(i => i.SmallTypeId == product.SmallTypeId).Select(i => i).FirstOrDefault();
                CSmallTypeViewModel cSmallType = new CSmallTypeViewModel();
                cSmallType.smallType = smallType;

                var bigType = dbContext.BigTypes.Where(i => i.BigTypeId == smallType.BigTypeId).Select(i => i).FirstOrDefault();
                CBigTypeViewModel cBigType = new CBigTypeViewModel();
                cBigType.bigType = bigType;

                var comments = dbContext.Comments.Where(i => i.ProductId == product.ProductId).Select(i => i);
                double commentAvgScore = 0;
                int commentCount = 0;
                if (comments.Count() != 0)
                {
                    commentAvgScore = comments.Average(i => (double)(byte)i.Star);
                    commentCount = comments.Count();
                }

                int sellCount = 0;
                var orderDetails = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == product.ProductId && i.Order.StatusId == 6).Select(i => i.Quantity).ToList();
                if (orderDetails.Count != 0)
                {
                    sellCount = orderDetails.Sum();
                }

                CItemIndexViewModel cItemIndex = new CItemIndexViewModel();
                cItemIndex.CProducts = cProduct;
                cItemIndex.CProductDetails = cProductDetails;
                cItemIndex.CProductPics = cProductPics;
                cItemIndex.CSmallType = cSmallType;
                cItemIndex.CBigType = cBigType;
                cItemIndex.CommentAvgScore = commentAvgScore;
                cItemIndex.CommentCount = commentCount;
                cItemIndex.SellCount = sellCount;
                return View(cItemIndex);
            }
            return RedirectToAction("Index", "Home");
        }
        
    }
}
