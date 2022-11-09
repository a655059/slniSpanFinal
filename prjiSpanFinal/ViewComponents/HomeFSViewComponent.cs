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
    public class HomeFSViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<CShowFSItem> list)
        {
           
            return View(list);
        }
    }
}
