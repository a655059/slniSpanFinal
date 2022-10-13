using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult Shipper()
        {
            return View();
        }

        public IActionResult Center()
        {
            return View();
        }

        public IActionResult NewIndex()
        {
            return View();
        }

        public IActionResult Coupon()
        {
            return View();
        }

        public IActionResult Even()
        {
            return View();
        }
    }
}
