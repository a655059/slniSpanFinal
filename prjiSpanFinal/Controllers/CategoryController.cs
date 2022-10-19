using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class CategoryController : Controller
    {
        List<CProduct> listprod;

        public IActionResult Index(int id)
        {
            
            //ViewBag.prodlist = a;
            return View(listprod);
        }
        public IActionResult SearchResult()
        {
            CBigType a = new CBigType()
            {
                BigTypeID = 1,
                BigTypeName = "水果"
            };
            ViewBag.prodlist = a;
            return View(listprod);
        }
    }
}
