using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.newManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;

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
            db.ProductDetails.Remove(D.First());
            db.SaveChanges();
            return RedirectToAction("ProductDetailList", new { id = id });
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
            var MemStatuses = ( from i in db.MemStatuses select i).ToList();
            var RegionLists = ( from i in db.RegionLists select i).ToList();
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
    }
}
