using Microsoft.AspNetCore.Mvc;
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
    }
}
