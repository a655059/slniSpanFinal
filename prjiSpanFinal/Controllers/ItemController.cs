using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Description()
        {
            return PartialView("/Views/Item/_ItemDescriptionPartial.cshtml");
        }
        public IActionResult Comment(int? id)
        {
            int buyerNum = (int)id;
            return PartialView("/Views/Item/_ItemCommentPartial.cshtml", buyerNum);
        }
        public IActionResult BuyerCount()
        {
            return PartialView("/Views/Item/_ItemBuyerCountPartial.cshtml");
        }
    }
}
