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
        iSpanProjectContext _db;
        List<Product> listprod;
        List<CShowItem> slist;
        CCategoryIndex list;
        List<WebAd> WebadLarge;
        public CategoryController()
        {            
            _db = new iSpanProjectContext();
            listprod =new List<Product>();
            slist =new List<CShowItem>();
            list = new CCategoryIndex();
            WebadLarge = _db.WebAds.Where(a => a.IsPublishing == true).Where(a => a.MemberId == 1).Where(a => a.WebAdimageTypeId == 3).ToList();
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
            list.WebADLarge = (new CHomeFactory()).toRndImg(WebadLarge);

            return View(list);
        }
        public IActionResult SortOrder(int BigTypeId, string[] filter, int pages, int eachpage, int priceMin, int priceMax, int SortOrder)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            var prodlist = new List<Product>();
            int[] filterint = Array.ConvertAll(filter, a => int.Parse(a));
            #region Filter (List<Product>)
            //Filter
            if (filter.Length > 0)
            {
                foreach (var item in filterint)
                {
                    var a = db.Products.Where(p => p.SmallTypeId == item && p.ProductStatusId == 0).ToList();
                    prodlist.AddRange(a);
                }
            }
            else
            {
                //都要
                prodlist = db.Products.Where(p => p.SmallType.BigTypeId == BigTypeId && p.ProductStatusId == 0).ToList();
            }
            #endregion

            List<CShowItem> list = (new CHomeFactory()).toShowItem(prodlist);

            #region  SortOrder
            //SortOrder
            switch (SortOrder)
            {
                case 2:
                    //最新排序
                    list = (new CHomeFactory()).toShowItem(prodlist.OrderByDescending(p => p.ProductId).ToList());
                    break;
                case 3:
                    //熱銷排序
                    list = ((new CHomeFactory()).toShowItem(prodlist)).OrderByDescending(s => s.salesVolume).ToList();
                    break;
                case 4:
                    //價高排序
                    list = (new CHomeFactory()).toShowItem(prodlist);
                    list = list.OrderByDescending(p => p.Price.Max()).ToList();
                    break;
                case 5:
                    //價低排序
                    list = (new CHomeFactory()).toShowItem(prodlist);
                    list = list.OrderBy(p => p.Price.Min()).ToList();
                    break;
                default:
                    list = (new CHomeFactory()).toShowItem(prodlist);
                    break;
            }
            #endregion

            #region  Price Min/Max
            //Price Min/Max
            list = list.Where(p => p.Price.Min() >= priceMin && p.Price.Max() <= priceMax).ToList();
            #endregion

            //Pages #todo
            //TBC
           
            int count = list.Count();
            
            return Json(new {list = list.Skip((pages - 1) * eachpage).Take(eachpage).ToList(), count });

            //return Json(new SortRequest().SortItems(BigTypeId,filter.Select(o=>Convert.ToInt32(o)).ToArray(), pages, eachpage,priceMin, priceMax, SortOrder));
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
            list.WebADLarge = (new CHomeFactory()).toRndImg(WebadLarge);

            return View(list);
        }

        public IActionResult SearchResult(string keyword)
        {
            if(keyword == null) {
                return RedirectToAction("Index", "Home");
            }
            listprod = _db.Products.ToList();
            keyword.Trim();
            string[] keys = keyword.Split(" ");
            for (int i = 0; i < keys.Length; i++)
            {
                listprod=listprod.Where(p => p.ProductName.Contains(keys[i]) || p.Description.Contains(keys[i])&&p.ProductStatusId==0).Select(p => p).ToList();
            }
            list = new CCategoryIndex();
            if (listprod.Any()) { 
                list.cShowItem = (new CHomeFactory()).toShowItem(listprod);
            }
            list.SearchKeyword = keyword;
            list.WebADLarge = (new CHomeFactory()).toRndImg(WebadLarge);

            return View(list);
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
        public IActionResult CallItemComponent(string data)
        {
            List<CShowItem> list = JsonSerializer.Deserialize<List<CShowItem>>(data);
            //List<CShowItem> list = data;
            return ViewComponent("CategoryShow", list);
        }
    }
}
