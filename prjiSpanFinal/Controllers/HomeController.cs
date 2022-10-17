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
        List<CShowItem> sendlist;
        int counter { get; set; } = 0;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            listBigType = _db.BigTypes.Select(p => p).ToList();
            listProd = (new CHomeFactory()).rdnProd(_db.Products.Select(p => p).ToList());
            listItem = ((new CHomeFactory()).toShowItem(listProd));
            sendlist = listItem.Take(7).ToList();
        }

        public IActionResult Index()
        {
            ViewBag.listBigtype = listBigType;
            return View(sendlist);
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
        public void ShowMoreItem()
        {
            counter++;
            foreach(var item in ((new CHomeFactory()).toShowMore(listItem, 6 * (counter - 1), 6 * counter))) { 
            sendlist.Add(item);
            }
        }
    }
}
