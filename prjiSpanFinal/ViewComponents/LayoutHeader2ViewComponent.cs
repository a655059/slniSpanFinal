using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            CHeader2ViewModel H2ViewModel = new CHeader2ViewModel();
            if (HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER) != null) { 
                H2ViewModel.loggedMember = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            }

            if (UsedftHeader2ctrlList.Contains(ctrlName))
            {
                return View(H2ViewModel);
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
