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
            var aaaaa = dbcontext.Orders.Where(o => o.OrderId == id).Select(o=>o.OrderDetails.FirstOrDefault().Comments.Count).ToList();
            var bbbbb = dbcontext.Orders.Where(o => o.OrderId == id).Select(o => o.OrderDetails.Count).FirstOrDefault();
            if (aaaaa.Count == bbbbb)
            {
                if(!aaaaa.Contains(0))
                {
                    Order b = dbcontext.Orders.Where(o => o.OrderId == id).FirstOrDefault();
                    b.FinishDate = DateTime.Now;
                    b.StatusId = 7;
                }
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

        public IActionResult GetMemSta()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //&& o.StatusId == tab
            {
                return RedirectToAction("Login", "Member");
            }
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.MemberAccounts.Where(m=>m.MemberId == id).First().MemStatusId == 1);
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

            var ADtypeId = _db.Adtypes.Select(n => n.AdTypeId).ToList(); //所有方案ID
            var ADtype = _db.Adtypes.Select(n => n).ToList();    //所有方案
            var ADId = _db.Ads.Where(n => ADtypeId.Contains(n.AdTypeId)).Select(n => n).ToList(); //找出方案裡的內容

            List<string> listName = new List<string>();
            List<string> listDes = new List<string>();

            List<List<decimal>> listFee = new List<List<decimal>>();
            //  List<一個商品有兩個Style>  =   <一個商品有兩個Style>  <一個商品有兩個Style>  <一個商品有兩個Style>
            List<List<int>> listPeriod = new List<List<int>>();
            List<List<int>> listAdId = new List<List<int>>();

            for(int i =0; i< ADtypeId.Count; i++)
            {
                List<decimal> subFee = new List<decimal>();
                List<int> subPeriod = new List<int>();
                List<int> subAdId = new List<int>();

                listName.Add(ADtype[i].AdType1);
                listDes.Add(ADtype[i].AdTyepDescription);

                var ADprogram = ADId.Where(n => n.AdTypeId == ADtypeId[i]).Select(n => n).ToList();
                for (int j=0;j< ADprogram.Count; j++)
                {
                    subFee.Add(ADprogram[j].AdFee);
                    subPeriod.Add(ADprogram[j].AdPeriod);
                    subAdId.Add(ADprogram[j].AdId);
                }
                listFee.Add(subFee);
                listPeriod.Add(subPeriod);
                listAdId.Add(subAdId);
            }


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
                ADName= listName,
                AdTyepDescription= listDes,
                ADId= listAdId,
                AdPeriod= listPeriod,
                AdFee= listFee
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
            List<CADProdViewModel> res = new CSellerADFactory().fgetShowITem(_db.Products.Where(p => p.MemberId == id).Where(p=>p.ProductStatusId!=1||p.ProductStatusId!=2).ToList(), nowpage);
            return Json(res);
        }
        public IActionResult ADshowCheckItem(int itemID)
        {
            CShowItem res = new CSellerADFactory().fgetCheckedshowItem(_db.Products.Where(p => p.ProductId == itemID).ToList());
            return Json(res);
        }
        public IActionResult getAD(int ADfilter)
        {
            List<CADeffectViewmodel> res = new CSellerADFactory().fgetAD(ADfilter);

            return Json(res);
        }
        public IActionResult getResult(int[] ADIDs)
        {
            List<CADeffectViewmodel> res = new CSellerADFactory().fgetResult(_db.Ads.ToList(), ADIDs);
            return Json(res);
        }
        public IActionResult getADfilter()
        {
            List<Adtype> filters = _db.Adtypes.OrderBy(p=>p.AdTypeId).ToList();
            return Json(filters);
        }
        public IActionResult getSubList(int[] filter1,int[] filter2, int Sort, int page, string key)
        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            List<CADSubviewmodel> res = new CSellerADFactory().fgetSubList(id);
            res = new CSellerADFactory().fgetSubList(res, key, filter1, filter2, Sort, page);

            return Json(res);
        }
        public IActionResult saveADSubs(int itemID, int[] ADIDs, string ADtext)

        {
            int id = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            MemberAccount acc = _db.MemberAccounts.Where(a => a.MemberId == id).FirstOrDefault();
            
            List<int> result = new List<int>();
            int cost = 0;
            int balance = acc.Balance;

            foreach(var ads in ADIDs)
            {
                if (_db.AdtoProducts.Where(p => p.ProductId == itemID).Where(p => p.IsSubActive == true).Where(a => Math.Truncate(Convert.ToDecimal((a.AdId - 1) / 3)) == Math.Truncate(Convert.ToDecimal(ads - 1) / 3)).Any())
                {
                    result = new List<int> { 2, Array.IndexOf(ADIDs, ads)+1 };
                    return Json(result);
                }
            }

            //=======
            foreach (var item in ADIDs)
            {
                cost += Convert.ToInt32(_db.Ads.Where(a => a.AdId == item).Select(a => a.AdFee).FirstOrDefault());                
            }
            if (balance - cost < 0)
            {
                result = new List<int>() { 3, balance, cost };
                return Json(result);
            }
            //==========================
            foreach (var ads in ADIDs)
            {

                decimal adscost = _db.Ads.Where(a => a.AdId == ads).Select(a => a.AdFee).FirstOrDefault();
                    int period = _db.Ads.Where(a => a.AdId == ads).Select(a => a.AdPeriod).FirstOrDefault();
                    AdtoProduct res = new AdtoProduct()
                    {
                        AdId = ads,
                        ProductId = itemID,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(period),
                        IsSubActive = true,
                        ExpoTimes = 0,
                        ClickTimes = 0,
                        AdSlogan = ADtext,
                    };
                    _db.AdtoProducts.Add(res);

                BalanceRecord res2 = new BalanceRecord
                {
                    MemberId = id,
                    Reason = "訂閱廣告",
                    Record = DateTime.Now,
                    Amount = Convert.ToInt32(0 - adscost)
                };
                _db.BalanceRecords.Add(res2);
            }
            try {
                acc.Balance -= cost;
                _db.SaveChanges();
                return Json(1);
            }
            catch {
                //儲存失敗
                return Json(0); 
            }
        }
        public IActionResult UnSubAction(int? id, int[] ids)
        {
            if (id != null)
            {
                AdtoProduct ad = _db.AdtoProducts.Where(a => a.AdtoProductId == id && a.IsSubActive == true).FirstOrDefault();
                if (ad != null)
                {
                    ad.EndDate = DateTime.Now;
                    ad.IsSubActive = false;
                }
                try {
                    _db.SaveChanges();
                    return Json(1);
                }
                catch {
                    return Json(10);
                }
            }
            else if (ids.Any())
            {
                try {
                    foreach (var i in ids) {
                        AdtoProduct ad = _db.AdtoProducts.Where(a => a.AdtoProductId == i && a.IsSubActive == true).FirstOrDefault();
                        if (ad != null)
                        {
                            ad.EndDate = DateTime.Now;
                            ad.IsSubActive = false;
                            try { 
                                _db.SaveChanges();
                            }
                            catch {
                                return Json(21);
                            }
                        }
                    }
                    return Json(2);
                }
                catch {
                    return Json(20);
                }
            }
            else
            {
                return Json(0);
            }
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
            var smID = _db.SmallTypes.Where(n => n.SmallTypeName == result.smalltype).Select(n => n.SmallTypeId).FirstOrDefault();
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

            if (result.BodyPic!= null)
            {
                for (int i = 0; i < result.BodyPic.Count; i++)
                {
                    ProductPic productPic = new ProductPic()
                    {
                        Pic = result.BodyPic[i],
                        ProductId = Convert.ToInt32(product.ProductId)
                    };
                    _db.ProductPics.Add(productPic);
                }
            }

            if (result.SelectADId != null)
            {
                for (int i = 0; i < result.SelectADId.Count; i++)
                {
                    var daynum = _db.Ads.Where(n => n.AdId == result.SelectADId[i]).Select(n => n.AdPeriod).FirstOrDefault();

                    AdtoProduct adtoProduct = new AdtoProduct()
                    {
                        AdId = Convert.ToInt32(result.SelectADId[i]),
                        ProductId = Convert.ToInt32(product.ProductId),
                        StartDate =DateTime.Now,
                        EndDate = DateTime.Now.AddDays(daynum),
                        IsSubActive = true,
                        ExpoTimes = 0,
                        ClickTimes = 0
                    };
                    _db.AdtoProducts.Add(adtoProduct);
                }
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
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int id = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            if (_db.MemberAccounts.Where(n=>n.MemberId== id && n.MemStatusId!=2).Select(n=>n).Any())
            {
                CLoginCheck cLoginCheck = new CLoginCheck()
                {
                    MemberId = id,
                    MemStatusId = _db.MemberAccounts.Where(n => n.MemberId == id).Select(n => n.MemStatusId).FirstOrDefault()
                };
                return View(cLoginCheck);
            }
            return View();
        }






        public IActionResult NewIndex(int? pageSize, int? page)
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

            List<int> myproductlist = new List<int>();
            List<Product> q1 = new List<Product>();
            if (string.IsNullOrEmpty(select))//傳回來是空值或沒東西
            {
                 myproductlist = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2 ).Select(n => n.ProductId).ToList(); //賣家所有商品ID
                 q1 = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2).Select(n => n).ToList();//賣家所有商品
            }
            else
            {
                 myproductlist = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2&&n.ProductName.ToUpper().Contains(select.ToUpper())).Select(n => n.ProductId).ToList(); //賣家所有商品ID
                 q1 = _db.Products.Where(n => n.MemberId == id && n.ProductStatusId != 2 && n.ProductName.ToUpper().Contains(select.ToUpper())).Select(n => n).ToList();//賣家所有商品
            }
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

            var paylist = _db.PaymentToSellers.Where(n => n.MemberId == memId).Select(n => n.PaymentId).ToList();
            var mempay = _db.Payments.Where(n => paylist.Contains(n.PaymentId)).Select(n => n).ToList();

            var Category = _db.CustomizedCategories.Where(n => n.MemberId == memId).Select(n => n.CustomizedCategoryName).ToList();
            var CustomizedCategoryID = _db.CustomizedCategories.Where(n => n.MemberId == memId).Select(n => n.CustomizedCategoryId).ToList();
            

            var pName = _db.Products.Where(n => n.ProductId == id).Select(n => n.ProductName).FirstOrDefault();
            var pProductId = id;
            var psmallTypeID = _db.Products.Where(n => n.ProductId == id).Select(n => n.SmallTypeId).FirstOrDefault();
            var psmallTypeName = _db.SmallTypes.Where(n => n.SmallTypeId == psmallTypeID).Select(n => n.SmallTypeName).FirstOrDefault();
            var pbigTypeID = _db.SmallTypes.Where(n=>n.SmallTypeId== psmallTypeID).Select(n=>n.BigTypeId).FirstOrDefault();
            var pbigTypeName = _db.BigTypes.Where(n => n.BigTypeId == pbigTypeID).Select(n => n.BigTypeName).FirstOrDefault();
            var pDBtoPic = _db.ProductPics.Where(n => n.ProductId == id).Select(n => n.Pic).ToList();
            var pDetail = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.ProductDetailId).ToList();
            var pStyle = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Style).ToList();
            var PQuantity = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Quantity).ToList();
            var pUnitPrice = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.UnitPrice).ToList();
            var pBodyPicID = _db.ProductPics.Where(n => n.ProductId == id).Select(n => n.ProductPicId).ToList();
            var pBodyPic = _db.ProductDetails.Where(n => n.ProductId == id).Select(n => n.Pic).ToList();
            var pDescription = _db.Products.Where(n => n.ProductId == id).Select(n => n.Description).FirstOrDefault();
            var pCategory = _db.Products.Where(n => n.ProductId == id).Select(n => n.CustomizedCategoryId).FirstOrDefault();
            var pCategoryName = _db.CustomizedCategories.Where(n=>n.CustomizedCategoryId== pCategory).Select(n=>n.CustomizedCategoryName).FirstOrDefault();

            CSellerCreateToViewViewModel x = new CSellerCreateToViewViewModel
            {
                bigType = bigType,
                bigTypealone= pbigTypeName,
                bigTypeIDalone = pbigTypeID,
                smallTypealone =psmallTypeName,
                smallType = smallType,
                memship = memship,
                shipID = shiperlist,
                PaymentID = paylist,
                mempay = mempay,
                Category =Category,
                CustomizedCategoryID=CustomizedCategoryID,
                Categoryalone= pCategoryName,
                ProductName = pName,
                DBtoPic= pDBtoPic,
                DetailID=pDetail,
                Style = pStyle,
                Quantity = PQuantity,
                UnitPrice = pUnitPrice,
                BodyPicID= pBodyPicID,
                BodyPic =pBodyPic,
                Description = pDescription,
                ProductID=Convert.ToInt32(pProductId)
            };
            return View(x);            
        }

        public void EditProductSuccess(CSellerCreateToViewViewModel jsonString) //商品編輯儲存
        {
            string jsonstring = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(jsonstring).MemberId; //字串轉物件 MemberAccount

            var Product= _db.Products.Where(n => n.ProductId == jsonString.ProductID).Select(n => n).FirstOrDefault();
            var smID = _db.SmallTypes.Where(n => n.SmallTypeName == jsonString.smalltype).Select(n => n.SmallTypeId).FirstOrDefault();
            Product.ProductName = jsonString.ProductName;
            Product.SmallTypeId = smID;
            Product.MemberId = Product.MemberId;
            Product.RegionId = Product.RegionId;
            Product.Description = jsonString.Description;
            Product.ProductStatusId = Product.ProductStatusId;
            Product.EditTime = DateTime.Now;
            Product.CustomizedCategoryId = jsonString.CategoryID;


            var ProductDetail = _db.ProductDetails.Where(n => n.ProductId == jsonString.ProductID).Select(n => n).ToList();

            //if (jsonString.DetailID != null)//刪除規格
            //{
            //    for (int i = 0; i < jsonString.DetailID.Count; i++)
            //    {
            //        _db.Remove(_db.ProductDetails.Where(n => n.ProductDetailId == jsonString.DetailID[i]).Select(n => n).FirstOrDefault());
            //    }
            //}
            //var proPic = _db.ProductPics.Where(n => n.ProductId == jsonString.ProductID).Select(n => n.Pic).ToList();

            if (jsonString.暫存規格 != null)//新增規格
            {
                foreach (var a in jsonString.暫存規格)
                {
                    var q= _db.ProductDetails.Where(i => i.ProductDetailId == a.productDetailIDStr).FirstOrDefault();
                    if (q == null)
                    {
                        ProductDetail productDetail = new ProductDetail()
                        {
                            ProductId = Convert.ToInt32(jsonString.ProductID),
                            Style = jsonString.暫存規格[0].StyleStr,
                            Quantity = Convert.ToInt32(jsonString.暫存規格[0].QuantityStr),
                            UnitPrice = Convert.ToInt32(jsonString.暫存規格[0].UnitPriceStr),
                            Pic = jsonString.暫存規格[0].BodyPicStr    
                        };
                        _db.ProductDetails.Add(productDetail);
                    }
                }
            }

            
            if (jsonString.BodyPic !=null)
            {
                var product = _db.ProductPics.Where(n => n.ProductId == jsonString.ProductID).Select(n => n).ToList();

                for (int i = 0; i < jsonString.BodyPic.Count; i++)
                {
                    if(jsonString.Dataphotoindex[i] < product.Count)
                    {
                        product[jsonString.Dataphotoindex[i]].Pic = jsonString.BodyPic[i];
                    }
                    else
                    {
                        ProductPic asd = new ProductPic()
                        {
                            Pic = jsonString.BodyPic[i],
                            ProductId = jsonString.ProductID,
                        };
                        _db.ProductPics.Add(asd);
                    }                    
                }
            }
            _db.SaveChanges();
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
            List<Coupon> x = new List<Coupon>();
            if (!string.IsNullOrEmpty(select))
            {
                 x = _db.Coupons.Where(n => n.MemberId == memId&&n.CouponName==select).Select(n => n).ToList();
            }
            else
            {
                 x = _db.Coupons.Where(n => n.MemberId == memId).Select(n => n).ToList();
            }
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
                IsFreeDelivery = result.IsFreeDelivery,
                MinimumOrder = result.MinimumOrder
            };
            _db.Coupons.Add(coupon);
            _db.SaveChanges();
        }
        public IActionResult DeleteCoupon(int? id)  //刪除商品
        {
            if (id != null)
            {
                Coupon coupon = _db.Coupons.Where(n => n.CouponId == id).FirstOrDefault();
                coupon.ExpiredDate= DateTime.Now.AddDays(-1); 
                coupon.ReceiveEndDate= DateTime.Now.AddDays(-1);
                _db.SaveChanges();
            }
            return RedirectToAction("Coupon");
        }

        public void EditCoupon(string jsonString)
        {
            Coupon result = JsonSerializer.Deserialize<Coupon>(jsonString);
            var Coupon = _db.Coupons.Where(n => n.CouponId == result.CouponId).Select(n => n).FirstOrDefault();

            Coupon.CouponName = result.CouponName;
            Coupon.CouponCode = result.CouponCode;
            Coupon.Discount = result.Discount;
            Coupon.StartDate = result.StartDate;
            Coupon.ExpiredDate = result.ExpiredDate;
            Coupon.ReceiveStartDate = result.ReceiveStartDate;
            Coupon.ReceiveEndDate = result.ReceiveEndDate;
            Coupon.IsFreeDelivery = result.IsFreeDelivery;
            Coupon.MinimumOrder = result.MinimumOrder;

            _db.SaveChanges();
        }
        

        public IActionResult seller跑條(int page)
        {
            return ViewComponent("seller跑條", page);
        }

        public IActionResult Event()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }
            string logindata = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(logindata).MemberId; //字串轉物件 MemberAccount


            var E = _db.SubOfficialEventLists.Select(i => i).ToList();  //所有子活動    
            var OEtoPro = _db.SubOfficialEventToProducts.Select(i => i.ProductId).ToList();
            var PID= _db.Products.Where(n => n.MemberId == memId&& !OEtoPro.Contains(n.ProductId)).Select(i => i).ToList();//賣家所有商品(不包含參加商品)
            var OE = _db.OfficialEventLists.Select(i => i).ToList(); //所有活動
            
            List<CSubEventToProductViewModel> list = new();
            foreach (var e in E)
            {
                var B = E.Where(n => n.OfficialEventListId == e.OfficialEventListId).Select(n => n).ToList();
                CSubEventToProductViewModel C = new()
                {
                    Products = PID,
                    SubOfficialEventID = e,
                    OfficialEventList = OE,
                    SubOfficialEventList=B,
                };
                list.Add(C);
            }
            return View(list);
        }

        public void EventJoin(CSubEventToProductViewModel jsonString)
        {
            SubOfficialEventToProduct subOfficialEventToProducts = new SubOfficialEventToProduct()
            {
                ProductId= jsonString.ProductID,
                SubOfficialEventListId=jsonString.SubOfficialEventIDBack,
                VerifyId=1
            };
            _db.SubOfficialEventToProducts.Add(subOfficialEventToProducts);
            _db.SaveChanges();
        }

        public IActionResult Smalleve(int bigeveid)
        {
            var a = _db.SubOfficialEventLists.Where(s => s.OfficialEventListId == bigeveid).Select(s => new { id = s.SubOfficialEventListId, name = s.SubEventName }).ToList();
            return Json(a);
        }

        public IActionResult JoinEventChi()
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return RedirectToAction("Login", "Member");
            }

            string logindata = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER); //拿出session登入字串
            int memId = JsonSerializer.Deserialize<MemberAccount>(logindata).MemberId; //字串轉物件 MemberAccount

            //多對一有導覽屬性，一對多沒有


             List<CSubEventToProductViewModel> cSubEventToProductViewModel = _db.SubOfficialEventToProducts.Where(i => i.Product.MemberId == memId).Select(i => new CSubEventToProductViewModel
            {
                審核結果 = i.Verify.VerifyName,
                productname = i.Product.ProductName,
                evename = i.SubOfficialEventList.OfficialEventList.EventName,
                subevename = i.SubOfficialEventList.SubEventName,
                SubOfficialEventToProductsID = i.SubOfficialEventToProductId,
            }).ToList();
            
            return View(cSubEventToProductViewModel);
        }

        public void DeleteEvent(int? id)
        {
            var SubOfficialEventToProductsID = _db.SubOfficialEventToProducts.Where(n => n.SubOfficialEventToProductId == id).Select(n => n).FirstOrDefault();
            _db.SubOfficialEventToProducts.Remove(SubOfficialEventToProductsID);
            _db.SaveChanges();
        }


        public IActionResult AddMoreProduct()
        {
            string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            MemberAccount member = JsonSerializer.Deserialize<MemberAccount>(memberString);
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var products = dbContext.Products.Where(i => i.MemberId == 28);
            foreach (var a in products)
            {
                a.MemberId = member.MemberId;
            }
            dbContext.SaveChanges();
            return Content("1");
        }

        public IActionResult ADdemo1() {
            int memId = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            for (int i= 1; i < 6; i++)
            {
                Product prod = new Product
                {
                    ProductName = "牛角麵包" + i,
                    SmallTypeId = 90,
                    MemberId = memId,
                    RegionId = 362,
                    Description = "好吃好吃麵包",
                    ProductStatusId = 0,
                    EditTime = DateTime.Now,
                    CustomizedCategoryId = 1
                };
                _db.Products.Add(prod);
                _db.SaveChanges();
                ProductDetail prode = new ProductDetail
                {
                    ProductId = prod.ProductId,
                    Style = "大紅",
                    Quantity = 777,
                    UnitPrice = 100,
                };
                _db.ProductDetails.Add(prode);
                _db.SaveChanges();
            }
            return Json(0);
        }
        public IActionResult ADdemo2()
        {
            int memId = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            var prod = _db.Products.Where(p => p.MemberId == memId).OrderByDescending(p => p.ProductId).Take(5);
            foreach(var p in prod)
            {
                AdtoProduct ad = new AdtoProduct
                {
                    ProductId=p.ProductId,
                    AdId = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    IsSubActive = true,
                    ExpoTimes = 0,
                    ClickTimes = 0,
                };
                AdtoProduct ad2 = new AdtoProduct
                {
                    ProductId = p.ProductId,
                    AdId = 4,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    IsSubActive = true,
                    ExpoTimes = 0,
                    ClickTimes = 0,
                };
                AdtoProduct ad3 = new AdtoProduct
                {
                    ProductId = p.ProductId,
                    AdId = 7,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    IsSubActive = true,
                    ExpoTimes = 0,
                    ClickTimes = 0,
                    AdSlogan="好吃的牛角族麵包！",
                };
                _db.AdtoProducts.Add(ad);
                _db.AdtoProducts.Add(ad2);
                _db.AdtoProducts.Add(ad3);
            }
            _db.SaveChanges();
            return Json(0);
        }
        public IActionResult DemoEventDemoCheck()
        {
            int memId = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            var a = _db.SubOfficialEventToProducts.Where(e => e.Product.MemberId == memId).Where(p => p.VerifyId == 1);
            if (a.Any()) { 
            foreach(var item in a)
            {
                item.VerifyId = 2;
            }
            _db.SaveChanges();
            }

            return Json(memId);
        }
        public IActionResult DemoGetOrder()
        {
            int memId = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER)).MemberId;
            ProductDetail proddetail = _db.ProductDetails.Where(p => p.Product.MemberId == memId).OrderByDescending(p => p.ProductDetailId).FirstOrDefault();
            for(int i = 0; i < 3; i++) {
                Order demo = new Order
                {
                    MemberId = 25,
                    OrderDatetime = DateTime.Now,
                    RecieveAdr = "106台北市大安區復興南路一段390號2樓",
                    FinishDate = DateTime.Now,
                    CouponId = 1,
                    StatusId = 7,
                    ShipperId = 1,
                    PaymentDate = DateTime.Now,
                    ShippingDate = DateTime.Now,
                    ReceiveDate = DateTime.Now,
                    PaymentId = 1,
                };
                _db.Orders.Add(demo);                
            }
            _db.SaveChanges();
            var Oid = _db.Orders.Where(p => p.MemberId == 25).OrderByDescending(p => p.OrderId).Select(p => p.OrderId).Take(3).ToList();
            for (int i = 0; i < 3; i++)
            {
                OrderDetail demo = new OrderDetail
                {
                    OrderId = Oid[i],
                    ProductDetailId = proddetail.ProductDetailId,
                    Quantity = (i + 1) * 2,
                    ShippingStatusId=5,
                    Unitprice=proddetail.UnitPrice,
                };
                _db.OrderDetails.Add(demo);
            }
            _db.SaveChanges();
            return Json(Oid);
        }
    }
}


