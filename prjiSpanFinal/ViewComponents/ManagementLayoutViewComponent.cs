using Microsoft.AspNetCore.Mvc;

namespace prjiSpanFinal.ViewComponents
{
    public class ManagementLayoutViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
