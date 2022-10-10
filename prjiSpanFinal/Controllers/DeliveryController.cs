using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowCart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult CheckoutPartial(int id)
        {
            return PartialView("~/Views/Delivery/_DeliveryCheckoutPartial.cshtml", id);
        }
        public IActionResult AddComment()
        {
            return View();
        }
        

    }
}
