using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.CategoryItemSort
{
    public class SortRequest
    {
        public List<CShowItem> SortItems(int BigTypeId, int[] filter, int priceMin,int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            var prodlist = new List<Product>();
            #region Filter (List<Product>)
            //Filter
            if (filter.Length > 0)
            {                
                foreach(var item in filter) {
                    var a = db.Products.Where(p => p.SmallTypeId == item&& p.ProductStatusId == 0).ToList();
                    prodlist.AddRange(a);
                }
            }
            else
            {
                //都要
                prodlist = db.Products.Where(p => p.SmallType.BigTypeId == BigTypeId&& p.ProductStatusId == 0).ToList();
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
                    list= list.OrderByDescending(p => p.Price.Max()).ToList();
                    break;
                case 5:
                    //價低排序
                    list = (new CHomeFactory()).toShowItem(prodlist);
                    list= list.OrderBy(p => p.Price.Min()).ToList();
                    break;
                default:
                    list = (new CHomeFactory()).toShowItem(prodlist);
                    break;
            }
            #endregion

            #region  Price Min/Max
            //Price Min/Max
            //List<CShowItem> list1;
            List<CShowItem> list2;
            //list1 = list.Where(p => p.Price.Count == 1).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax).ToList();
            list2 = list/*.Where(p => p.Price.Count == 2)*/.Where(p => p.Price.Min() >= priceMin && p.Price.Min() <= priceMax || p.Price.Max() >= priceMin && p.Price.Max() <= priceMax).ToList();
            //list1.AddRange(list2);
            #endregion

            //Pages #todo
            //TBC
            return list2;
        }
        public List<CShowItem> SearchSortItem(string keyword, int priceMin, int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            var prodlist = new List<Product>();
            #region Keyword (List<Product>)
            //keyword
            if (keyword != null)
            {
                keyword.Trim();
                string[] keys = keyword.Split(" ");
                for (int i = 0; i < keys.Length; i++)
                {
                    prodlist.AddRange(db.Products.Where(p => p.ProductName.Contains(keys[i]) || p.Description.Contains(keys[i])&& p.ProductStatusId == 0).Select(p => p).ToList());
                }
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
            List<CShowItem> list1;
            List<CShowItem> list2;
            list1 = list.Where(p => p.Price.Count == 1).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax).ToList();
            list2 = list.Where(p => p.Price.Count == 2).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax || p.Price[1] >= priceMin && p.Price[1] <= priceMax).ToList();
            list1.AddRange(list2);
            #endregion

            //Pages #todo
            //TBC

            return list1;
        }
        public List<CShowItem> SmalltypeSortItem(int id, int priceMin, int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            var prodlist = new List<Product>();
            #region SmalltypeID (List<Product>)
            //SmalltypeID
            prodlist.AddRange(db.Products.Where(p => p.ProductStatusId == 0 && p.SmallTypeId == id).Select(p => p).ToList());
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
            List<CShowItem> list1;
            List<CShowItem> list2;
            list1 = list.Where(p => p.Price.Count == 1).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax).ToList();
            list2 = list.Where(p => p.Price.Count == 2).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax || p.Price[1] >= priceMin && p.Price[1] <= priceMax).ToList();
            list1.AddRange(list2);
            #endregion

            //Pages #todo
            //TBC

            return list1;
        }
    }
}
