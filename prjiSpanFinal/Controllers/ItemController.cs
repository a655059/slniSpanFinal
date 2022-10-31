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
                iSpanProjectContext dbContext = new();
                var product = dbContext.Products.Where(i => i.ProductId == id).Select(i => new
                {
                    product = i,
                    smallType = i.SmallType,
                    bigType = i.SmallType.BigType,
                    seller = i.Member,
                    sellerRegion = i.Member.Region.Country.CountryName + i.Member.Region.RegionName,
                }).FirstOrDefault();
                var productDetails = dbContext.ProductDetails.Where(i => i.ProductId == id).Select(i => i).ToList();
                var productPics = dbContext.ProductPics.Where(i => i.ProductId == id).Select(i => i.Pic).ToList();
                var sellerProductIDList = dbContext.Products.Where(i => i.MemberId == product.product.MemberId).Select(i => i.ProductId).ToList();
                List<CSellerShipperViewModel> sellerShipper = dbContext.ShipperToSellers.Where(i => i.MemberId == product.product.MemberId).Select(i => new CSellerShipperViewModel
                {
                    shipper = i.Shipper.ShipperName,
                    fee = i.Shipper.Fee,
                }).ToList();
                List<CSellerPaymentViewModel> sellerPayment = dbContext.PaymentToSellers.Where(i => i.MemberId == product.product.MemberId).Select(i => new CSellerPaymentViewModel
                {
                    payment = i.Payment.PaymentName,
                    fee = i.Payment.Fee,
                }).ToList();
                int commentCount = 0;
                double avgCommentStar = 0;
                int sellerCommentCount = 0;
                double avgSellerCommentStar = 0;
                var comments = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.ProductId == id).ToList();
                if (comments.Count() > 0)
                {
                    commentCount = comments.Count;
                    avgCommentStar = comments.Average(i => i.CommentStar);
                }
                int salesVolume = 0;
                int buyerCount = 0;
                var sales = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == id && i.Order.StatusId == 7);
                if (sales.Count() > 0)
                {
                    salesVolume = sales.Sum(i => i.Quantity);
                    buyerCount = dbContext.OrderDetails.Where(i => i.ProductDetail.ProductId == id && i.Order.StatusId == 7).Select(i => i.Order.MemberId).Distinct().Count();
                }
                var sellerComments = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.Product.MemberId == product.seller.MemberId);
                if (sellerComments.Count() > 0)
                {
                    sellerCommentCount = sellerComments.Count();
                    avgSellerCommentStar = sellerComments.Average(i => i.CommentStar);
                }

                var ReportType = dbContext.ReportTypes.Select(i => i).ToList();
                CItemIndexViewModel itemIndex = new();
                itemIndex.product = product.product;
                itemIndex.bigType = product.bigType;
                itemIndex.smallType = product.smallType;
                itemIndex.productDetails = productDetails;
                itemIndex.productPics = productPics;
                itemIndex.sellerProductIDs = sellerProductIDList;
                itemIndex.ReportType = ReportType;
                itemIndex.seller = product.seller;
                itemIndex.sellerRegion = product.sellerRegion;
                itemIndex.sellerShipper = sellerShipper;
                itemIndex.sellerPayment = sellerPayment;
                itemIndex.avgCommentStar = avgCommentStar;
                itemIndex.commentCount = commentCount;
                itemIndex.salesVolume = salesVolume;
                itemIndex.buyerCount = buyerCount;
                itemIndex.avgSellerCommentStar = avgSellerCommentStar;
                itemIndex.sellerCommentCount = sellerCommentCount;
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                    MemberAccount user = System.Text.Json.JsonSerializer.Deserialize<MemberAccount>(memberString);
                    var like = dbContext.Likes.Where(i => i.MemberId == user.MemberId && i.ProductId == id).Select(i => i).FirstOrDefault();
                    if (like != null)
                    {
                        itemIndex.Islike = true;
                    }
                    else
                    {
                        itemIndex.Islike = false;
                    }
                    itemIndex.IsLogin = true;
                    itemIndex.user = user;
                }
                else
                {
                    itemIndex.IsLogin = false;
                    itemIndex.user = null;
                    itemIndex.Islike = false;
                }
                return View(itemIndex);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddItemLike(int? memberID, int productID)
        {
            if (memberID <= 0)
            {
                return Content("0");
            }
            else
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                var like = dbContext.Likes.Where(i => i.MemberId == memberID && i.ProductId == productID).Select(i => i).FirstOrDefault();
                if (like != null)
                {
                    dbContext.Likes.Remove(like);
                    dbContext.SaveChanges();
                    return Content("2");
                }
                else
                {
                    Like newLike = new Like
                    {
                        MemberId = (int)memberID,
                        ProductId =productID,
                    };
                    dbContext.Likes.Add(newLike);
                    dbContext.SaveChanges();
                    return Content("1");
                }
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
                    ReportStatusId=1,
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
