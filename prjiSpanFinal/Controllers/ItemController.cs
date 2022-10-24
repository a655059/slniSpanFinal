using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace prjiSpanFinal.Controllers
{
    public class ItemController : Controller
    {
        private IWebHostEnvironment _enviro;

        public ItemController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => i).FirstOrDefault();
                var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i).ToList();
                var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i.Pic).ToList();
                var ReportType = dbContext.ReportTypes.Select(i => i).ToList();
                var sellerProducts = dbContext.Products.Where(i => i.MemberId == product.MemberId && i.ProductId != product.ProductId).Select(i => i).ToList();
                List<CItemIndexSellerProductViewModel> sellerProductList = new List<CItemIndexSellerProductViewModel>();
                foreach (var p in sellerProducts)
                {
                    int productID = p.ProductId;
                    string productName = p.ProductName;
                    byte[] productPic = dbContext.ProductPics.Where(i => i.ProductId == p.ProductId).Select(i => i.Pic).FirstOrDefault();
                    var prices = dbContext.ProductDetails.Where(i => i.ProductId == p.ProductId).Select(i => i.UnitPrice).ToList();
                    decimal maxPrice = prices.Max();
                    decimal minPrice = prices.Min();
                    string price = "1";
                    if (maxPrice == minPrice)
                    {
                        price = $"${minPrice.ToString("0")}";
                    }
                    else
                    {
                        price = $"${minPrice.ToString("0")} - ${maxPrice.ToString("0")}";
                    }
                    var starCounts = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.ProductId == p.ProductId).Select(i => i.CommentStar);
                    double starCount = 0;
                    if (starCounts.Count() == 0)
                    {
                        starCount = 0;
                    }
                    else
                    {
                        starCount = starCounts.Average(i => i);
                    }
                    var salesVolumes = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == p.ProductId && i.Order.StatusId == 7).Select(i => i.Quantity);
                    int salesVolume = 0;
                    if (salesVolumes.Count() == 0)
                    {
                        salesVolume = 0;
                    }
                    else
                    {
                        salesVolume = salesVolumes.Sum(i => i);
                    }
                    CItemIndexSellerProductViewModel sellerProduct = new()
                    {
                        productID = productID,
                        productName = productName,
                        productPic = productPic,
                        price = price,
                        starCount = starCount,
                        salesVolume = salesVolume
                    };
                    sellerProductList.Add(sellerProduct);
                }
                sellerProductList = sellerProductList.OrderByDescending(i => i.starCount).ToList();
                CItemIndexViewModel itemIndex = new();
                itemIndex.product = product;
                itemIndex.productDetails = productDetails;
                itemIndex.productPics = productPics;
                itemIndex.sellerProducts = sellerProductList;
                itemIndex.ReportType = ReportType;
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                    //JsonSerializer serializer = new JsonSerializer();
                    //MemberAccount user = serializer.Deserialize<MemberAccount>(memberString);
                    MemberAccount user = System.Text.Json.JsonSerializer.Deserialize<MemberAccount>(memberString);
                    itemIndex.IsLogin = true;
                    itemIndex.user = user;
                }
                else
                {
                    itemIndex.IsLogin = false;
                    itemIndex.user = null;
                }
                return View(itemIndex);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult ReportCreate(Report d)
        {
            var db = (new iSpanProjectContext());
            if (d != null)
            {
                Report report = new()
                {
                    ReporterId = d.ReporterId,
                    ProductId = d.ProductId,
                    ReportTypeId = d.ReportTypeId,
                    Reason = d.Reason,
                    ReportPic = d.ReportPic,
                };
                db.Reports.Add(report);
                db.SaveChanges();
                return Json(new { Res = true, Msg = "成功" });
            }
            return Json(new { Res = false, Msg = "失敗" });
        }

        public IActionResult Description()
        {
            return PartialView("~/Views/Item/_ItemDescriptionPartial.cshtml");
        }
        public IActionResult Comment(int? id)
        {
            int buyerNum = (int)id;
            return PartialView("~/Views/Item/_ItemCommentPartial.cshtml", buyerNum);
        }
        public IActionResult BuyerCount(int? id)
        {
            return PartialView("~/Views/Item/_ItemBuyerCountPartial.cshtml", (int)id);
        }
    }
}
