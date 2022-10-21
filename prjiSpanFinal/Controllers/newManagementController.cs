using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.newManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace prjiSpanFinal.Controllers
{
    public class newManagementController : Controller
    {
        #region ProductRegion
        public List<CProductListViewModel> GetProductsFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            List<CProductListViewModel> list = new();
            IQueryable<Product> Prods = null;
            if (keyword == null)
            {
                Prods = db.Products.Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Prods = db.Products.
                     Where(i => i.ProductId == key).
                     Select(e => e);
            }
            else
            {
                Prods = db.Products.
                    Where(i => i.ProductName.Contains(keyword)).
                    Select(e => e); ;
            }
            var ProductStatusName = (from i in db.ProductStatuses select i).ToList();
            var RegionName = (from i in db.RegionLists select i).ToList();
            var SmallTypeName = (from i in db.SmallTypes select i).ToList();
            var CustomizedCategoryName = (from i in db.CustomizedCategories select i).ToList();
            foreach (var p in Prods)
            {
                CProductListViewModel model = new()
                {
                    Product = p,
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
                list.Add(model);
            }

            return list;
        }
        protected IPagedList<CProductListViewModel> GetPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetProductsFromDatabase(keyword);
            IPagedList<CProductListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult newProductList(string keyword, int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Prods = GetPagedProcess(page, pageSize, keyword);
            var PList = GetPagedProcess(page, pageSize, keyword);
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
            var X=D.First();
            var G = from g in db.Products
                    where g.ProductId == D.First().ProductId
                    select g;
            var K = from k in db.ProductDetails
                    where k.ProductId == G.First().ProductId
                    select k;
            if (K.Count() - 1 > 0)
            {
                db.ProductDetails.Remove(D.First());
            }    
            db.SaveChanges();
            return RedirectToAction("ProductDetailList", new { id=X.ProductId });
        }
        #endregion
        #region MemberRegion
        public List<CMemberListViewModel> GetMembersFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            List<CMemberListViewModel> list = new();
            IQueryable<MemberAccount> mems = null;
            if (String.IsNullOrEmpty(keyword))
            {
                mems = db.MemberAccounts.Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                mems = db.MemberAccounts.
                     Where(i => i.MemberId == key).
                     Select(e => e);
            }
            else
            {
                mems = db.MemberAccounts.
                    Where(i => i.Name.Contains(keyword) || i.Phone.Contains(keyword) || i.Email.Contains(keyword) || i.MemberAcc.Contains(keyword)).
                    Select(e => e); ;
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
        protected IPagedList<CMemberListViewModel> GetMemPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetMembersFromDatabase(keyword);
            IPagedList<CMemberListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult MemberList(string keyword, int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Prods = GetMemPagedProcess(page, pageSize, keyword);
            var PList = GetMemPagedProcess(page, pageSize, keyword);
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
        public List<COrderListViewModel> GetOrdersFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            List<COrderListViewModel> list = new();
            IQueryable<Order> Orders = null;
            if (keyword == null)
            {
                Orders = db.Orders.Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Orders = db.Orders.
                     Where(i => i.OrderId == key || i.MemberId == key).
                     Select(e => e);
            }
            else if (DateTime.TryParse(keyword, out DateTime key2))
            {
                Orders = db.Orders.
                     Where(i => i.OrderDatetime == key2).
                     Select(e => e);
            }
            else
            {
                Orders = db.Orders.
                    Where(i => i.OrderMessage.Contains(keyword)).
                    Select(e => e); ;
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
        protected IPagedList<COrderListViewModel> GetOrdersPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetOrdersFromDatabase(keyword);
            IPagedList<COrderListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult OrderList(string keyword, int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Prods = GetOrdersPagedProcess(page, pageSize, keyword);
            var PList = GetOrdersPagedProcess(page, pageSize, keyword);
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
        public IActionResult OrderStop(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 9;
            db.SaveChanges();
            return Content("1");
        }
        public IActionResult OrderDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.Orders
                    where d.OrderId == id
                    select d;
            D.First().StatusId = 10;
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
        public List<CReportListViewModel> GetReportsFromDatabase(string keyword)
        {
            var db = new iSpanProjectContext();
            List<CReportListViewModel> list = new();
            IQueryable<Report> Reps = null;
            if (keyword == null)
            {
                Reps = db.Reports.Select(i => i);
            }
            else if (int.TryParse(keyword, out int key))
            {
                Reps = db.Reports.
                     Where(i => i.ReportId == key||i.ReporterId==key).
                     Select(e => e);
            }
            else
            {
                Reps = db.Reports.
                    Where(i => i.Reason.Contains(keyword)).
                    Select(e => e); ;
            }
            var ProductName = (from i in db.Products select i).ToList();
            
            var ReportTypeName = (from i in db.ReportTypes select i).ToList();
            foreach (var p in Reps)
            {
                CReportListViewModel model = new()
                {
                    Report=p,

                    ProductName = (from i in ProductName
                                   where i.ProductId == p.ProductId
                                   select i.ProductName).First(),
                   ReportTypeName = (from i in ReportTypeName
                                     where i.ReportTypeId == p.ReportTypeId
                                     select i.ReportTypeName).First(),
                };
                list.Add(model);
            }

            return list;
        }
        protected IPagedList<CReportListViewModel> GetReportPagedProcess(int? page, int pageSize, string keyword)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetReportsFromDatabase(keyword);
            IPagedList<CReportListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult ReportList(string keyword, int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Prods = GetReportPagedProcess(page, pageSize, keyword);
            var PList = GetReportPagedProcess(page, pageSize, keyword);
            //填入頁面資料
            return View(PList);
        }
        #endregion

    }
}
