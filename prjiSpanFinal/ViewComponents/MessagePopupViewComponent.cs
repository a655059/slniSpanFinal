using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class MessagePopupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //return View(JsonSerializer.Deserialize<Member>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)));
            return View();
        }
    }
}
