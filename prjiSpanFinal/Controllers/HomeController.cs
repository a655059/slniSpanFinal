using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Home;

namespace prjiSpanFinal.Controllers
{
    public class HomeController : Controller
    {
        iSpanProjectContext _db = new iSpanProjectContext();
        List<BigType> listBigType;
        private readonly ILogger<HomeController> _logger;
        List<Product> listProd;
        List<CShowItem> listItem;
        List<SmallType> listSmallType;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            listBigType = _db.BigTypes.Select(p => p).ToList();
            listProd = (new CHomeFactory()).rdnProd(_db.Products.Select(p => p).ToList());
            listItem = ((new CHomeFactory()).toShowItem(listProd)).Take(48).ToList();
            listSmallType = _db.SmallTypes.Select(p => p).ToList();
        }

        public IActionResult Index()
        {
            CHomeIndex home = new CHomeIndex()
            {
                lSmallType = listSmallType,
                lBigType = listBigType,
                cShowItem = listItem,
            };
            return View(home);
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
