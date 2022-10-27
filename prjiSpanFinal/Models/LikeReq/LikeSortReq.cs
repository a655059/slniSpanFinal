using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.LikeReq
{
    public class LikeSortReq
    {
        public List<MyLikeShowItem> MyLikeSortItems(int BigTypeId, int[] filter, int priceMin, int priceMax, int SortOrder, int pages)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            var prodlist = new List<Product>();
            #region Filter (List<Product>)
            //Filter
            if (filter.Length > 0)
            {
                foreach (var item in filter)
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

            List<MyLikeShowItem> list = (new MyLikeFactory()).toShowItem(prodlist);

            #region  SortOrder
            //SortOrder
            switch (SortOrder)
            {
                case 2:
                    //最新排序
                    list = (new MyLikeFactory()).toShowItem(prodlist.OrderByDescending(p => p.ProductId).ToList());
                    break;
                case 3:
                    //熱銷排序
                    list = ((new MyLikeFactory()).toShowItem(prodlist)).OrderByDescending(s => s.salesVolume).ToList();
                    break;
                case 4:
                    //價高排序
                    list = (new MyLikeFactory()).toShowItem(prodlist);
                    list = list.OrderByDescending(p => p.Price.Max()).ToList();
                    break;
                case 5:
                    //價低排序
                    list = (new MyLikeFactory()).toShowItem(prodlist);
                    list = list.OrderBy(p => p.Price.Min()).ToList();
                    break;
                default:
                    list = (new MyLikeFactory()).toShowItem(prodlist);
                    break;
            }
            #endregion

            #region  Price Min/Max
            //Price Min/Max
            List<MyLikeShowItem> list1;
            List<MyLikeShowItem> list2;
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

