using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Home;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class HomeItemShowViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CShowItem shopProduct)
        {
            return View(shopProduct);
        }
    }
}
