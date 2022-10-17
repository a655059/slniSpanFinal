using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace prjiSpanFinal.Controllers
{
    public class newManagementController : Controller
    {
        private readonly iSpanProjectContext db;
        public newManagementController()
        {
            db = new iSpanProjectContext();
        }
        #region ProductRegion
        public IActionResult ProductList()
        {
            var Q = from i in db.Products 
                            select i;
            return View();
        }
        public IActionResult newProductList()
        {
            var C=new CProductListViewModel();
            var Q = from i in db.Products
                    select i;
           C.Products=Q.ToList();
            return View(C);
        }
        public IActionResult ProductEdit(int? id)
        {
            if (id != null)
            {
                var Q = db.Products.FirstOrDefault(i => i.ProductId == id);
                return View(Q);
            }
            return RedirectToAction("ProductList");
        }
        #endregion
        #region
        public async Task<IActionResult> Index(int? page = 1)
        {
            //每頁幾筆
            const int pageSize = 20;
            //處理頁數
            ViewBag.Prods = GetPagedProcess(page, pageSize);
            //填入頁面資料
            return View(await db.Products.Skip<Product>(pageSize * ((page ?? 1) - 1)).Take(pageSize).ToListAsync());
        }
        protected IPagedList<Product> GetPagedProcess(int? page, int pageSize)
        {
            // 過濾從client傳送過來有問題頁數
            if (page.HasValue && page < 1)
                return null;
            // 從資料庫取得資料
            var listUnpaged = GetStuffFromDatabase();
            IPagedList<Product> pagelist = listUnpaged.ToPagedList(page ?? 1, pageSize);
            // 過濾從client傳送過來有問題頁數，包含判斷有問題的頁數邏輯
            if (pagelist.PageNumber != 1 && page.HasValue && page > pagelist.PageCount)
                return null;
            return pagelist;
        }
        protected IQueryable<Product> GetStuffFromDatabase()
        {
            return db.Products;
        }
        #endregion
    }
}
