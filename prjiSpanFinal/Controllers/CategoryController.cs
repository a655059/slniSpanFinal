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
        List<CProduct> listpro;
        void fillin()
        {
        }
        public IActionResult Index(int? id)
        {
            return View();
        }
    }
}
