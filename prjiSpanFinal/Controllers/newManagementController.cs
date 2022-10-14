using Microsoft.AspNetCore.Mvc;
using MvcPaging;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModel;
using System.Linq;


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
            C.Products = Q.ToPagedList(C.Page > 0 ? C.Page - 1 : 0, PageSize); ;
            int ID=0;
            C.id = ID;
            return PartialView(C);
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
    }
}
