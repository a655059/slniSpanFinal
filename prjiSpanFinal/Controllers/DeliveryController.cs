using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class DeliveryController : Controller
    {
        [HttpPost]
        public IActionResult Checkout(CItemIndexPurchaseViewModel purchaseItem)
        {
            if (purchaseItem.purchaseStyle < 1)
                return RedirectToAction("Index", "Item", new { id=purchaseItem.purchaseProduct});
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var productDetail = dbContext.ProductDetails.Where(i => i.ProductDetailId == purchaseItem.purchaseStyle).Select(i => i).FirstOrDefault();
            var product = dbContext.Products.Where(i => i.ProductId == productDetail.ProductId).Select(i => i).FirstOrDefault();
            var seller = dbContext.MemberAccounts.Where(i => i.MemberId == product.MemberId).Select(i => i).FirstOrDefault();
            int purchaseCount = purchaseItem.purchaseCount;
            CProductDetailViewModel cProductDetail = new CProductDetailViewModel();
            cProductDetail.productDetail = productDetail;

            CProductViewModel cProduct = new CProductViewModel();
            cProduct.product = product;

            CMemberAccountViewModel cMemberAccount = new CMemberAccountViewModel();
            cMemberAccount.memberAccount = seller;

            CDeliveryCheckoutViewModel cDeliveryCheckout = new CDeliveryCheckoutViewModel();
            cDeliveryCheckout.CProduct = cProduct;
            cDeliveryCheckout.CProductDetail = cProductDetail;
            cDeliveryCheckout.Seller = cMemberAccount;
            cDeliveryCheckout.PurchaseCount = purchaseCount;

            return View(cDeliveryCheckout);
        }
        [HttpPost]
        public IActionResult CartList(CItemIndexPurchaseViewModel purchaseItem)
        {
            
            return View();
        }
    }
}
