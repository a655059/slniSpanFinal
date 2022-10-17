using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class MemberController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly iSpanProjectContext _context;
        public MemberController(iSpanProjectContext context, IWebHostEnvironment host) 
        {
            _host = host;
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
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        
       
        public IActionResult Create1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create1(MemberAccount mem, IFormFile File1)
        {

            //string filePath = Path.Combine(_host.WebRootPath, "uploads", File1.FileName);
            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
            //    File1.CopyTo(fileStream);
            //}
            //var m = from i in _context.MemberAccounts
            //        select i;
            //var re = from i in _context.RegionLists
            //         select i;
            var sites = _context.RegionLists.Where(a => a.RegionName == site).Select(a => a.RegionId).Distinct();


            int reginid_1 = Convert.ToInt32(sites);



            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                //File1.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }
            mem.MemPic = imgByte;
            mem.RegionId = reginid_1;
            _context.MemberAccounts.Add(mem);
            _context.SaveChanges();
            return RedirectToAction("Login");
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
