using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class CategoryShowViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(List<CShowItem> shopProduct)
        {
            return View(shopProduct);
        }

    }
}
