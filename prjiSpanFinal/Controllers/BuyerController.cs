using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class BuyerController : Controller
    {
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
                CProductViewModel cProduct = new CProductViewModel();
                cProduct.product = product;
                List<CProductViewModel> cProducts = new List<CProductViewModel>();
                cProducts.Add(cProduct);

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

                CBuyerIndexViewModel cBuyerIndex = new CBuyerIndexViewModel();
                cBuyerIndex.CProducts = cProducts;
                cBuyerIndex.CProductDetails = cProductDetails;
                cBuyerIndex.CProductPics = cProductPics;
                return View(cBuyerIndex);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
