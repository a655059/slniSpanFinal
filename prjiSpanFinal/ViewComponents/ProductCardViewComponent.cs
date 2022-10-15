using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Item;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ProductCardViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CItemIndexSellerProductViewModel sellerProduct)
        {
            return View(sellerProduct);
        }
    }
}
