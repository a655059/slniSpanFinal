using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SalesCourtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult 賣場()
        {
            return View();
        }

        public IActionResult 關於我()
        {
            return View();
        }
    }
}
