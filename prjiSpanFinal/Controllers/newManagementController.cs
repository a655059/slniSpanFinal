using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace prjiSpanFinal.Controllers
{
    public class newManagementController : Controller
    {
        #region ProductRegion
        public List<CProductListViewModel> GetStuffFromDatabase()
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
                                     select i.SmallTypeName).First()
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
            var listUnpaged = GetStuffFromDatabase();
            IPagedList<CProductListViewModel> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        public async Task<IActionResult> newProductList(int? page = 1)
        {
            var db = new iSpanProjectContext();
            //每頁幾筆
            const int pageSize = 20;
            //處理頁數
            ViewBag.Prods = GetPagedProcess(page, pageSize);
           var PList = GetPagedProcess(page, pageSize);
            //填入頁面資料
            return View(PList);
        }
        #endregion
    }
}
