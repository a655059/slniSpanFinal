using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class HomeController : Controller
    {
        List<CBigType> listCBigType;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        void fillin()
        {
            CBigType a = new CBigType()
            {
                BigTypeID = 1,
                BigTypeName = "食品"
            };
            CBigType b = new CBigType()
            {
                BigTypeID = 2,
                BigTypeName = "數位"
            };
            CBigType c = new CBigType()
            {
                BigTypeID = 3,
                BigTypeName = "衣裝"
            };
            listCBigType = new List<CBigType>();
            listCBigType.Add(a);
            listCBigType.Add(b);
            listCBigType.Add(c);
        }
        public IActionResult Index()
        {
            fillin();
            return View(listCBigType);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
