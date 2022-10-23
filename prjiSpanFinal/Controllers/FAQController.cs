using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetA(string q)
        {
            if(q== null)
            {
                return Json("");
            }
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Faqs.Where(o => o.Question == q).FirstOrDefault().Answer);
        }

        public IActionResult GetQ(string t)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            return Json(dbcontext.Faqs.Where(o => o.Faqtype.FaqtypeName == t).Select(o=>o.Question).ToList());
        }
    }


}
