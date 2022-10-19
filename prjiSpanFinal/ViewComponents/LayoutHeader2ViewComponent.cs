using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class LayoutHeader2ViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string ctrlName = RouteData.Values["Controller"].ToString();
            //string atnName = RouteData.Values["Action"].ToString();
            string[] UsedftHeader2ctrlList = new string[] { "Home", "Item", "Category", "SalesCourt" };

            if (UsedftHeader2ctrlList.Contains(ctrlName))
            {
                return View();
            }
            else if (ctrlName == "Management" || ctrlName == "newManagement")
            {
                return View("~/Views/Shared/Components/ManagementLayout/Default.cshtml");
            }
            //else if(atnName == "")
            else
            {
                return View("Blank");
            }
            
        }
    }
}
