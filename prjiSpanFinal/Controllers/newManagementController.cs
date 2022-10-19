using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;

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
        public List<CProductListViewModel> GetProductsFromDatabase()
        {
            var db = new iSpanProjectContext();
            List<CProductListViewModel> list = new();
            var Prods = db.Products;
            foreach (var p in Prods)
            {
                CProductListViewModel model = new()
                {
                    Product = p,
                    ProductStatusName = (from i in db.ProductStatuses
                                         where i.ProductStatusId == p.ProductStatusId
                                         select i.ProductStatusName).First(),
                    RegionName = (from i in db.RegionLists
                                  where i.RegionId == p.RegionId
                                  select i.RegionName).First(),
                    SmallTypeName = (from i in db.SmallTypes
                                     where i.SmallTypeId == p.SmallTypeId
                                     select i.SmallTypeName).First(),
                    CustomizedCategoryName = (from i in db.CustomizedCategories
                                              where i.CustomizedCategoryId == p.CustomizedCategoryId
                                              select i.CustomizedCategoryName).First(),
                };
                list.Add(model);
            }
            return list;
        }
        protected IPagedList<CProductListViewModel> GetPagedProcess(int? page, int pageSize)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetProductsFromDatabase();
            IPagedList<CProductListViewModel> pagelist = listUnpaged.ToPagedList(page ??= 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public IActionResult newProductList(int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 3;
            //處理頁數
            ViewBag.Prods = GetPagedProcess(page, pageSize);
            var PList = GetPagedProcess(page, pageSize);
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
        public IActionResult ProductDetailList(int? id)
        {
            var db = new iSpanProjectContext();
            var Q = (from i in db.ProductDetails
                     where i.ProductId == id
                     select i);
            return View(Q);
        }
        public IActionResult ProductDetailDelete(int id)
        {
            var db = (new iSpanProjectContext());
            var D = from d in db.ProductDetails
                    where d.ProductDetailId == id
                    select d;
            db.ProductDetails.Remove(D.First());
            db.SaveChanges();
            return RedirectToAction("ProductDetailList");
        }

    }
}
