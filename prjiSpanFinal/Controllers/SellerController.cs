using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using prjiSpanFinal.Models.OrderReq;
using prjiSpanFinal.ViewModels.Order;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;

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
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            return View();
        }
        public IActionResult SortOrder(int sort, int tab)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            return Json(new OrderSortReq().SortTab(sort, tab, id));

        }
        public IActionResult SearchOrder(string keyword, DateTime startdate, DateTime enddate)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            return Json(new OrderSortReq().SearchOrder(keyword, startdate.AddDays(1), enddate.AddDays(1), id));

        }
        public IActionResult WriteComment(int id, byte star, string keyword)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            if (keyword == null)
            {
                keyword = "";
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            CommentForCustomer a = new CommentForCustomer() { Comment = keyword, CommentStar = star, CommentTime = DateTime.Now, OrderId = id };
            dbcontext.CommentForCustomers.Add(a);
            if (!dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault().OrderDetails.Select(o => o.Comments.Count).ToList().Contains(0))
            {
                Order b = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
                b.StatusId = 7;
            }
            dbcontext.SaveChanges();
            return Json("1");
        }

        public IActionResult WriteShipping(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }

            iSpanProjectContext dbcontext = new iSpanProjectContext();
            Order b = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
            b.StatusId = 4;
            b.ShippingDate = DateTime.Now;
            var q = dbcontext.OrderDetails.Where(o => o.OrderId == id);
            foreach (var item in q)
            {
                item.ShippingStatusId = 2;
            }
            dbcontext.SaveChanges();
            return Json("1");
        }

        public IActionResult GetReturn(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }

            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Arguments.Where(a => a.OrderId == id).OrderBy(o => o.ArgumentId).Select(o => new OrderReturnViewModel()
            {
                ReasonName = o.ArgumentReason.ArgumentReasonName,
                ReasonText = o.ReasonText,
                TypeName = o.ArgumentType.ArgumentTypeName,
                pics = o.ArguePics.Select(o => o.ArguePic1).ToList(),
                TypeID = o.ArgumentTypeId,
            }).LastOrDefault());
        }

        public IActionResult AcceptReturn(int id, int type)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }

            iSpanProjectContext dbcontext = new iSpanProjectContext();
            if (type == 1)
            {
                Order a = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
                int thisorderid = id;
                int countofods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid).Count();
                int countof45ods = dbcontext.OrderDetails.Where(o => o.OrderId == thisorderid && (o.ShippingStatusId == 4 || o.ShippingStatusId == 5)).Count();
                Order b = dbcontext.Orders.FirstOrDefault(o => o.OrderId == thisorderid);
                if (countof45ods == countofods)
                {
                    b.StatusId = 5;
                }
                else
                {
                    b.StatusId = 4;
                }
            }
            //o => o.PaymentFee + o.ShipperFee + o.Quantity.Select((Value, index) => Value * Convert.ToInt32(o.Unitprice[index])).Sum()
            else if (type == 2)
            {
                Order a = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
                a.StatusId = 7;
                MemberAccount b = dbcontext.Orders.Where(o => o.OrderId == id).Select(o => o.Member).FirstOrDefault();
                b.Balance += dbcontext.Orders.Where(o => o.OrderId == id).Select(o => o.Payment.Fee + o.Shipper.Fee + o.OrderDetails.Select(o => Convert.ToInt32(o.Unitprice) * o.Quantity).Sum()).FirstOrDefault();
                if (dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault().Coupon.IsFreeDelivery == true)
                {
                    b.Balance -= dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault().Shipper.Fee;
                }
            }

            dbcontext.SaveChanges();
            return Json("1");
        }

        public IActionResult Create()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var bigType = _db.BigTypes.Select(i => i.BigTypeName).ToList();
            var smallType = _db.SmallTypes.Select(i => i.SmallTypeName).ToList();

            var shiperlist = _db.ShipperToSellers.Where(n => n.MemberId == id).Select(n => n.ShipperId).ToList();
            var memship = _db.Shippers.Where(n => shiperlist.Contains(n.ShipperId)).Select(s => s.ShipperName).ToList();

            var mempay = _db.PaymentToSellers.Where(n => n.MemberId == id).Select(n => n.PaymentId).ToList();

            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                smallType = smallType,
                memship = memship,
                shipID = shiperlist,
                PaymentID = mempay
            };
            return View(x);
        }


        public IActionResult AD(string jsonString)
        {
            return PartialView(jsonString);
        }

        public void CreateSuccess(CSellerCreateToViewViewModel jsonString)//新增商品所有資料session  //新增成功畫面
        {

            //HttpContext.Session.Remove(CSellerSessionViewModel.PRODUCT_ALL_DATA);
            //HttpContext.Session.SetString(CSellerSessionViewModel.PRODUCT_ALL_DATA, jsonString);
            //string jsonAll = HttpContext.Session.GetString(CSellerSessionViewModel.PRODUCT_ALL_DATA); //拿出session登入字串
            var result = jsonString;

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var regionId = _db.MemberAccounts.Where(n => n.MemberId == id).Select(n => n.RegionId).FirstOrDefault();

            //if (jsonString.HeadPic != null)
            //{
            //    byte[] imgByte = null;
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        File1.CopyTo(memoryStream);
            //        imgByte = memoryStream.ToArray();
            //    }
            //    jsonString.HeadPic = imgByte;
            //}

            Product product = new Product()
            {
                ProductName = result.ProductName,
                SmallTypeId = Convert.ToInt32(result.smalltype),
                MemberId = id,
                RegionId = regionId,
                Description = result.Description,
                ProductStatusId = 0,
                EditTime = DateTime.Now,
                CustomizedCategoryId = 1
            };
            _db.Products.Add(product);
            _db.SaveChanges();

            var productId = _db.Products.Select(n => n).FirstOrDefault();

            for (int i = 0; i < result.暫存規格.Count; i++)
            {
                ProductDetail productDetail = new ProductDetail()
                {
                    ProductId = Convert.ToInt32(product.ProductId),
                    Style = result.暫存規格[i].StyleStr,
                    Quantity = Convert.ToInt32(result.暫存規格[i].QuantityStr),
                    UnitPrice = Convert.ToInt32(result.暫存規格[i].UnitPriceStr),
                    //Pic=result.暫存規格[i].BodyPicStr     //照片todo
                };
                _db.ProductDetails.Add(productDetail);
            }

            for (int i = 0; i < result.ShiperID.Count; i++)
            {
                ShipperToProduct shipperToProduct = new ShipperToProduct()
                {
                    ShipperId = Convert.ToInt32(result.ShiperID[i]),
                    ProductId = Convert.ToInt32(product.ProductId)
                };
                _db.ShipperToProducts.Add(shipperToProduct);
            }

            for (int i = 0; i < result.PaymentID.Count; i++)
            {
                PaymentToProduct paymentToProduct = new PaymentToProduct()
                {
                    PaymentId = Convert.ToInt32(result.PaymentID[i]),
                    ProductId = Convert.ToInt32(product.ProductId)
                };
                _db.PaymentToProducts.Add(paymentToProduct);
            }
            _db.SaveChanges();
        }



        //連結小類別選項

        public IActionResult SmallType(string bigtype)
        {
            var smalltype = _db.SmallTypes.Where(a => a.BigType.BigTypeName == bigtype).Select(a => a.SmallTypeName).Distinct();
            return Json(smalltype);
        }

        public IActionResult OrderDetail(int id)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            var vm = dbcontext.Orders.Where(o => o.OrderId == id).Select(o => new OrderDetailViewModel()
            {
                OrderId = o.OrderId,
                SellerAcc = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.MemberAcc,
                SellerEmail = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Email,
                SellerName = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Name,
                SellerPhone = o.OrderDetails.FirstOrDefault().ProductDetail.Product.Member.Phone,
                BuyerAcc = o.Member.MemberAcc,
                BuyerEmail = o.Member.Email,
                BuyerName = o.Member.Name,
                BuyerPhone = o.Member.Phone,
                OrderDatetime = o.OrderDatetime,
                RecieveAdr = o.RecieveAdr,
                CouponName = o.Coupon.CouponName,
                IsFreeDelivery = o.Coupon.IsFreeDelivery,
                OrderStatusName = o.Status.OrderStatusName,
                ShipperStatusId = o.StatusId,
                ShipperName = o.Shipper.ShipperName,
                ShipperFee = o.Shipper.Fee,
                ShipperPhone = o.Shipper.Phone,
                PaymentDate = o.PaymentDate,
                ShippingDate = o.ShippingDate,
                ReceiveDate = o.ReceiveDate,
                PaymentName = o.Payment.PaymentName,
                PaymentFee = o.Payment.Fee,
                OrderMessage = o.OrderMessage,
                OrderDetailId = o.OrderDetails.Select(o => o.OrderDetailId).ToList(),
                Quantity = o.OrderDetails.Select(o => o.Quantity).ToList(),
                ShipStatusName = o.OrderDetails.Select(o => o.ShippingStatus.ShipStatusName).ToList(),
                Unitprice = o.OrderDetails.Select(o => o.Unitprice).ToList(),
                Style = o.OrderDetails.Select(o => o.ProductDetail.Style).ToList(),
                Pic = o.OrderDetails.Select(o => o.ProductDetail.Pic).ToList(),
                ProductName = o.OrderDetails.Select(o => o.ProductDetail.Product.ProductName).ToList(),
            }).FirstOrDefault();
            return View(vm);
        }


        public IActionResult Shipper()  //傳資料進去view
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var payname = _db.Payments.Select(n => n).ToList();
            //var memberid = _db.PaymentToProducts.Select(n => n.ProductId);
            //var productid = _db.PaymentToSellers.Select(n => n.MemberId);
            var shipname = _db.Shippers.Select(n => n).ToList();

            List<int> PaymentId = new List<int>();
            List<string> PaymentName = new List<string>();
            List<int> ShipperId = new List<int>();
            List<string> ShipperName = new List<string>();
            List<CSellerPaymentToViewViewModel> SellerPaymentToView = new List<CSellerPaymentToViewViewModel>();

            for (int i = 0; i < payname.Count; i++) //付款資料
            {
                PaymentName.Add(payname[i].PaymentName);
                PaymentId.Add(payname[i].PaymentId);
            }

            var shiperlist = _db.ShipperToSellers.Where(n => n.MemberId == id).Select(n => n.ShipperId).ToList();

            for (int i = 0; i < shipname.Count; i++)//物流資料
            {
                CSellerPaymentToViewViewModel SellerPayment1 = new CSellerPaymentToViewViewModel();

                ShipperName.Add(shipname[i].ShipperName);
                ShipperId.Add(shipname[i].ShipperId);

                //如果ShipperToSellers有資料
                if (shiperlist.Count > 0)
                {
                    SellerPayment1.ShipperIdToShip = shipname[i].ShipperId;//把ShipperToSellers ID資料帶入

                    if (shiperlist.Any(n => n == shipname[i].ShipperId))//有資料的話Checked給1 沒有給0
                    {
                        SellerPayment1.CheckedOX = 1;// 有資料的話Checked打開
                    }
                    else
                    {
                        SellerPayment1.CheckedOX = 0;// 有資料的話Checked關閉
                    }
                }
                SellerPaymentToView.Add(SellerPayment1);
            }

            CSellerPaymentToViewViewModel x = new CSellerPaymentToViewViewModel
            {
                PaymentId = PaymentId,
                PaymentName = PaymentName,
                ShipperId = ShipperId,
                ShipperName = ShipperName,
                x = SellerPaymentToView //對應到客戶的shipID
            };

            return View(x);
        }

        public IActionResult Shipperrequest(string jsonString)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
                int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

                List<CSellerViewToPaymentViewModel> list = JsonSerializer.Deserialize<List<CSellerViewToPaymentViewModel>>(jsonString);//字串轉物件

                var PID = _db.PaymentToSellers.Where(n => n.MemberId == id).Select(n => n); //找出登入者PaymentToSellers的所有資料
                if (PID != null)//如果有資料就逐筆刪除
                {
                    foreach (var p in PID)
                    {
                        _db.PaymentToSellers.Remove(p);
                    }

                    List<string> 金流 = list[0].選取的;  //list[0] 是金流ID
                    foreach (var i in 金流)
                    {
                        PaymentToSeller paymentToSeller = new PaymentToSeller()
                        {
                            MemberId = id,
                            PaymentId = Convert.ToInt32(i)
                        };
                        _db.PaymentToSellers.Add(paymentToSeller);
                    }
                }

                var MID = _db.ShipperToSellers.Where(n => n.MemberId == id).Select(n => n);//找出登入者ShipperToSellers的所有資料
                if (MID != null)  //如果有資料就逐筆刪除
                {
                    foreach (var p in MID)
                    {
                        _db.ShipperToSellers.Remove(p);
                    }

                    List<string> 物流 = list[1].選取的;  //list[1] 是物流ID
                    foreach (var i in 物流)
                    {
                        ShipperToSeller shipperToSeller = new ShipperToSeller()
                        {
                            MemberId = id,
                            ShipperId = Convert.ToInt32(i)
                        };
                        _db.ShipperToSellers.Add(shipperToSeller);
                    }
                }

                _db.SaveChanges();
                return Content("1");  //1=true
            }
            else
                return Content("0");   //0=false
        }




        public IActionResult Center()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            return View();
        }






        public IActionResult NewIndex()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件

            var myproductlist = _db.Products.Where(n => n.MemberId == id).Select(n => n.ProductId).ToList(); //賣家所有商品ID
            var q1 = _db.Products.Where(n => n.MemberId == id).Select(n => n).ToList();//賣家所有商品
            var q2 = _db.ProductDetails.Where(n => myproductlist.Contains(n.ProductId)).Select(n => n).ToList(); //Contains是只把賣家所有商品ID全部挑出來
            List<string> listName = new List<string>();

            List<List<string>> listStyle = new List<List<string>>();
            //  List<一個商品有兩個Style>  =   <一個商品有兩個Style>  <一個商品有兩個Style>  <一個商品有兩個Style>

            List<List<int>> listQty = new List<List<int>>();
            List<List<decimal>> listPrice = new List<List<decimal>>();
            List<List<byte[]>> listPic = new List<List<byte[]>>();

            for (int i = 0; i < myproductlist.Count; i++)  //外迴圈把所有商品列出來存進List
            {
                List<string> sublistStyle = new List<string>();
                List<int> sublistQty = new List<int>();
                List<decimal> sublistPrice = new List<decimal>();
                List<byte[]> sublistPic = new List<byte[]>();

                listName.Add(q1[i].ProductName);//把商品名稱存進去
                var detail = q2.Where(p => p.ProductId == q1[i].ProductId).ToList();//找出所有同ID商品的資料

                for (int j = 0; j < detail.Count; j++)//內迴圈把同ID商品所有Style和相關資料列出來存進List
                {
                    sublistStyle.Add(detail[j].Style);
                    sublistQty.Add(detail[j].Quantity);
                    sublistPrice.Add(detail[j].UnitPrice);
                    sublistPic.Add(detail[j].Pic);
                }
                listQty.Add(sublistQty);  //把所有商品的數量存進去
                listPrice.Add(sublistPrice);
                listPic.Add(sublistPic);
                listStyle.Add(sublistStyle);
            }

            CSellerNewIndexToViewViewModel x = new CSellerNewIndexToViewViewModel
            {
                productName = listName,
                Style = listStyle,
                Quantity = listQty,
                UnitPrice = listPrice,
                Pic = listPic
            };

            return View(x);
        }


        public IActionResult Coupon(string jsonString)
        {
            int id = 1;
            var x = _db.Coupons.Where(n => n.MemberId == id).Select(n => n).ToList();

            return View(x);
        }

        public void Couponresponse(string jsonString)
        {
        }

        public IActionResult seller跑條(int page)
        {
            return ViewComponent("seller跑條", page);
        }

        public IActionResult Event()
        {
            var E = _db.SubOfficialEventLists.Select(i => i).ToList();
            var P = _db.Products.Select(i => i).ToList();
           
            var OE = _db.OfficialEventLists.Select(i => i).ToList();
            List<CSubEventToProductViewModel> list = new();
            foreach (var e in E)
            {
                var A = from a in OE
                        where a.OfficialEventListId == e.OfficialEventListId
                        select a;
                CSubEventToProductViewModel C = new()
                {
                    Products = P,
                    SubOfficialEventID = e,
                    OfficialEventList = A.First(),
                };
                list.Add(C);
            }

            return View(list);
        }
    }
}


