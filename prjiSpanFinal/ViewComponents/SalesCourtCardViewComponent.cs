using System;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.SalesCourt;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class SalesCourtCardViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(Card賣場ViewModel shopProduct)
        {
            return View(shopProduct);
        }
    }
}
