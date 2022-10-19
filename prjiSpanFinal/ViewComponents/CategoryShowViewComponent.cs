using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Home;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class CategoryShowViewComponent : ViewComponent
    {
        
            public async Task<IViewComponentResult> InvokeAsync(CShowItem shopProduct)
            {
                return View(shopProduct);
            }
        
    }
}
