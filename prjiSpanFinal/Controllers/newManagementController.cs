using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System.Linq;

namespace prjiSpanFinal.Controllers
{
    public class newManagementController : Controller
    {
        private readonly iSpanProjectContext db;
        public newManagementController(iSpanProjectContext _db)
        {
            db = _db;
        }
        #region ProductRegion
        public IActionResult ProductList()
        {
            var Q = from i in db.Products 
                            select i;
            return View(Q);
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
