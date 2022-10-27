using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class MyLikeCategoryShowViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<MyLikeShowItem> shopProduct)
        {
            return View(shopProduct);
        }
    }
}
