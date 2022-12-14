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
using System.Text.Json;

namespace prjiSpanFinal.Controllers
{
    public class HomeController : Controller
    {
        iSpanProjectContext _db;
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger, iSpanProjectContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> listProd = (new CHomeFactory()).rdnProd(_db.Products.Select(p => p).ToList());
            List<CShowItem> listItem = ((new CHomeFactory()).toShowItem(listProd)).Take(36).ToList();
            listProd = new CHomeFactory().rdnProd(_db.AdtoProducts.Where(p => p.IsSubActive && p.Ad.AdTypeId == 3).Select(p=>p.Product).ToList());
            List<CShowBBItem> listBB = new CHomeFactory().toShowBBItem(listProd.Take(15).ToList());
            int FSevtID = _db.OfficialEventLists.Where(e=>e.OfficialEventListId!=1 &e.OfficialEventTypeId==2).Where(e => e.StartDate.CompareTo(DateTime.Now) <= 0 && e.EndDate.CompareTo(DateTime.Now) >= 0).Select(e=>e.OfficialEventListId).FirstOrDefault();
            listProd = new CHomeFactory().rdnProd(_db.SubOfficialEventToProducts.Where(e => e.SubOfficialEventList.OfficialEventListId == FSevtID).Where(p => p.VerifyId==2).Where(e=>e.SubOfficialEventList.IsFreeDelivery==false).Select(p => p.Product).ToList());
            List<CShowFSItem> listFS = new CHomeFactory().toShowFSItem(listProd.ToList(), FSevtID);
            var webAds = _db.WebAds.Where(a => a.IsPublishing == true);
            List<OfficialEventList> NowEvent = new CHomeFactory().toGetEvent(_db.OfficialEventLists.Where(e => e.OfficialEventListId != 1).Where(e => e.OfficialEventTypeId == 1).Where(e => DateTime.Now.CompareTo(e.EndDate.AddDays(3)) < 0).ToList());
            
            if (DateTime.Now.ToString("tt") == "上午")
            {
                listFS = listFS.Where(p => p.product.ProductId % 2 == 0).Take(15).ToList();
            }
            else if (DateTime.Now.ToString("tt") == "下午")
            {
                listFS = listFS.Where(p => p.product.ProductId % 2 == 1).Take(15).ToList();
            }

            CHomeIndex home = new CHomeIndex()
            {
                lSmallType = _db.SmallTypes.Select(p => p).ToList(),
                lBigType = _db.BigTypes.Select(p => p).ToList(),
                cShowItem = listItem,
                WebADCarousel = webAds.Where(a => a.WebAdimageTypeId == 1 && a.IsPublishing).ToList(),
                WebADSmall = (new CHomeFactory()).toRndImg(webAds.Where(a => a.WebAdimageTypeId == 2 && a.IsPublishing).ToList()),
                WebADBig = (new CHomeFactory()).toRndImg(webAds.Where(a => a.WebAdimageTypeId == 3 && a.IsPublishing).ToList()),
                cShowBB = listBB,
                cShowFS = listFS,
                Event = NowEvent,
            };

            isExpo(listBB);
            return View(home);
        }

        public IActionResult FlashSales()
        {
            return View();
        }
        public IActionResult redirectProdLink(int id)
        {
            var a = _db.AdtoProducts.Where(a => a.IsSubActive && a.ProductId == id);
            if (a.Any()) {
                if (Request.Cookies["Click" +a.First().ProductId]!=null)
                {
                    return Json(0);
                }
                else
                {
                    TimeSpan TS = TimeSpan.FromMinutes(10);
                    Response.Cookies.Append("Click" + a.First().ProductId, "ClickClicked", new CookieOptions { MaxAge = TS, });
                    foreach(var ads in a)
                    {
                        ads.ClickTimes += 1;
                    }
                }
            }
            _db.SaveChanges();
            return Json(0);
        }
        private void isExpo(List<CShowBBItem> list)
        {
            if (list.Any())
            {
                foreach(var item in list)
                {
                    if (Request.Cookies["Expo"+item.product.ProductId] != null)
                    {
                        continue;
                    }
                    TimeSpan TS = TimeSpan.FromMinutes(10);
                    Response.Cookies.Append("Expo"+item.product.ProductId,"ExpoClicked",new CookieOptions { MaxAge=TS,});
                    List<AdtoProduct> p = _db.AdtoProducts.Where(p => p.ProductId == item.product.ProductId&&p.IsSubActive).ToList();
                    if (p.Any())
                    {
                        foreach(var ads in p)
                        {
                            ads.ExpoTimes += 1;
                        }
                    }
                    
                }
                _db.SaveChanges();
            }
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
            if (Request.Cookies["LayoutSearchKeyword"] == null)
            {
                if (!String.IsNullOrEmpty(key)) {
                    Response.Cookies.Append("LayoutSearchKeyword", key);
                    keywordList.Add(key);
                }
            }
            //if cookies 
            else
            {
                string keywordReq = Request.Cookies["LayoutSearchKeyword"];
                keywordList = (keywordReq.Split(",")).ToList();
                if (!String.IsNullOrEmpty(key))
                {
                    if (!keywordList.Contains(key)) { 
                        keywordList.Add(key);
                        if (keywordList.Count >5)
                        {
                            keywordList.RemoveAt(0);
                        }
                        keywordReq = String.Join(",", keywordList.ToArray());
                        Response.Cookies.Append("LayoutSearchKeyword", keywordReq);
                    }
                }
                keywordList.Reverse();
            }
            return Json(keywordList.ToArray());
        }

        public IActionResult ShoppingCartStockDisplay()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return Json(0);
            }
            MemberAccount acc = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            int CardCount = _db.OrderDetails.Where(o => o.Order.MemberId == acc.MemberId && o.Order.StatusId == 1).Count();
            
            if (CardCount == 0)
            {
                return Json(0);
            }
            else
            {
                return Json(CardCount);
            }
                
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
