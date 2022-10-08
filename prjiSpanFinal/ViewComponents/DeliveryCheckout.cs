using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class DeliveryCheckoutViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> Invoke()
        {
            return View();
        }
    }
}
