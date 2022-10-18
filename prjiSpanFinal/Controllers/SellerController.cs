using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class SellerController : Controller
    {
        private readonly IWebHostEnvironment _enviro;
        private readonly iSpanProjectContext _db;

        public SellerController(IWebHostEnvironment p, iSpanProjectContext db)
        {
            _enviro = p;
            _db = db;
        }

        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //public iactionresult create(productdetail product)
        //{
        //    _db.productdetails.add(product);
        //    _db.savechanges();
        //}
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

        public IActionResult AD()
        {
            return View();
        }

        public IActionResult seller跑條(int page)
        {
            return ViewComponent("seller跑條",page);
        }

        public IActionResult Event()
        {
            return View();
        }
    }
}
