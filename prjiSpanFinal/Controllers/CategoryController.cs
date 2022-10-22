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

            //if checkedtypes

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
            //if checkedtypes

            //if pages


            return View(list);
        }
        public IActionResult SearchResult(string keyword,int? sortOrder)
        {
            if(keyword == null) {
                return RedirectToAction("Index", "Home");
            }
            listprod= _db.Products.Where(p => p.ProductName.ToUpper().Contains(keyword.ToUpper())).ToList();
            list = new CCategoryIndex();
            if (listprod.Any()) { 
                list.cShowItem = (new CHomeFactory()).toShowItem(listprod);
                //if sort
                list.cShowItem = fSortOrder(Convert.ToInt32(sortOrder), listprod);
                //if checkedtypes

                //if pages
            }
            list.SearchKeyword = keyword;


            return View(list);
        }
        List<CShowItem> fSortOrder(int sortOrder,List<Product> listprod)
        {
            List<Product> Ordered=new List<Product>();
            List<CShowItem> SIlist = new List<CShowItem>();
            switch (sortOrder)
            {
                case 1:
                    //一般排序      
                    SIlist= (new CHomeFactory()).toShowItem(listprod);
                    return SIlist;
                case 2:
                    //最新排序
                    SIlist = (new CHomeFactory()).toShowItem(listprod.OrderByDescending(p => p.ProductDetails).ToList());
                    return SIlist;
                case 3:
                    //熱銷排序
                    SIlist = ((new CHomeFactory()).toShowItem(listprod)).OrderBy(s=>s.salesVolume).ToList();
                    return SIlist;
                case 4:
                    //價高排序
                    SIlist = (new CHomeFactory()).toShowItem(listprod);
                    return SIlist.OrderByDescending(p => p.Price.Max()).ToList();
                case 5:
                    //價低排序
                    SIlist = (new CHomeFactory()).toShowItem(listprod);
                    return SIlist.OrderBy(p => p.Price.Min()).ToList();
                default:
                    SIlist = (new CHomeFactory()).toShowItem(listprod);
                    return SIlist;
            }
        }
    }
}
