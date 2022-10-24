using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class MessengerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public void NewIcon(byte[] pic)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            dbcontext.IconTypes.Add(new IconType() {IconPic = pic,IconTypeName = "123" });
            dbcontext.SaveChanges();
        }
    }
}
