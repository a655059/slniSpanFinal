using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class LayoutHeader1ViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string ctrlName = RouteData.Values["Controller"].ToString();
            //string atnName = RouteData.Values["Action"].ToString();
            if (ctrlName == "Management")
            {
                return View("Manage");
            }
            else if(ctrlName != "Home")
            {
                return View(12345678);
            }
            else
            {
                return View(0);
            }
            
        }
    }
}
