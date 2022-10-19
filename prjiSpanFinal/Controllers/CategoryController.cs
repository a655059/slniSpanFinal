using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Category;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class CategoryController : Controller
    {
        iSpanProjectContext _db = new iSpanProjectContext();
        List<Product> listprod;
        List<CShowItem> slist;
        CCategoryIndex list;
        public CategoryController()
        {
            listprod =new List<Product>();
            slist =new List<CShowItem>();
            list = new CCategoryIndex();
        }

        public IActionResult Index(int? id)
        {
            listprod = _db.Products.Where(p => p.SmallType.BigTypeId == id&&p.ProductStatusId==0).ToList();
            slist = (new CHomeFactory()).toShowItem(listprod);
            list = new CCategoryIndex();
            list.cShowItem = slist;
            list.SearchType = _db.BigTypes.Where(t => t.BigTypeId == id).FirstOrDefault();
            list.lSmallType = (new CHomeFactory()).searchTypeSmall(list.SearchType);

            return View(list);
        }
        [HttpGet]
        public IActionResult Index(int? id,int? sortOrder)
        {
            listprod = _db.Products.Where(p => p.SmallType.BigTypeId == id && p.ProductStatusId == 0).ToList();
            slist = (new CHomeFactory()).toShowItem(listprod);
            list = new CCategoryIndex();
            list.SearchType = _db.BigTypes.Where(t => t.BigTypeId == id).FirstOrDefault();
            list.lSmallType = (new CHomeFactory()).searchTypeSmall(list.SearchType);

            //if sort
                list.cShowItem = fSortOrder(Convert.ToInt32(sortOrder), listprod);
            
            //if pages

            return View(list);
        }
        public IActionResult SmallType(int id,int? sortOrder)
        {
            listprod = _db.Products.Where(p => p.SmallTypeId == id && p.ProductStatusId == 0).ToList();
            List<CShowItem> slist = (new CHomeFactory()).toShowItem(listprod);
            CCategoryIndex list = new CCategoryIndex();
            list.cShowItem = slist;
            list.SearchType = _db.SmallTypes.Where(t => t.SmallTypeId == id).Select(t => t.BigType).FirstOrDefault();
            list.lSmallType = (new CHomeFactory()).searchTypeSmall(list.SearchType);
            list.SearchSmallType = _db.SmallTypes.Where(t => t.SmallTypeId == id).FirstOrDefault();
            //if sort

                list.cShowItem = fSortOrder(Convert.ToInt32(sortOrder), listprod);
            return View(list);
        }
        public IActionResult SearchResult(string keyword)
        {
            if(keyword == null) {
                return RedirectToAction("Index", "Home");
            }
            List<Product> listp = _db.Products.Where(p => p.ProductName.ToUpper().Contains(keyword.ToUpper())).ToList();
            list = new CCategoryIndex();
            list.cShowItem = (new CHomeFactory()).toShowItem(listp);

            return View(listprod);
        }
        List<CShowItem> fSortOrder(int sortOrder,List<Product> listprod)
        {
            switch (sortOrder)
            {
                case 1:
                    slist = (new CHomeFactory()).toShowItem(listprod);
                    return slist;
                case 2:
                    slist = (new CHomeFactory()).toShowItem(listprod);
                    return slist.OrderByDescending(p => p.Product.ProductId).ToList();
                case 3:
                //todo#1
                //var s = _db.OrderDetails.GroupBy(o => o.ProductDetail.Product.ProductId);

                //foreach(var group in s)
                //{

                //}
                //var ss = _db.Products.Where(p=>p.ProductId==s.Select(s=>s.Key).FirstOrDefault())
                //slist = (new CHomeFactory()).toShowItem(listprod);
                //list.cShowItem = slist.OrderByDescending(p => p.Product.);
                //break;
                case 4:
                    slist = (new CHomeFactory()).toShowItem(listprod);
                    return slist.OrderByDescending(p => p.Price.Max()).ToList();
                case 5:
                    slist = (new CHomeFactory()).toShowItem(listprod);
                    return slist.OrderBy(p => p.Price.Min()).ToList();
                default:
                    slist = (new CHomeFactory()).toShowItem(listprod);
                    return slist;
            }
        }
    }
}
