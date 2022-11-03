using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class BiddingItemCardViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var item = dbContext.Biddings.Where(i => i.BiddingId == id).Select(i => new CBiddingItemToCardViewModel
            {
                bidding = i,
                product = i.ProductDetail.Product,
                sellerAcc = i.ProductDetail.Product.Member.MemberAcc,
                currentBiddingPrice = Convert.ToInt32(i.ProductDetail.UnitPrice),
            }).FirstOrDefault();
            byte[] productPic = dbContext.ProductPics.Where(i => i.ProductId == item.product.ProductId).Select(i => i.Pic).FirstOrDefault();
            if (productPic != null)
            {
                item.productPic = productPic;
            }
            var biddingDetails = dbContext.BiddingDetails.Where(i => i.BiddingId == id).ToList();
            if (biddingDetails.Count > 0)
            {
                int currentBiddingPrice = biddingDetails.Max(i => i.Price);
                int biddingCount = biddingDetails.Count;
                item.currentBiddingPrice = currentBiddingPrice;
                item.biddingCount = biddingCount;
            }
            else
            {
                item.biddingCount = 0;
            }
            return View(item);
        }
    }
}
