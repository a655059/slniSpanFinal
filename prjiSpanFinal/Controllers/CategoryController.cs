using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.Models.CategoryItemSort;
using prjiSpanFinal.ViewModels.Category;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            listprod = _db.Products.Where(p => p.SmallType.BigTypeId == id&&p.ProductStatusId==0).ToList();
            slist = (new CHomeFactory()).toShowItem(listprod);
            list = new CCategoryIndex();
            list.cShowItem = slist;
            list.SearchType = _db.BigTypes.Where(t => t.BigTypeId == id).FirstOrDefault();
            list.lSmallType = (new CHomeFactory()).searchTypeSmall(list.SearchType);

            return View(list);
        }
        public IActionResult SortOrder(int BigTypeId, string[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        {
            
            return Json(new SortRequest().SortItems(BigTypeId,filter.Select(o=>Convert.ToInt32(o)).ToArray(), priceMin, priceMax, SortOrder, pages));
        }
        public IActionResult SearchSort(string keyword, int priceMin, int priceMax, int SortOrder, int pages)
        {

            return Json(new SortRequest().SearchSortItem(keyword, priceMin, priceMax, SortOrder, pages));
        }
        public IActionResult SmallTypeSort(int id, int priceMin, int priceMax, int SortOrder, int pages)
        {

            return Json(new SortRequest().SmalltypeSortItem(id, priceMin, priceMax, SortOrder, pages));
        }
        public IActionResult SmallType(int? id)
        {
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            listprod = _db.Products.Where(p => p.SmallTypeId == id && p.ProductStatusId == 0).ToList();
            List<CShowItem> slist = (new CHomeFactory()).toShowItem(listprod);
            CCategoryIndex list = new CCategoryIndex();
            list.cShowItem = slist;
            list.SearchType = _db.SmallTypes.Where(t => t.SmallTypeId == id).Select(t => t.BigType).FirstOrDefault();
            list.lSmallType = (new CHomeFactory()).searchTypeSmall(list.SearchType);
            list.SearchSmallType = _db.SmallTypes.Where(t => t.SmallTypeId == id).FirstOrDefault();

            
            return View(list);
        }
        public IActionResult SearchResult(string keyword)
        {
            if(keyword == null) {
                return RedirectToAction("Index", "Home");
            }
            keyword.Trim();
            string[] keys = keyword.Split(" ");
            for (int i = 0; i < keys.Length; i++)
            {
                listprod.AddRange(_db.Products.Where(p => p.ProductName.Contains(keys[i]) || p.Description.Contains(keys[i])&&p.ProductStatusId==0).Select(p => p).ToList());
            }
            list = new CCategoryIndex();
            if (listprod.Any()) { 
                list.cShowItem = (new CHomeFactory()).toShowItem(listprod);
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
                    SIlist = (new CHomeFactory()).toShowItem(listprod.OrderByDescending(p => p.ProductId).ToList());
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
        List<Product> fFacetOrder(string keyword, List<Product> listprod)
        {
            string[] temp= keyword.Split(',');
            List<int> tempSmallTypeIDs = new List<int>();
            foreach(var Idstring in temp)
            {
                tempSmallTypeIDs.Add(Convert.ToInt32(Idstring.Substring(5)));
            }
            return listprod.Where(p => tempSmallTypeIDs.Contains(p.SmallTypeId)).ToList();
        }
        public IActionResult CBselected(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }
            List<string> temp = JsonSerializer.Deserialize<List<string>>(keyword);
            List<int> tempSmallTypeIDs = new List<int>();
            List<CShowItem> SIlist = new List<CShowItem>();
            listprod = _db.Products.ToList();
            //string[] temp = keyword.Split(',');
            foreach (var Idstring in temp)
            {
                tempSmallTypeIDs.Add(Convert.ToInt32(Idstring.Substring(5)));
            }
            
            listprod = listprod.Where(p => tempSmallTypeIDs.Contains(p.SmallTypeId)).ToList();
            SIlist = (new CHomeFactory()).toShowItem(listprod);
            string jsonString = JsonSerializer.Serialize(SIlist);

            return Json(jsonString);
        }
        public IActionResult FilterShowItem(List<CShowItem> data)
        {
            //List<CShowItem> model = JsonSerializer.Deserialize<List<CShowItem>>(Json);
            //List <CShowItem> items= JsonSerializer.Deserialize<List<string>>(Json);
            return ViewComponent("CategoryShow", data);
        }

    }
}
