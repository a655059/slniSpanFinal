using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.ViewModels.Event;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class EventCouponViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CShowCoupon cs)
        {
            return View(cs);
        }
    }
}
