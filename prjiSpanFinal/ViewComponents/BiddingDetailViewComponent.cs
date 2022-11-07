using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class BiddingDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var biddingDetails = dbContext.BiddingDetails.Where(i => i.BiddingId == id).OrderByDescending(i=>i.BiddingDetailId).Select(i => new CBiddingDetailWithMemberViewModel
            {
                biddingDetail = i,
                member = i.Member,
            }).ToList();

            return View(biddingDetails);
        }
    }
}
