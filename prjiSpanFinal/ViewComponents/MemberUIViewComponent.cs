using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace prjiSpanFinal.ViewComponents
{
    public class MemberUIViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
