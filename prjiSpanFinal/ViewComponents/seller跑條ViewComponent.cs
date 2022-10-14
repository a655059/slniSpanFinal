using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class seller跑條ViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page)
        {
            return View(page);
        }
    }
}
