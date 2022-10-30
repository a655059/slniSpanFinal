using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Home;
using prjiSpanFinal.Models.LayOutReq;
using Microsoft.AspNetCore.Http;
using System.IO;

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
            var webAds = _db.WebAds.Where(a => a.IsPublishing == true);
            
            CHomeIndex home = new CHomeIndex()
            {
                lSmallType = listSmallType,
                lBigType = listBigType,
                cShowItem = listItem,
                WebADCarousel = webAds.Where(a => a.WebAdimageTypeId == 1).ToList(),
                WebADSmall = (new CHomeFactory()).toRndImg(webAds.Where(a=>a.WebAdimageTypeId==2).ToList()),
                WebADBig = (new CHomeFactory()).toRndImg(webAds.Where(a => a.WebAdimageTypeId == 3).ToList()),
            };
            return View(home);
        }
        public IActionResult FlashSales()
        {
            return View();
        }

        //  測試用 //
        public IActionResult TestImage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TestImage(IFormFile adimg,string adnum)
        {
            if (adimg != null)
            {
                MemoryStream ms = new MemoryStream();
                adimg.CopyTo(ms);
                if (adnum==null) {

                    WebAd ads = new WebAd()
                    {
                        MemberId = 1,
                        WebAdimage = ms.ToArray(),
                        WebAdimageTypeId = 1,
                        IsPublishing = true,
                    };
                    _db.WebAds.Add(ads);
                }
                else if(_db.WebAds.Where(w => w.WebAdid == Convert.ToInt32(adnum)).Any())
                {
                    WebAd ads = _db.WebAds.Where(w => w.WebAdid == Convert.ToInt32(adnum)).FirstOrDefault();
                    ads.WebAdimage = ms.ToArray();
                }
                _db.SaveChanges();
            }
            return View();
        }
        public IActionResult WebImages()
        {
            var LIST = _db.WebAds;
            return View(LIST);
        }
        //  End測試用   //

        //  LayOutUse   //
        public IActionResult GetSearchDetail(string key)
        {
            List<string> keywordList = new List<string>();
            //No cookies
            if (Request.Cookies["key"] == null)
            {
                if (!String.IsNullOrEmpty(key)) {
                    Response.Cookies.Append("key", key);
                    keywordList.Add(key);
                }
            }
            //if cookies 
            else
            {
                string keywordReq = Request.Cookies["key"];
                keywordList = (keywordReq.Split(",")).ToList();
                if (!String.IsNullOrEmpty(key))
                {
                    if (!keywordList.Contains(key)) { 
                        keywordList.Add(key);
                        if (keywordList.Count == 8)
                        {
                            keywordList.RemoveAt(0);
                        }
                        keywordReq = String.Join(",", keywordList.ToArray());
                        Response.Cookies.Append("key", keywordReq);
                    }
                }
            }
            return Json(keywordList.ToArray());
        }
        //  EndLayOutUse  //
        
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
