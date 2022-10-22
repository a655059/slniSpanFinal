using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Header;
using prjiSpanFinal.ViewModels;
using System;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace prjiSpanFinal.ViewComponents
{
    public class MemberUIViewComponent : ViewComponent
    {
        private IWebHostEnvironment _enviro;
        public MemberUIViewComponent(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IViewComponentResult Invoke()
        {
            string ctrlName = RouteData.Values["Controller"].ToString();
            //string atnName = RouteData.Values["Action"].ToString();
            if (ctrlName == "newManagement")
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
                        string pName = "/img/Member/nopicmem.jpg";
                        string path = _enviro.WebRootPath + pName;
                        b.Mempic = File.ReadAllBytes(path);
                    }
                    return View(b);
                }
            }

        }
    }
}
