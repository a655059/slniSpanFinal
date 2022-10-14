using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ProductCardViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
