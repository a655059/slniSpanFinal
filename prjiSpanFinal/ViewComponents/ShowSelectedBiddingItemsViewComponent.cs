using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ShowSelectedBiddingItemsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<int> biddingIDs)
        {
            return View(biddingIDs);
        }
    }
}
