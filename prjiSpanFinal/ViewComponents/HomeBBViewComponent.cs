using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class HomeBBViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<CShowBBItem> list)
        {
           
            return View(list);
        }
    }
}
