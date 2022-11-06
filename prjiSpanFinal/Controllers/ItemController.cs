using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
                        ProductId = productID,
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
                    ReportStatusId = 1,
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

        public IActionResult AddMessage(int messageParentID, string message, int productID)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                int userID = JsonSerializer.Deserialize<MemberAccount>(memberString).MemberId;
                MessageBoard messageBoard = new MessageBoard
                {
                    MemberId = userID,
                    ProductId = productID,
                    Message = message,
                    Parent = messageParentID,
                    PostTime = DateTime.Now,
                };
                dbContext.MessageBoards.Add(messageBoard);
                dbContext.SaveChanges();
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public IActionResult LikeMessageBoard(int messageBoardID)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                int userID = JsonSerializer.Deserialize<MemberAccount>(memberString).MemberId;
                var isLike = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == messageBoardID && i.MemberId == userID).FirstOrDefault();
                if (isLike == null)
                {
                    MessageBoardLike messageBoardLike = new MessageBoardLike
                    {
                        MessageBoardId = messageBoardID,
                        MemberId = userID,
                    };
                    dbContext.MessageBoardLikes.Add(messageBoardLike);
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.MessageBoardLikes.Remove(isLike);
                    dbContext.SaveChanges();
                }
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public IActionResult DeleteMessage(int messageBoardID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            int userID = JsonSerializer.Deserialize<MemberAccount>(memberString).MemberId;
            var messageBoardLikes = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == messageBoardID);
            foreach (var a in messageBoardLikes)
            {
                dbContext.MessageBoardLikes.Remove(a);
            }
            dbContext.SaveChanges();
            var messageBoard = dbContext.MessageBoards.Where(i => i.MessageBoardId == messageBoardID).FirstOrDefault();
            dbContext.MessageBoards.Remove(messageBoard);
            dbContext.SaveChanges();
            return Content("1");
        }

        public IActionResult ShowMessageBoard(int productID)
        {
            return ViewComponent("ShowMessageBoard", productID);
        }

        public IActionResult BiddingItemUpload()
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var bigTypes = dbContext.BigTypes.ToList();
            var smallTypes = dbContext.SmallTypes.Where(i => i.BigTypeId == bigTypes[0].BigTypeId).ToList();
            CBiddingItemUploadViewModel x = new CBiddingItemUploadViewModel
            {
                bigTypes = bigTypes,
                smallTypes = smallTypes,
            };
            return View(x);
        }
        
        public IActionResult GetSmallType(int bigTypeID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var smallTypes = dbContext.SmallTypes.Where(i => i.BigTypeId == bigTypeID).ToList();
            return Json(smallTypes);
        }

        public IActionResult BiddingItemSave(CBiddingItemSaveViewModel x, List<IFormFile> itemPhotos)
        {
            decimal startPrice = 0;
            int stepPrice = 0;
            if (Decimal.TryParse(x.startPrice, out startPrice) && Int32.TryParse(x.stepPrice, out stepPrice))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount seller = JsonSerializer.Deserialize<MemberAccount>(memberString);
                string productName = x.itemName;
                int bigTypeID = x.bigType;
                int smallTypeID = x.smallType;
                Product product = new Product
                {
                    ProductName = productName,
                    SmallTypeId = smallTypeID,
                    MemberId = seller.MemberId,
                    RegionId = seller.RegionId,
                    Description = x.description,
                    ProductStatusId = 3,
                    EditTime = DateTime.Now,
                    CustomizedCategoryId = 1
                };
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                int newProductID = dbContext.Products.Where(i => i.MemberId == seller.MemberId && i.ProductName == productName).OrderByDescending(i => i.ProductId).Select(i => i.ProductId).FirstOrDefault();
                ProductDetail productDetail = new ProductDetail
                {
                    ProductId = newProductID,
                    Style = "樣式一",
                    Quantity = 1,
                    UnitPrice = startPrice,
                };
                dbContext.ProductDetails.Add(productDetail);
                dbContext.SaveChanges();
                if (itemPhotos.Count > 0)
                {
                    foreach (var a in itemPhotos)
                    {
                        if (a.Length > 0)
                        {
                            byte[] fileByte;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                a.CopyTo(ms);
                                fileByte = ms.GetBuffer();
                            }
                            ProductPic productPic = new ProductPic
                            {
                                ProductId = newProductID,
                                Pic = fileByte,
                            };
                            dbContext.ProductPics.Add(productPic);
                        }
                    }
                    dbContext.SaveChanges();
                }
                int newProductDetailID = dbContext.ProductDetails.Where(i => i.ProductId == newProductID).OrderByDescending(i => i.ProductDetailId).Select(i => i.ProductDetailId).FirstOrDefault();



                Bidding bidding = new Bidding
                {
                    ProductDetailId = newProductDetailID,
                    StartTime = Convert.ToDateTime(x.startDate.ToString("yyyy-MM-dd") + " " + x.startTime.TimeOfDay),
                    EndTime = Convert.ToDateTime(x.endDate.ToString("yyyy-MM-dd") + " " + x.endTime.TimeOfDay),
                    StartPrice = Convert.ToInt32(startPrice),
                    StepPrice = stepPrice
                };
                dbContext.Biddings.Add(bidding);
                dbContext.SaveChanges();

                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        public IActionResult BiddingItemHome()
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            List<int> biddingIDs = dbContext.Biddings.Where(i => i.ProductDetail.Product.ProductStatusId == 4).Select(i => i.BiddingId).ToList();
            return View(biddingIDs);
        }
        public IActionResult BiddingIndex(int id)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var infos = dbContext.Biddings.Where(i => i.BiddingId == id).Select(i => new CBiddingItemIndexViewModel
            {
                bidding = i,
                productDetail = i.ProductDetail,
                product = i.ProductDetail.Product,
                seller = i.ProductDetail.Product.Member,
                smallType = i.ProductDetail.Product.SmallType,
                bigType = i.ProductDetail.Product.SmallType.BigType,
                region = i.ProductDetail.Product.Member.Region,
                country = i.ProductDetail.Product.Member.Region.Country,
            }).FirstOrDefault();
            infos.productPics = dbContext.ProductPics.Where(i => i.ProductId == infos.product.ProductId).Select(i => i.Pic).ToList();
            infos.biddingDetailWithMember = dbContext.BiddingDetails.Where(i => i.BiddingId == infos.bidding.BiddingId).OrderByDescending(i=>i.Price).Select(i => new CBiddingDetailWithMemberViewModel
            {
                biddingDetail = i,
                member = i.Member
            }).ToList();
            infos.user = null;
            infos.isLike = false;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                infos.user = JsonSerializer.Deserialize<MemberAccount>(memberString);
                if (dbContext.Likes.Any(i => i.ProductId == infos.product.ProductId && i.MemberId == infos.user.MemberId))
                {
                    infos.isLike = true;
                }
            }
            infos.sellerShippers = dbContext.ShipperToSellers.Where(i => i.MemberId == infos.seller.MemberId).Select(i => i.Shipper).ToList();
            infos.sellerPayments = dbContext.PaymentToSellers.Where(i => i.MemberId == infos.seller.MemberId).Select(i => i.Payment).ToList();
            infos.sellerProductCount = dbContext.Products.Where(i => i.MemberId == infos.seller.MemberId).Count();
            
            var sellerComments = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.Product.MemberId == infos.seller.MemberId).ToList();
            infos.sellerCommentCount = 0;
            infos.avgSellerCommentStar = 0;
            if (sellerComments.Count > 0)
            {
                infos.sellerCommentCount = sellerComments.Count;
                infos.avgSellerCommentStar = sellerComments.Average(i => i.CommentStar);
            }
            
            return View(infos);
        }

        public IActionResult Bid(int biddingID, string biddingType, int price, int topPrice)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                iSpanProjectContext dbContext = new iSpanProjectContext();
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                MemberAccount user = JsonSerializer.Deserialize<MemberAccount>(memberString);
                BiddingDetail biddingDetail = new BiddingDetail
                {
                    BiddingId = biddingID,
                    MemberId = user.MemberId,
                    Price = price
                };
                dbContext.BiddingDetails.Add(biddingDetail);
                dbContext.SaveChanges();
                if (biddingType == "autoBidding")
                {
                    AutoBidding autoBidding = new AutoBidding
                    {
                        BiddingId = biddingID,
                        MemberId = user.MemberId,
                        TopPrice = topPrice
                    };
                    dbContext.AutoBiddings.Add(autoBidding);
                    dbContext.SaveChanges();
                }
                var bidding = dbContext.BiddingDetails.Where(i => i.BiddingId == biddingID).OrderByDescending(i => i.Price).Select(i => new
                {
                    biddingDetail = i,
                    member = i.Member,
                });
                int biddingCount = bidding.Count();
                string topMember = bidding.FirstOrDefault().member.MemberAcc;

                List<string> result = new List<string> { bidding.FirstOrDefault().biddingDetail.Price.ToString(), biddingCount.ToString(), topMember };

                return Json(result);
            }
            else
            {
                return Content("0");
            }
        }
        public IActionResult ShowBiddingDetail(int id)
        {
            return ViewComponent("BiddingDetail", id);
        }
    }
}
