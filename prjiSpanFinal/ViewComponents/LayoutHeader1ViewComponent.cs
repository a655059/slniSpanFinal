using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Header;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            else
            {
                string loginstr = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                if (loginstr == null)
                {
                    return View(new CHeader1ViewModel());
                }
                else
                {
                    MemberAccount a = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                    CHeader1ViewModel b = new CHeader1ViewModel();
                    b.MemberAcc = a.MemberAcc;
                    if (a.MemPic != null)
                    {
                        b.Mempic = a.MemPic;
                    }
                    else
                    {
                        b.Mempic = File.ReadAllBytes("~/img/Member/nopicmem.jpg");
                    }
                    return View(b);
                }
            }
            
        }
    }
}
