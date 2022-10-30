using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class WebADLargeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<WebAd> list)
        {
            return View(list[0]);
        }
    }
}
