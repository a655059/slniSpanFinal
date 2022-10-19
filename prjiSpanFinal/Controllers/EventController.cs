using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Discount()
        {
            //查USER
            return View();
        }
        public IActionResult AD(int? id)
        {
            //查USER
            return View();
        }
    }
}
