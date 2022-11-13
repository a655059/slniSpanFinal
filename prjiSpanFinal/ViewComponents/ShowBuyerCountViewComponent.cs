using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ShowBuyerCountViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int productID, int page)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var q = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == productID && i.Order.StatusId == 7).Select(i => new CShowBuyerCountViewModel
            {
                buyer = i.Order.Member,
                buyCount = i.Quantity,
                finishDate = i.Order.FinishDate.ToString("yyyy-MM-dd"),
                page = page
            }).ToList();
            var q1 = q.GroupBy(i => i.buyer).Select(g => new CShowBuyerCountViewModel
            {
                buyer = g.Key,
                buyCount = g.Sum(i => i.buyCount),
                finishDate = g.Select(i=>i.finishDate).LastOrDefault(),
                page = page
            }).OrderByDescending(i=>i.finishDate).ToList();
            
            return View(q1);
        }
    }
}
