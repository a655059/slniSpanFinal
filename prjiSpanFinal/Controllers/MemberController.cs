using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewComponents;
using prjiSpanFinal.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class MemberController : Controller
    {
        private readonly iSpanProjectContext _context;
        public MemberController(iSpanProjectContext context) 
        {
            _context = context;
        }

        public IActionResult List()
        {
            return View();
        }
        //public IActionResult productShow()
        //{
        //    return PartialView("cshtml");
        //}
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginCheck(string txtAccount, string txtPW)
        {
            var mem = _context.MemberAccounts.FirstOrDefault(m => m.MemberAcc == txtAccount);
            if (mem != null)
            {
                if (mem.MemberPw == txtPW)
                {
                    string jsonUser = JsonSerializer.Serialize(mem);
                    HttpContext.Session.SetString(CDictionary.SK_LOGINED_USER, jsonUser);
                    return Content("1", "text/plain", Encoding.UTF8);
                }
            }
            return Content("0", "text/plain", Encoding.UTF8); ;
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Like()
        {
            return View();
        }
        public IActionResult Coupon()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult OrderDetail()
        {
            return View();
        }
        public IActionResult forgetPw()
        {
            return View();
        }
        public IActionResult ChangePw()
        {
            return View();
        }
        public IActionResult City()
        {
            var cities = _context.CountryLists.Select(a => a.CountryName).Distinct();
            return Json(cities);
        }
        public IActionResult getCityID(string city)
        {
            var sites = _context.CountryLists.Where(a => a.CountryName == city).Select(a => a.CountryId).Distinct();
            return Json(sites);
        }
        public IActionResult Site(int site)
        {
            var sites = _context.RegionLists.Where(a => a.CountryId == site).Select(a => a.RegionName).Distinct();
            return Json(sites);
        }
    }
}
