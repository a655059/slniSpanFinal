using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.LikeReq
{
    public class LikeSortReq
    {
        //like篩選的所有方法
        public List<MyLikeShowItem> MyLikeSortItems( int[] filter, int priceMin, int priceMax, int SortOrder, int pages,int memberid)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            
            var mylike = new List<Product>();
            var LikeProduct = new List<Like>();
            #region Filter (List<Product>)
            //Filter
            //if (filter.Length > 0)
            //{
            //    foreach (var item in filter)
            //    {
            //        var a = db.Products.Where(p => p.SmallTypeId == item && p.ProductStatusId == 0).ToList();
            //        prodlist.AddRange(a);
            //    }
            //}

            //都要
             mylike = db.Likes.Where(p => p.MemberId == memberid).Select(p => p.Product).ToList();
            //var product_無效 = db.Likes.Where(p => p.Product.ProductStatusId == 1).ToList();
            #endregion

            List<MyLikeShowItem> list = (new MyLikeFactory()).toShowItem(mylike);

            #region  SortOrder
            //SortOrder
            switch (SortOrder)
            {
                case 1:
                    //全部按讚
                    list = (new MyLikeFactory()).toShowItem(mylike);
                    break;
                //取消按讚
                //case 6:

                //    list = (new MyLikeFactory()).toShowItem(mylike);
                //    break;
                ////無效商品
                //case 7:
                //    list = (new MyLikeFactory()).toShowItem(mylike);
                //    break;
                case 3:
                    //熱銷排序
                    list = ((new MyLikeFactory()).toShowItem(mylike)).OrderByDescending(s => s.salesVolume).ToList();
                    break;
                case 4:
                    //價高排序
                    list = (new MyLikeFactory()).toShowItem(mylike);
                    list = list.OrderByDescending(p => p.Price.Max()).ToList();
                    break;
                case 5:
                    //價低排序
                    list = (new MyLikeFactory()).toShowItem(mylike);
                    list = list.OrderBy(p => p.Price.Min()).ToList();
                    break;
                default:
                    list = (new MyLikeFactory()).toShowItem(mylike);
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

