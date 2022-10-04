using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class CategoryController : Controller
    {
        List<CProduct> listpro;
        void fillin(int stock)
        {
            listpro = new List<CProduct>();
            for(int i = 1; i < (stock+1); i++) {
                CProduct A = new CProduct
                {
                    ProductId = i,
                    ProductName = "蘋果",
                    Stock = 5,
                    SellerName = "PEKO",
                    SmallType = "食物",
                    ProductStatus = "上架中",
                    UnitPrice = 999
                };
                listpro.Add(A);
            }
        }
        public IActionResult Index()
        {
            fillin(20);
            return View(listpro);
        }
    }
}
