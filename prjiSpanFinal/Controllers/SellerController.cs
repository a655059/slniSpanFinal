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
using prjiSpanFinal.ViewModels.Home;

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
        public IActionResult SortOrder(int sort, int tab ,int pages, int eachpage, string keyword, DateTime startdate, DateTime enddate)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            return Json(new OrderSortReq().SortTab(sort, tab, id, pages, eachpage, keyword, startdate, enddate.AddDays(1)));
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

            var paylist = _db.PaymentToSellers.Where(n => n.MemberId == id).Select(n => n.PaymentId).ToList();
            var mempay = _db.Payments.Where(n => paylist.Contains(n.PaymentId)).Select(n => n).ToList();

            var Category = _db.CustomizedCategories.Where(n => n.MemberId == id).Select(n => n.CustomizedCategoryName).ToList();
            var CustomizedCategoryID = _db.CustomizedCategories.Where(n => n.MemberId == id).Select(n => n.CustomizedCategoryId).ToList();
            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                smallType = smallType,
                Category= Category,
                CustomizedCategoryID= CustomizedCategoryID,
                memship = memship,
                shipID = shiperlist,
                PaymentID = paylist,
                mempay= mempay,
            };
            return View(x);
        }

        public IActionResult AD()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            CSellerADViewModel ViewModel = new CSellerADViewModel()
            {
                SellerProds = (new CHomeFactory()).toShowItem(_db.Products.Where(p => p.MemberId == id).ToList()),
            };
            return View(ViewModel);
        }
        public IActionResult getItem(int nowpage)
        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            List<CShowItem> res = (new CSellerADFactory()).fgetShowITem(_db.Products.Where(p => p.MemberId == id).ToList(), nowpage);
            return Json(res);
        }




        public void CreateSuccess(CSellerCreateToViewViewModel jsonString) //新增商品畫面成功
        {

            //HttpContext.Session.Remove(CSellerSessionViewModel.PRODUCT_ALL_DATA);
            //HttpContext.Session.SetString(CSellerSessionViewModel.PRODUCT_ALL_DATA, jsonString);
            //string jsonAll = HttpContext.Session.GetString(CSellerSessionViewModel.PRODUCT_ALL_DATA); //拿出session登入字串
            var result = jsonString;

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var regionId = _db.MemberAccounts.Where(n => n.MemberId == id).Select(n => n.RegionId).FirstOrDefault();
            int smID = result.smalltype;
            //var smID = _db.SmallTypes.FirstOrDefault(n => n.SmallTypeName == smName).SmallTypeId;

            Product product = new Product()
            {
                ProductName = result.ProductName,
                SmallTypeId = smID,
                MemberId = id,
                RegionId = regionId,
                Description = result.Description,
                ProductStatusId = 0,
                EditTime = DateTime.Now,
                CustomizedCategoryId = result.CategoryID,
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
                    Pic = result.暫存規格[i].BodyPicStr     //照片todo

                };
                _db.ProductDetails.Add(productDetail);
            }

            for (int i = 0; i < result.BodyPic.Count; i++)
            {
                ProductPic productPic = new ProductPic()
                {
                    Pic = result.BodyPic[i],
                    ProductId = Convert.ToInt32(product.ProductId)
                };
                _db.ProductPics.Add(productPic);
            }

            //if (!_db.CustomizedCategories.Where(n => n.MemberId == id).Select(n => n).Any())
            //{
            //    CustomizedCategory customizedCategory = new CustomizedCategory()
            //    {
            //        MemberId = id,
            //        CustomizedCategoryName = "未分類",
            //        SortNumber = 1
            //    };
            //    _db.CustomizedCategories.Add(customizedCategory);
            //}

            _db.SaveChanges();
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

            //連結小類別選項

        public IActionResult SmallType(int bigtype)
        {
            var a = _db.SmallTypes.Where(s => s.BigTypeId == bigtype).Select(s => new { id = s.SmallTypeId, name = s.SmallTypeName }).ToList();
            return Json(a);
        }



        public IActionResult Shipper()  //傳資料進去view
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var allpay = _db.Payments.Select(n => n).ToList();
            var allship = _db.Shippers.Select(n => n).ToList();

            CSellerPaymentToViewViewModel memdata = new CSellerPaymentToViewViewModel();
            memdata.PaymentName = new List<string>();
            memdata.ShipperName = new List<string>();
            memdata.PayCheckedOX = new List<int>();
            memdata.ShipCheckedOX = new List<int>();
            memdata.PaymentId = new List<int>();
            memdata.ShipperId = new List<int>();

            //客戶的payment
            var mempay = _db.PaymentToSellers.Where(n => n.MemberId == id).Select(n => n.PaymentId).ToList();

            for (int i = 0; i < allpay.Count; i++) //付款資料
            {
                memdata.PaymentName.Add(allpay[i].PaymentName);

                if (mempay.Count > i)
                {
                    memdata.PayCheckedOX.Add(1);
                    memdata.PaymentId.Add(mempay[i]);
                }
                else
                {
                    memdata.PayCheckedOX.Add(0);
                    memdata.PaymentId.Add(allpay[i].PaymentId);
                }
            }

            //客戶的shiper
            var memship = _db.ShipperToSellers.Where(n => n.MemberId == id).Select(n => n.ShipperId).ToList();
            for (int i = 0; i < allship.Count; i++)
            {
                memdata.ShipperName.Add(allship[i].ShipperName);
                //如果ShipperToSellers有資料
                if (memship.Count > i)
                {
                    memdata.ShipCheckedOX.Add(1);
                    memdata.ShipperId.Add(memship[i]);
                }
                else
                {
                    memdata.ShipCheckedOX.Add(0);
                    memdata.ShipperId.Add(allship[i].ShipperId);
                }
            }

            return View(memdata);
        }

        public void Shipperrequest(List<bool> payment, List<bool> shipper)
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
                int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount
                var pay = _db.PaymentToSellers.ToList();

                //===========PaymentToSellers=======================================

                for (int i=0;i < payment.Count;i++)
                {
                    if (payment[i] == true)
                    {
                        if(pay.Where(o=>o.MemberId== id && o.PaymentId == i + 1).Any())
                        {
                            continue;
                        }
                        else
                        {
                            _db.PaymentToSellers.Add(new PaymentToSeller() { MemberId = id, PaymentId = i + 1 });
                        }
                    }
                    else
                    {
                        if (pay.Where(o => o.MemberId == id && o.PaymentId == i + 1).Any())
                        {
                            _db.PaymentToSellers.Remove(_db.PaymentToSellers.Where(o => o.MemberId == id && o.PaymentId == i + 1).FirstOrDefault());
                        }
                    }
                }

                //===========ShipperToSellers=======================================

                var ship = _db.ShipperToSellers.ToList();

                for (int i = 0; i < shipper.Count; i++)
                {
                    if (shipper[i] == true)
                    {
                        if (ship.Where(o => o.MemberId == id && o.ShipperId == i + 1).Any())
                        {
                            continue;
                        }
                        else
                        {
                            _db.ShipperToSellers.Add(new ShipperToSeller() { MemberId = id, ShipperId = i + 1 });
                        }
                    }
                    else
                    {
                        if (ship.Where(o=>o.MemberId==id&&o.ShipperId==i+1).Any())
                        {
                            _db.ShipperToSellers.Remove(_db.ShipperToSellers.Where(o => o.ShipperId == i + 1 && o.MemberId == id).FirstOrDefault());
                        }                       
                    }
                }
                _db.SaveChanges();
            }
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

            var myproductlist = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2).Select(n => n.ProductId).ToList(); //賣家所有商品ID
            var q1 = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2).Select(n => n).ToList();//賣家所有商品
            var q2 = _db.ProductDetails.Where(n => myproductlist.Contains(n.ProductId)).Select(n => n).ToList(); //Contains是只把賣家所有商品ID全部挑出來
            List<string> listName = new List<string>();
            List<int> listproductId = new List<int>();
            List<int> liststatus = new List<int>();

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
                listproductId.Add(q1[i].ProductId);//把商品ID存進去
                liststatus.Add(q1[i].ProductStatusId);//把商品StatusId存進去
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
                    productId = listproductId,
                    Style = listStyle,
                    Quantity = listQty,
                    UnitPrice = listPrice,
                    Pic = listPic,
                    ProductStatusId=liststatus
                };
                    return View(x);


        }
        public IActionResult SelectIndex(string select)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件

            var myproductlist = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2&&n.ProductName.ToUpper().Contains(select.ToUpper())).Select(n => n.ProductId).ToList(); //賣家所有商品ID
            var q1 = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2 && n.ProductName.ToUpper().Contains(select.ToUpper())).Select(n => n).ToList();//賣家所有商品
            var q2 = _db.ProductDetails.Where(n => myproductlist.Contains(n.ProductId)).Select(n => n).ToList(); //Contains是只把賣家所有商品ID全部挑出來
            List<string> listName = new List<string>();
            List<int> listproductId = new List<int>();
            List<int> liststatus = new List<int>();

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
                listproductId.Add(q1[i].ProductId);//把商品ID存進去
                liststatus.Add(q1[i].ProductStatusId);//把商品StatusId存進去
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
                productId = listproductId,
                Style = listStyle,
                Quantity = listQty,
                UnitPrice = listPrice,
                Pic = listPic,
                ProductStatusId = liststatus
            };
            return Json(x);
        }





        public IActionResult EditProduct(int? id) //進入商品編輯頁
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }

            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var bigType = _db.BigTypes.Select(i => i.BigTypeName).ToList();
            var smallType = _db.SmallTypes.Select(i => i.SmallTypeName).ToList();
            var shiperlist = _db.ShipperToSellers.Where(n => n.MemberId == memId).Select(n => n.ShipperId).ToList();
            var memship = _db.Shippers.Where(n => shiperlist.Contains(n.ShipperId)).Select(s => s.ShipperName).ToList();
            var mempay = _db.PaymentToSellers.Where(n => n.MemberId == memId).Select(n => n.PaymentId).ToList();

            var pName = _db.Products.Where(n => n.ProductId == id).Select(n => n.ProductName).FirstOrDefault();
            //var pSmallTypeID = _db.Products.Where(n => n.ProductId == id).Select(n => n.SmallTypeId).FirstOrDefault();
            //var pSmallTypeName = _db.SmallTypes.Where(n => n.SmallTypeId == pSmallTypeID).Select(n => n.SmallTypeName).FirstOrDefault();
            //var pBigTypeID = _db.SmallTypes.Where(n => n.SmallTypeId == pSmallTypeID).Select(n => n.BigTypeId).FirstOrDefault();
            //var pBigTypeName = _db.BigTypes.Where(n => n.BigTypeId == pBigTypeID).Select(n => n.BigTypeName);
            var pDBtoPic = _db.ProductPics.Where(n => n.ProductId == id).Select(n => n.Pic).ToList();
            var pStyle = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Style).ToList();
            var PQuantity = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Quantity).ToList();
            var pUnitPrice = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.UnitPrice).ToList();
            var pBodyPic = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Pic).ToList();
            var pDescription = _db.Products.Where(n => n.ProductId == id).Select(n => n.Description).FirstOrDefault();
            
            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                smallType = smallType,
                memship = memship,
                shipID = shiperlist,
                PaymentID = mempay,

                ProductName = pName,
                //smalltype= pSmallTypeName,
                DBtoPic= pDBtoPic,
                Style = pStyle,
                Quantity = PQuantity,
                UnitPrice = pUnitPrice,
                BodyPic=pBodyPic,
                Description = pDescription
            };
            return View(x);            
        }

        public void EditProductSuccess(CSellerCreateToViewViewModel jsonString) //商品編輯儲存
        {
        

        }



            public IActionResult TakeDownProduct(CSellerNewIndexToViewViewModel select) //下架商品
        {
            var result = select;
            if (result != null)
            {
                Product product = _db.Products.Where(n => n.ProductId == result.productId[0]).FirstOrDefault();
                product.ProductStatusId = result.ProductStatusId[0];
                product.EditTime = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("NewIndex");
        }





        public IActionResult DeleteProduct(int? id)  //刪除商品
        {
            if (id != null)
            {
                Product product = _db.Products.Where(n => n.ProductId == id).FirstOrDefault();
                product.ProductStatusId = 2;
                _db.SaveChanges();
            }
            return RedirectToAction("NewIndex");
            //if (id != null)
            //{
            //    List<ProductPic> productPic = _db.ProductPics.Where(n => n.ProductId == id).Select(n => n).ToList();
            //    for (int i = 0; i < productPic.Count; i++)  //刪=ProductId全部照片
            //    {
            //        if (productPic[i] != null)  //如果有資料
            //        {
            //            _db.ProductPics.Remove(productPic[i]);
            //        }
            //    }

            //    List<PaymentToProduct> paymentToProducts = _db.PaymentToProducts.Where(n => n.ProductId == id).Select(n => n).ToList();
            //    for (int i = 0; i < paymentToProducts.Count; i++) //刪=ProductId全部金流
            //    {
            //        if (paymentToProducts[i] != null)
            //        {
            //            _db.PaymentToProducts.Remove(paymentToProducts[i]);
            //        }
            //    }

            //    List<ShipperToProduct> shipperToProducts = _db.ShipperToProducts.Where(n => n.ProductId == id).Select(n => n).ToList();
            //    for (int i = 0; i < shipperToProducts.Count; i++) //刪=ProductId全部物流
            //    {
            //        if (shipperToProducts[i] != null)
            //        {
            //            _db.ShipperToProducts.Remove(shipperToProducts[i]);
            //        }
            //    }

            //    List<ProductDetail> productDetails = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n).ToList();
            //    for (int i = 0; i < productDetails.Count; i++)  //刪=ProductId全部Details
            //    {
            //        if (productDetails[i] != null)
            //        {
            //            _db.ProductDetails.Remove(productDetails[i]);
            //        }
            //    }

            //    Product product = _db.Products.FirstOrDefault(n => n.ProductId == id);
            //    if (product != null)
            //    {
            //        _db.Products.Remove(product); //刪Product
            //    }

            //    _db.SaveChanges();
            //}

        }

        public IActionResult Coupon()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            string logindata = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(logindata).MemberId; //字串轉物件 MemberAccount

            var x = _db.Coupons.Where(n => n.MemberId == memId).Select(n => n).ToList();

            return View(x);
        }
        public IActionResult SelectCoupon(string select)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            string logindata = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(logindata).MemberId; //字串轉物件 MemberAccount

            var x = _db.Coupons.Where(n => n.MemberId == memId&&n.CouponName==select).Select(n => n).ToList();


            return Json(x);
        }

        public void Couponresponse(string jsonString)
        {
            CSellerCouponViewModel result = JsonSerializer.Deserialize<CSellerCouponViewModel>(jsonString);

            string logindata = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(logindata).MemberId; //字串轉物件 MemberAccount

            Coupon coupon = new Coupon()
            {
                CouponName = result.CouponName,
                CouponCode = result.CouponCode,
                StartDate = result.StartDate,
                ExpiredDate = result.ExpiredDate,
                Discount = result.Discount,
                MemberId = memId,
                ReceiveStartDate = result.StartDate,
                ReceiveEndDate = result.ExpiredDate,
                IsFreeDelivery = false,
                MinimumOrder = 0
            };
            _db.Coupons.Add(coupon);
            _db.SaveChanges();
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


