using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.newManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using X.PagedList;

namespace prjiSpanFinal.Controllers
{
    public class newManagementController : Controller
    {
        #region ProductRegion
        public List<CProductListViewModel> GetProductsFromDatabase(string keyword, string filter)
        {
            var db = new iSpanProjectContext();
            List<CProductListViewModel> list = new();
            IQueryable<Product> Prods = null;
            int FilterId;
            if (!String.IsNullOrEmpty(filter))
            {
                FilterId = db.ProductStatuses.FirstOrDefault(i => i.ProductStatusName == filter).ProductStatusId;
            }
            else
            {
                FilterId = -1;
            }
            if (keyword == null && FilterId < 0)
            {
                Prods = db.Products.Select(i => i);
            }
            else if (keyword == null)
            {
                Prods = db.Products.Where(i => i.ProductStatusId == FilterId).Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Prods = db.Products.
                     Where(i => i.ProductId == key && i.ProductStatusId == FilterId).
                     Select(e => e);
            }
            else
            {
                Prods = db.Products.
                    Where(i => i.ProductName.Contains(keyword) && i.ProductStatusId == FilterId).
                    Select(e => e); ;
            }
            var MemberAcc = (from i in db.MemberAccounts select i).ToList();
            var ProductStatusName = (from i in db.ProductStatuses select i).ToList();
            var RegionName = (from i in db.RegionLists select i).ToList();
            var SmallTypeName = (from i in db.SmallTypes select i).ToList();
            var CustomizedCategoryName = (from i in db.CustomizedCategories select i).ToList();
            var ProductPic = db.ProductPics.ToList();
            foreach (var p in Prods)
            {
                CProductListViewModel model = new()
                {
                    Product = p,
                    MemberAcc = MemberAcc.FirstOrDefault(i => i.MemberId == p.MemberId).MemberAcc,
                    ProductStatusName = (from i in ProductStatusName
                                         where i.ProductStatusId == p.ProductStatusId
                                         select i.ProductStatusName).First(),
                    RegionName = (from i in RegionName
                                  where i.RegionId == p.RegionId
                                  select i.RegionName).First(),
                    SmallTypeName = (from i in SmallTypeName
                                     where i.SmallTypeId == p.SmallTypeId
                                     select i.SmallTypeName).First(),
                    CustomizedCategoryName = (from i in CustomizedCategoryName
                                              where i.CustomizedCategoryId == p.CustomizedCategoryId
                                              select i.CustomizedCategoryName).First(),
                };
              var Q = ProductPic.Where(i=>i.ProductId==model.Product.ProductId);
                if (Q.Any())
                {
                    model.ProductPic = Q.First().Pic;
                }
                list.Add(model);
            }

            return list;
        }
        protected IPagedList<CProductListViewModel> GetPagedProcess(int? page, int pageSize, string keyword, string filter)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetProductsFromDatabase(keyword, filter);
            IPagedList<CProductListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult newProductList(string keyword, int? pageSize, int? page, string filter)
        {
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetPagedProcess(page, (int)pageSize, keyword, filter);
            var PList = GetPagedProcess(page, (int)pageSize, keyword, filter);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult newProductList2(int? id)
        {
            var Q = from u in new iSpanProjectContext().Products
                    where u.ProductId == id
                    select u;
            return View(Q);
        }
        public IActionResult ProductDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Products
                    where d.ProductId == id
                    select d;
            D.First().ProductStatusId = 2;
            D.First().EditTime = DateTime.Now;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ProductUndo(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Products
                    where d.ProductId == id
                    select d;
            D.First().ProductStatusId = 0;
            D.First().EditTime = DateTime.Now;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ProductDown(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Products
                    where d.ProductId == id
                    select d;
            D.First().ProductStatusId = 1;
            D.First().EditTime = DateTime.Now;
            db.SaveChanges();
            return Content("1");
        }

        #endregion
        #region ProductDetailRegion
        public IActionResult ProductDetailList(int? id)
        {
            var db = new iSpanProjectContext();
            if (id != null)
            {
                var Q = (from i in db.ProductDetails
                         where i.ProductId == id
                         select i);

                return View(Q);
            }
            else
            {
                return RedirectToAction("newProductList");
            }
        }
        public IActionResult ProductDetailDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.ProductDetails
                    where d.ProductDetailId == id
                    select d;
            var X = D.First();
            var G = from g in db.Products
                    where g.ProductId == D.First().ProductId
                    select g;
            var K = from k in db.ProductDetails
                    where k.ProductId == G.First().ProductId
                    select k;

            return RedirectToAction("ProductDetailList", new { id = X.ProductId });
        }
        #endregion
        #region MemberRegion
        public List<CMemberListViewModel> GetMembersFromDatabase(string keyword, string filter)
        {
            var db = new iSpanProjectContext();
            List<CMemberListViewModel> list = new();
            IQueryable<MemberAccount> mems = null;

            int FilterId;
            if (!String.IsNullOrEmpty(filter))
            {
                FilterId = db.MemStatuses.FirstOrDefault(i => i.MemStatusName == filter).MemStatusId;
            }
            else
            {
                FilterId = -1;
            }

            if (String.IsNullOrEmpty(keyword) && FilterId < 0)
            {
                if (FilterId < 0)
                {
                    mems = db.MemberAccounts.Select(i => i);
                }
                else
                {
                    mems = db.MemberAccounts.Where(i => i.MemStatusId == FilterId).Select(i => i);
                }
            }
            else if (int.TryParse(keyword, out int key))
            {
                if (FilterId > 0)
                {
                    mems = db.MemberAccounts.
                         Where(i => i.MemberId == key && i.MemStatusId == FilterId).
                         Select(e => e);
                }
                else
                {
                    mems = db.MemberAccounts.
                        Where(i => i.MemberId == key).
                        Select(e => e);
                }
            }
            else
            {
                if (FilterId > 0)
                {
                    mems = db.MemberAccounts.
                        Where(i => i.MemStatusId == FilterId && (i.Name.Contains(keyword) || i.Phone.Contains(keyword) || i.Email.Contains(keyword) || i.MemberAcc.Contains(keyword))).
                        Select(e => e);
                }
                else
                {
                    mems = db.MemberAccounts.
                       Where(i => i.Name.Contains(keyword) || i.Phone.Contains(keyword) || i.Email.Contains(keyword) || i.MemberAcc.Contains(keyword))
                       .Select(e => e);
                }
            }
            var MemStatuses = (from i in db.MemStatuses select i).ToList();
            var RegionLists = (from i in db.RegionLists select i).ToList();
            foreach (var p in mems)
            {
                CMemberListViewModel model = new()
                {
                    MemberAccount = p,
                    MemStatusName = (from i in MemStatuses
                                     where i.MemStatusId == p.MemStatusId
                                     select i.MemStatusName).First(),
                    RegionName = (from i in RegionLists
                                  where i.RegionId == p.RegionId
                                  select i.RegionName).First(),
                };
                list.Add(model);
            }

            return list;
        }
        protected IPagedList<CMemberListViewModel> GetMemPagedProcess(int? page, int? pageSize, string keyword, string filter)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetMembersFromDatabase(keyword, filter);
            IPagedList<CMemberListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, (int)pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult MemberList(string keyword, string filter, int? pageSize, int? page = 1)
        {
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetMemPagedProcess(page, (int)pageSize, keyword, filter);
            var PList = GetMemPagedProcess(page, (int)pageSize, keyword, filter);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult MemberList2(int? id)
        {
            var Q = from u in new iSpanProjectContext().MemberAccounts
                    where u.MemberId == id
                    select u;
            return View(Q);
        }
        public IActionResult MemberDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.MemberAccounts
                    where d.MemberId == id
                    select d;
            D.First().MemStatusId = 5;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult MemberUndo(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.MemberAccounts
                    where d.MemberId == id
                    select d;
            D.First().MemStatusId = 2;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult MemberStop(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.MemberAccounts
                    where d.MemberId == id
                    select d;
            D.First().MemStatusId = 4;
            db.SaveChanges();
            return Content("1");
        }
        #endregion
        #region PowerBi
        public IActionResult PowerBi()
        {
            return View();
        }
        #endregion
        #region OrderRegion
        public List<COrderListViewModel> GetOrdersFromDatabase(string keyword, string filter)
        {
            var db = new iSpanProjectContext();
            List<COrderListViewModel> list = new();
            IQueryable<Order> Orders = null;
            int FilterId;
            if (!String.IsNullOrEmpty(filter))
            {
                FilterId = db.OrderStatuses.FirstOrDefault(i => i.OrderStatusName == filter).OrderStatusId;
            }
            else
            {
                FilterId = -1;
            }

            if (keyword == null)
            {
                if (FilterId < 0)
                {
                    Orders = db.Orders.Select(i => i);
                }
                else
                {
                    Orders = db.Orders.Where(i => i.StatusId == FilterId).Select(i => i);
                }
            }
            else if (int.TryParse(keyword, out int key))
            {
                if (FilterId > 0)
                {
                    Orders = db.Orders.
                         Where(i => i.StatusId == FilterId && (i.OrderId == key || i.MemberId == key)).
                         Select(e => e);
                }
                else
                {
                    Orders = db.Orders.
                             Where(i => i.OrderId == key || i.MemberId == key).
                             Select(e => e);
                }
            }
            else if (DateTime.TryParse(keyword, out DateTime key2))
            {
                if (FilterId > 0)
                {
                    Orders = db.Orders.
                         Where(i => i.StatusId == FilterId && i.OrderDatetime == key2).
                         Select(e => e);
                }
                else
                {
                    Orders = db.Orders.
                             Where(i => i.OrderDatetime == key2).
                             Select(e => e);
                }
            }
            else
            {
                if (FilterId < 0)
                {
                    Orders = db.Orders.
                        Where(i => i.OrderMessage.Contains(keyword)).
                        Select(e => e); ;
                }
                else
                {
                    Orders = db.Orders.
                            Where(i => i.StatusId == FilterId && i.OrderMessage.Contains(keyword)).
                            Select(e => e); ;
                }
            }
            var OrderStatusName = (from i in db.OrderStatuses select i).ToList();
            var ShipperName = (from i in db.Shippers select i).ToList();
            var SmallTypeName = (from i in db.SmallTypes select i).ToList();
            var PaymentName = (from i in db.Payments select i).ToList();
            var CouponName = (from i in db.Coupons select i).ToList();
            foreach (var p in Orders)
            {
                COrderListViewModel model = new()
                {
                    Order = p,
                    OrderStatusName = (from i in OrderStatusName
                                       where i.OrderStatusId == p.StatusId
                                       select i.OrderStatusName).First(),
                    ShipperName = (from i in ShipperName
                                   where i.ShipperId == p.ShipperId
                                   select i.ShipperName).First(),
                    PaymentName = (from i in PaymentName
                                   where i.PaymentId == p.PaymentId
                                   select i.PaymentName).First(),
                    CouponName = (from i in CouponName
                                  where i.CouponId == p.CouponId
                                  select i.CouponName).First(),
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<COrderListViewModel> GetOrdersPagedProcess(int? page, int pageSize, string keyword, string filter)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetOrdersFromDatabase(keyword, filter);
            IPagedList<COrderListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult OrderList(string keyword, string filter, int? pageSize, int? page = 1)
        {
            ViewBag.OrderStatus = (new iSpanProjectContext().OrderStatuses).Select(i => i);
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetOrdersPagedProcess(page, (int)pageSize, keyword, filter);
            var PList = GetOrdersPagedProcess(page, (int)pageSize, keyword, filter);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult OrderOut(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 4;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult OrderStop(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 8;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult OrderDeliver(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 6;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult OrderFin(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 8;
            D.First().FinishDate = DateTime.Now;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult OrderDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 9;
            db.SaveChanges();
            return RedirectToAction("OrderList", new { id });
        }

        //視情形決定要不要用第二張List
        //public IActionResult OrderList2(int? id)
        //{
        //    var Q = from u in new iSpanProjectContext().Orders
        //            where u.OrderId == id
        //            select u;
        //    return View(Q);
        //}
        #endregion
        #region OrderDetailRegion
        public IActionResult OrderDetailList(int? id)
        {
            var db = new iSpanProjectContext();
            if (id != null)
            {
                var Qs = (from i in db.OrderDetails
                          where i.OrderId == id
                          select i);
                var PDName = (from u in db.ProductDetails select u).ToList();
                var PName = (from x in db.Products select x).ToList();
                var SSName = (from s in db.ShippingStatuses select s).ToList();
                List<COrderDetailViewModel> A = new();
                foreach (var Q in Qs)
                {
                    var a = new COrderDetailViewModel()
                    {
                        OrderDetail = Q,
                        ProductDetailName = (from w in PDName
                                             where w.ProductDetailId == Q.ProductDetailId
                                             select w.Style).First(),
                        ShippingStatusName = (from ss in SSName
                                              where ss.ShippingStatusId == Q.ShippingStatusId
                                              select ss.ShipStatusName).First(),
                        ProductName = (from x in PName
                                       join xs in PDName on x.ProductId equals xs.ProductId
                                       where xs.ProductDetailId == Q.ProductDetailId
                                       select x.ProductName).First(),
                    };
                    A.Add(a);
                }

                return View(A);
            }
            else
            {
                return RedirectToAction("OrderList");
            }
        }
        public IActionResult OrderDetailDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.OrderDetails
                    where d.OrderDetailId == id
                    select d;
            var G = from g in db.Orders
                    where g.OrderId == D.First().OrderId
                    select g;
            var K = from k in db.OrderDetails
                    where k.OrderId == G.First().OrderId
                    select k;
            if (K.Count() - 1 > 0)
            {
                db.OrderDetails.Remove(D.First());
            }
            db.SaveChanges();
            return RedirectToAction("OrderDetailList", new { id = id });
        }
        #endregion
        #region ReportRegion
        public List<CReportListViewModel> GetReportsFromDatabase(string keyword, string filter)
        {
            var db = new iSpanProjectContext();
            List<CReportListViewModel> list = new();
            IQueryable<Report> Reps = null;
            int FilterId;
            if (!String.IsNullOrEmpty(filter))
            {
                FilterId = db.ReportStatuses.FirstOrDefault(i => i.ReportStatusName == filter).ReportStatusId;
            }
            else
            {
                FilterId = -1;
            }

            if (keyword == null)
            {
                if (FilterId < 0)
                {
                    Reps = db.Reports.Select(i => i);
                }
                else
                {
                    Reps = db.Reports.Where(i => i.ReportStatusId == FilterId).Select(i => i);
                }
            }
            else if (int.TryParse(keyword, out int key))
            {
                if (FilterId < 0)
                {
                    Reps = db.Reports.
                         Where(i => i.ReportId == key || i.ReporterId == key).
                         Select(e => e);
                }
                else
                {
                    Reps = db.Reports.
                         Where(i => i.ReportStatusId == FilterId && (i.ReportId == key || i.ReporterId == key)).
                         Select(e => e);
                }
            }
            else
            {
                if (FilterId < 0)
                {
                    Reps = db.Reports.
                        Where(i => i.Reason.Contains(keyword)).
                        Select(e => e);
                }
                else
                {
                    Reps = db.Reports.
                     Where(i => i.ReportStatusId == FilterId && i.Reason.Contains(keyword)).
                     Select(e => e);
                }
            }
            var ProductName = (from i in db.Products select i).ToList();
            var ReportTypeName = (from i in db.ReportTypes select i).ToList();
            var ReportStatusName = (from i in db.ReportStatuses select i).ToList();
            foreach (var p in Reps)
            {
                CReportListViewModel model = new()
                {
                    Report = p,

                    ProductName = (from i in ProductName
                                   where i.ProductId == p.ProductId
                                   select i.ProductName).First(),
                    ReportTypeName = (from i in ReportTypeName
                                      where i.ReportTypeId == p.ReportTypeId
                                      select i.ReportTypeName).First(),
                    ReportStatusName = (from i in ReportStatusName
                                        where i.ReportStatusId == p.ReportStatusId
                                        select i.ReportStatusName).First(),
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<CReportListViewModel> GetReportPagedProcess(int? page, string filter, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetReportsFromDatabase(keyword, filter);
            IPagedList<CReportListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult ReportList(string keyword, string filter, int? pageSize, int? page = 1)
        {
            ViewBag.ReprotStatus = (new iSpanProjectContext().ReportStatuses).Select(i => i.ReportStatusName);
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 3;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetReportPagedProcess(page, filter, (int)pageSize, keyword);
            var PList = GetReportPagedProcess(page, filter, (int)pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult ReportProcess(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatusId = 1;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ReportDelete(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatusId = 2;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ReportUndo(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatusId = 6;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ReportWarning(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            Q.ReportStatusId = 3;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ReportServere(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            var prod = from p in db.Products
                       where p.ProductId == Q.ProductId
                       select p.ProductId;
            var G = db.Products.FirstOrDefault(i => i.ProductId == prod.First());
            G.ProductStatusId = 1;
            Q.ReportStatusId = 4;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ReportStop(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = db.Reports.FirstOrDefault(i => i.ReportId == id);
            var prod = from p in db.Products
                       join x in db.Reports on p.ProductId equals x.ProductId
                       where p.ProductId == Q.ProductId
                       select p.MemberId;
            var K = prod.First();
            var G = db.MemberAccounts.FirstOrDefault(i => i.MemberId == K);
            G.MemStatusId = 4;
            Q.ReportStatusId = 5;
            db.SaveChanges();
            return Content("1");
        }
        #endregion
        #region CouponRegion
        public List<CouponViewModel> GetCouponsFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            List<CouponViewModel> list = new();
            IQueryable<Coupon> Coupons = null;
            if (keyword == null)
            {
                Coupons = db.Coupons.Where(i => i.OfficialEventListId == 1).Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Coupons = db.Coupons.
                     Where(i => i.OfficialEventListId == 1 && (i.CouponId == key || i.MemberId == key)).
                     Select(e => e);
            }
            else if (DateTime.TryParse(keyword, out DateTime key2))
            {
                Coupons = db.Coupons.
                     Where(i => i.OfficialEventListId == 1 && (i.StartDate == key2 || i.ExpiredDate == key2 || i.ReceiveStartDate == key2 || i.ReceiveEndDate == key2)).
                     Select(e => e);
            }
            else
            {
                Coupons = db.Coupons.
                    Where(i => i.OfficialEventListId == 1 && (i.CouponCode.Contains(keyword) || i.CouponName.Contains(keyword))).
                    Select(e => e); ;
            }
            var MemberAcc = (from i in db.MemberAccounts select i).ToList();
            foreach (var coupon in Coupons)
            {
                CouponViewModel model = new()
                {
                    Coupon = coupon,
                    MemberAcc = MemberAcc.FirstOrDefault(i => i.MemberId == coupon.MemberId).MemberAcc
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<CouponViewModel> GetCouponsPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetCouponsFromDatabase(keyword);
            IPagedList<CouponViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult CouponList(string keyword, int? pageSize, int? page = 1)
        {
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetCouponsPagedProcess(page, (int)pageSize, keyword);
            var PList = GetCouponsPagedProcess(page, (int)pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult CouponCreate(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IActionResult CouponCreate(Coupon coupon)
        {
            iSpanProjectContext db = new();
            db.Coupons.Add(coupon);
            db.SaveChanges();
            return RedirectToAction("CouponList");
        }
        public IActionResult CouponEdit(int? id)
        {
            iSpanProjectContext db = new();
            var cps = (from i in db.Coupons
                       where i.CouponId == id
                       select i).First();
            return View(cps);
        }
        [HttpPost]
        public IActionResult CouponEdit(Coupon coupon)
        {
            iSpanProjectContext db = new();
            var cp = db.Coupons.FirstOrDefault(i => i.CouponId == coupon.CouponId);
            cp.CouponName = coupon.CouponName;
            cp.MemberId = coupon.MemberId;
            cp.CouponName = coupon.CouponName;
            cp.StartDate = coupon.StartDate;
            cp.ExpiredDate = coupon.ExpiredDate;
            cp.Discount = coupon.Discount;
            cp.CouponCode = coupon.CouponCode;
            cp.ReceiveStartDate = coupon.ReceiveStartDate;
            cp.ReceiveEndDate = coupon.ReceiveEndDate;
            cp.IsFreeDelivery = coupon.IsFreeDelivery;
            cp.MinimumOrder = coupon.MinimumOrder;
            db.SaveChanges();
            return RedirectToAction("CouponList");
        }
        #endregion
        #region ArgumentRegion
        public List<CArgumentViewModel> GetArgumentsFromDatabase(string keyword, string filter)
        {
            var db = new iSpanProjectContext();
            List<CArgumentViewModel> list = new();
            IQueryable<Argument> Arguments = null;
            int FilterId;
            if (!String.IsNullOrEmpty(filter))
            {
                FilterId = db.ArgumentTypes.FirstOrDefault(i => i.ArgumentTypeName == filter).ArgumentTypeId;
            }
            else
            {
                FilterId = -1;
            }
            if (keyword == null)
            {
                if (FilterId < 0)
                {
                    Arguments = db.Arguments.Select(i => i);
                }
                else
                {
                    Arguments = db.Arguments.Where(i => i.ArgumentTypeId == FilterId).Select(i => i);
                }
            }
            else if (int.TryParse(keyword, out int key))
            {
                if (FilterId < 0)
                {
                    Arguments = db.Arguments.
                         Where(i => i.ArgumentId == key || i.OrderId == key).
                         Select(e => e);
                }
                else
                {
                    Arguments = db.Arguments.
                         Where(i => i.ArgumentTypeId == FilterId && (i.ArgumentId == key || i.OrderId == key)).
                         Select(e => e);
                }
            }
            else
            {
                if (FilterId < 0)
                {
                    Arguments = db.Arguments.
                    Where(i => i.ReasonText.Contains(keyword)).
                    Select(e => e);
                }
                else
                {
                    Arguments = db.Arguments.
                    Where(i => i.ArgumentTypeId == FilterId && i.ReasonText.Contains(keyword)).
                    Select(e => e); ;
                }
            }
            var ArgumentTypeName = (from i in db.ArgumentTypes select i).ToList();

            var ArgumentReasonName = (from i in db.ArgumentReasons select i).ToList();
            foreach (var p in Arguments)
            {
                CArgumentViewModel model = new()
                {
                    Argument = p,
                    ArgumentReasonName = (from i in ArgumentReasonName
                                          where i.ArgumentReasonId == p.ArgumentReasonId
                                          select i.ArgumentReasonName).First(),
                    ArgumentTypeName = (from i in ArgumentTypeName
                                        where i.ArgumentTypeId == p.ArgumentTypeId
                                        select i.ArgumentTypeName).First(),
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<CArgumentViewModel> GetArgumentsPagedProcess(int? page, int pageSize, string keyword, string filter)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetArgumentsFromDatabase(keyword, filter);
            IPagedList<CArgumentViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult ArgumentList(string keyword, string filter, int? pageSize, int? page = 1)
        {
            ViewBag.ArgumentType = (new iSpanProjectContext().ArgumentTypes).Select(i => i);
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetArgumentsPagedProcess(page, (int)pageSize, keyword, filter);
            var PList = GetArgumentsPagedProcess(page, (int)pageSize, keyword, filter);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult ArgumentDelete(int? id)
        {
            iSpanProjectContext db = new();
            var G = from i in db.Arguments
                    where i.ArgumentId == id
                    select i;
            var g = G.First();
            g.ArgumentTypeId = 4;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult ArgumentApprove(int? id)
        {
            iSpanProjectContext db = new();
            var G = from i in db.Arguments
                    where i.ArgumentId == id
                    select i;
            var g = G.First();
            var K = (from k in db.Orders
                     join m in db.MemberAccounts on k.MemberId equals m.MemberId
                     where k.OrderId == g.OrderId
                     select m).First();
            var order = db.Orders.FirstOrDefault(i => i.OrderId == g.OrderId);
            order.StatusId = 7;
            K.MemStatusId = 4;
            g.ArgumentTypeId = 4;
            try
            {
                db.SaveChanges();
                return Content(K.MemberId.ToString());
            }
            catch (Exception)
            {
                return Content(null);
            }

        }
        public IActionResult ArgumentApprove2(int? id)
        {
            iSpanProjectContext db = new();
            var G = from i in db.Arguments
                    where i.ArgumentId == id
                    select i;
            var g = G.First();
            var S = db.OrderDetails.FirstOrDefault(i => i.OrderId == g.OrderId);

            var E = db.ProductDetails.FirstOrDefault(s => s.ProductDetailId == S.ProductDetailId).ProductId;
            var h = db.Products.FirstOrDefault(i => i.ProductId == E).MemberId;
            var SellerId = h;
            var Seller = db.MemberAccounts.FirstOrDefault(i => i.MemberId == SellerId);
            Seller.MemStatusId = 4;
            var order = db.Orders.FirstOrDefault(i => i.OrderId == g.OrderId);
            order.StatusId = 7;
            try
            {
                db.SaveChanges();
                return Content(SellerId.ToString());
            }
            catch (Exception)
            {
                return Content(null);
            }

        }
        #endregion
        #region EventRegion
        protected IQueryable<OfficialEventList> GetEventsFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            IQueryable<OfficialEventList> Prods = null;
            if (keyword == null)
            {
                Prods = db.OfficialEventLists.Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Prods = db.OfficialEventLists.
                     Where(i => i.OfficialEventListId == key).
                     Select(e => e);
            }
            else
            {
                Prods = db.OfficialEventLists.
                    Where(i => i.EventName.Contains(keyword)).
                    Select(e => e); ;
            }
            return Prods;
        }
        protected IPagedList<OfficialEventList> GetEventsPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetEventsFromDatabase(keyword);
            IPagedList<OfficialEventList> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult EventList(string keyword, int? pageSize, int? page = 1)
        {
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 3;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetEventsPagedProcess(page, (int)pageSize, keyword);
            var PList = GetEventsPagedProcess(page, (int)pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult EventCreate()
        {
            iSpanProjectContext db = new();
            var Q=db.OfficialEventTypes.ToList();
            ViewBag.EventType = Q;
            return View();
        }
        [HttpPost]
        public IActionResult EventCreate(CCreateEventViewModel ofevent)
        {
            iSpanProjectContext db = new();
            OfficialEventList Ev = new()
            {
                EndDate = ofevent.EndDate,
                EventName = ofevent.EventName,
                JoinEndDate = ofevent.JoinEndDate,
                JoinStartDate = ofevent.JoinStartDate,
                OfficialEventListId = ofevent.OfficialEventListId,
                StartDate = ofevent.StartDate,
                SubOfficialEventLists = ofevent.SubOfficialEventLists,
                OfficialEventTypeId=ofevent.OfficialEventTypeId
            };
            using (var ms = new MemoryStream())
            {
                ofevent.EventPic.CopyTo(ms);
                var filbytes = ms.ToArray();
                Ev.EventPic = filbytes;
            }
            db.OfficialEventLists.Add(Ev);
            db.SaveChanges();
            SubOfficialEventList sub = new()
            {
                Discount = 1,
                IsFreeDelivery = false,
                OfficialEventListId = Ev.OfficialEventListId,
                SubEventName = "預設活動",
            };
            db.SubOfficialEventLists.Add(sub);
            db.SaveChanges();
            return RedirectToAction("EventList");
        }
        public IActionResult EventEdit(int? id)
        {
            iSpanProjectContext db = new();
            var G = from g in db.OfficialEventLists where g.OfficialEventListId == id select g;
            var ofevent = G.First();
            return View(ofevent);
        }
        [HttpPost]
        public IActionResult EventEdit(CCreateEventViewModel ofevent)
        {
            iSpanProjectContext db = new();
            OfficialEventList Ev = db.OfficialEventLists.FirstOrDefault(i => i.OfficialEventListId == ofevent.OfficialEventListId);
            Ev.EndDate = ofevent.EndDate;
            Ev.EventName = ofevent.EventName;
            Ev.JoinEndDate = ofevent.JoinEndDate;
            Ev.JoinStartDate = ofevent.JoinStartDate;
            Ev.OfficialEventListId = ofevent.OfficialEventListId;
            Ev.StartDate = ofevent.StartDate;
            Ev.SubOfficialEventLists = ofevent.SubOfficialEventLists;
            using (var ms = new MemoryStream())
            {
                ofevent.EventPic.CopyTo(ms);
                var filbytes = ms.ToArray();
                Ev.EventPic = filbytes;
            }
            db.SaveChanges();
            return RedirectToAction("EventList");
        }
        public IActionResult EventDelete(int? id)
        {
            iSpanProjectContext db = new();
            var E = from e in db.OfficialEventLists
                    where e.OfficialEventListId == id
                    select e;
            var Ev = E.First();
            Ev.EndDate = DateTime.Now.AddDays(-1);
            db.SaveChanges();
            return Content(Ev.EndDate.ToString("yyyy/MM/dd"));
        }
        #endregion
        #region EventCouponRegion
        public IActionResult EventCouponCreate(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IActionResult EventCouponCreate(Coupon coupon)
        {
            iSpanProjectContext db = new();
            db.Coupons.Add(coupon);
            db.SaveChanges();
            return RedirectToAction("CouponList");
        }
        public List<CouponViewModel> GetEventCouponsFromDatabase(int id, string keyword)
        {
            var db = new iSpanProjectContext();
            List<CouponViewModel> list = new();
            IQueryable<Coupon> Coupons = null;
            if (keyword == null)
            {
                Coupons = db.Coupons.Where(i => i.OfficialEventListId == id).Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Coupons = db.Coupons.
                     Where(i => i.OfficialEventListId == id && (i.CouponId == key || i.MemberId == key)).
                     Select(e => e);
            }
            else if (DateTime.TryParse(keyword, out DateTime key2))
            {
                Coupons = db.Coupons.
                     Where(i => i.OfficialEventListId == id && (i.StartDate == key2 || i.ExpiredDate == key2 || i.ReceiveStartDate == key2 || i.ReceiveEndDate == key2)).
                     Select(e => e);
            }
            else
            {
                Coupons = db.Coupons.
                    Where(i => i.OfficialEventListId == id && (i.CouponCode.Contains(keyword) || i.CouponName.Contains(keyword))).
                    Select(e => e); ;
            }
            var MemberAcc = (from i in db.MemberAccounts select i).ToList();
            foreach (var coupon in Coupons)
            {
                CouponViewModel model = new()
                {
                    Coupon = coupon,
                    MemberAcc = MemberAcc.FirstOrDefault(i => i.MemberId == coupon.MemberId).MemberAcc
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<CouponViewModel> GetEventCouponsPagedProcess(int id, int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetEventCouponsFromDatabase(id, keyword);
            IPagedList<CouponViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult EventCouponList(int id, string keyword, int? pageSize, int? page = 1)
        {
            ViewBag.pageSize = pageSize;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetEventCouponsPagedProcess(id, page, (int)pageSize, keyword);
            var PList = GetEventCouponsPagedProcess(id, page, (int)pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        #endregion
        #region subEventRegion
        public IActionResult subEventList(int id)
        {
            iSpanProjectContext db = new();
            List<CsubEventViewModel> list = new();
            var Q = from i in db.SubOfficialEventLists
                    where i.OfficialEventListId == id
                    select i;
            var en = (from o in db.OfficialEventLists select o).ToList();
            foreach (var i in Q)
            {
                CsubEventViewModel model = new()
                {
                    SubOfficialEventList = i,
                    OfficialEventName = (from s in en
                                         where s.OfficialEventListId == i.OfficialEventListId
                                         select s.EventName).First(),
                };
                list.Add(model);
            }
            var A = Q.First();
            ViewBag.Id = A.OfficialEventListId;
            return View(list);
        }
        public IActionResult subEventCreate(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IActionResult subEventCreate(SubOfficialEventList ofevent)
        {
            iSpanProjectContext db = new();
            db.SubOfficialEventLists.Add(ofevent);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("subEventList", new { id = ofevent.OfficialEventListId });
            }
            return RedirectToAction("subEventList", new { id = ofevent.OfficialEventListId });
        }
        public IActionResult subEventDelete(int? id)
        {
            var db = (new iSpanProjectContext());
            var q = from i in db.SubOfficialEventLists
                    where i.SubOfficialEventListId == id
                    select i;
            var X = q.First();
            var G = from gg in db.OfficialEventLists
                    where gg.OfficialEventListId == q.First().OfficialEventListId
                    select gg.OfficialEventListId;
            var g = from i in db.SubOfficialEventLists
                    where i.OfficialEventListId == G.First()
                    select i;
            if (g.Count() - 1 > 0)
            {
                db.SubOfficialEventLists.Remove(q.First());
                db.SaveChanges();
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        public IActionResult subEventEdit(int? id)
        {
            ViewBag.Id = id;
            iSpanProjectContext db = new();
            var q = from i in db.SubOfficialEventLists
                    where i.SubOfficialEventListId == id
                    select i;
            var Q = q.First();
            return View(Q);
        }
        [HttpPost]
        public IActionResult subEventEdit(SubOfficialEventList ofevent)
        {
            iSpanProjectContext db = new();
            var ofeven = ofevent;
            var q = (from i in db.SubOfficialEventLists
                     where i.SubOfficialEventListId == ofevent.SubOfficialEventListId
                     select i).First();
            q.OfficialEventListId = ofevent.OfficialEventListId;
            q.SubEventName = ofevent.SubEventName;
            q.Discount = ofevent.Discount;
            q.IsFreeDelivery = ofevent.IsFreeDelivery;
            db.SaveChanges();
            return RedirectToAction("subEventList", new { id = ofevent.OfficialEventListId });
        }

        public List<subEventtoProductListViewModel> GetEtoPsFromDatabase(int id, string keyword)
        {
            var db = new iSpanProjectContext();
            List<subEventtoProductListViewModel> list = new();
            IQueryable<SubOfficialEventToProduct> EtoPs = null;
            if (keyword == null)
            {
                EtoPs = db.SubOfficialEventToProducts.Where(i => i.SubOfficialEventListId == id).Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                EtoPs = db.SubOfficialEventToProducts.
                     Where(i => i.SubOfficialEventListId == id && i.SubOfficialEventListId == key).
                     Select(e => e);
            }
            else
            {
                EtoPs = db.SubOfficialEventToProducts.
                    Where(i => i.SubOfficialEventListId == id && (i.Product.ProductName.Contains(keyword) || i.SubOfficialEventList.SubEventName.Contains(keyword))).
                    Select(e => e); ;
            }
            var ProductName =db.Products.ToList();
            var Verify = db.Verifies.ToList();
            var subEventName = db.SubOfficialEventLists.ToList();
            foreach (var EventToProds in EtoPs)
            {
                subEventtoProductListViewModel model = new()
                {
                    SubOfficialEventToProduct= EventToProds,
                    ProductName = ProductName.FirstOrDefault(I=>I.ProductId==EventToProds.ProductId).ProductName,
                    subEventName= subEventName.FirstOrDefault(i=>i.SubOfficialEventListId== EventToProds.SubOfficialEventListId).SubEventName,
                    Verify= Verify.FirstOrDefault(i=>i.VerifyId==EventToProds.VerifyId).VerifyName,
                };
                list.Add(model);
            }
            return list;
        }
        #endregion
        #region SubEventToProductRegion
        protected IPagedList<subEventtoProductListViewModel> GetEtoPsPagedProcess(int id, int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetEtoPsFromDatabase(id, keyword);
            IPagedList<subEventtoProductListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult subEventtoProductList(int id, string keyword, int? pageSize, int? page = 1)
        {
            iSpanProjectContext db = new();
            ViewBag.pageSize = pageSize;
            ViewBag.Verifies = db.Verifies.ToList();
            ViewBag.subEvent = db.SubOfficialEventLists.FirstOrDefault(i => i.SubOfficialEventListId == id).OfficialEventListId;
            //每頁幾筆
            pageSize ??= 5;//if null的寫法
            page ??= 1;
            //處理頁數
            ViewBag.Prods = GetEtoPsPagedProcess(id, page, (int)pageSize, keyword);
            var PList = GetEtoPsPagedProcess(id, page, (int)pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        public IActionResult EtoPApprove(int id)
        {
            iSpanProjectContext db = new();
            var prod = db.SubOfficialEventToProducts.FirstOrDefault(i => i.SubOfficialEventToProductId == id);
            prod.VerifyId = 2;
            var p = db.Products.FirstOrDefault(i => i.ProductId == prod.ProductId);
            db.SaveChanges();
            return Content($"{p}");
        }
        public IActionResult EtoPDeny(int id)
        {
            iSpanProjectContext db = new();
            var prod = db.SubOfficialEventToProducts.FirstOrDefault(i => i.SubOfficialEventToProductId == id);
            prod.VerifyId = 3;
            var p = db.Products.FirstOrDefault(i => i.ProductId == prod.ProductId);
            db.SaveChanges();
            return Content($"{p}");
        }
        #endregion
    }
}
