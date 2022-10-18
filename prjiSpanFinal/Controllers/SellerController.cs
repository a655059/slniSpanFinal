using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.seller;
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
            var bigType = _db.BigTypes.Select(i => i.BigTypeName).ToList();
            var smallType = _db.SmallTypes.Select(i => i.SmallTypeName).ToList();
            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                smallType=smallType,
            };
            return View(x);
        }
        //[HttpPost]
        //public IActionResult create(CSellerCreateViewModel product)
        //{
        //    _db.productdetails.add(product);
        //    _db.savechanges();
        //    var q = 
        //}


        //連結小類別選項

        public IActionResult SmallType(string bigtype)
        {
            var smalltype = _db.SmallTypes.Where(a => a.BigType.BigTypeName == bigtype).Select(a => a.SmallTypeName).Distinct();
            return Json(smalltype);
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
            //var q = from c in _db.Products where c.MemberId == 
            //var productID = _db.Products.Select(n => n.ProductId);
            //var productName = _db.Products.Where(n => n.ProductId = productID).Select(n => n.);
            //var Quantity = _db.ProductDetails.Select(n => n.Quantity).ToList();
            //var UnitPrice = _db.ProductDetails.Select(n => n.UnitPrice).ToList();
            //var Pic = _db.ProductDetails.Select(n => n.Pic).ToList();
            CSellerNewIndexToViewViewModel x = new CSellerNewIndexToViewViewModel
            {
                //productName = productName,
                //Style = Style.ToString(),
                //Quantity = Convert.ToInt32(Quantity),
                //UnitPrice = Convert.ToDecimal(UnitPrice),
                //Pic= Pic
            };
            return View(x);
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
