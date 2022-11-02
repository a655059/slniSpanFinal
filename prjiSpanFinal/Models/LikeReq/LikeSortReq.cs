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
            //都要
             mylike = db.Likes.Where(p => p.MemberId == memberid).Select(p => p.Product).ToList();
            //var product_無效 = db.Likes.Where(p => p.Product.ProductStatusId == 1).ToList();
            #endregion

            List<MyLikeShowItem> list = (new MyLikeFactory()).toShowItem(mylike);


            //關鍵字搜尋list(product)foreach做關鍵字的比對，商品名稱/商品描述，每一個比對成功加進新的list<pd>
            //var q = db.Likes.Where(p => p.MemberId == memberid).Select(p => new
            //{
            //    name = p.Product.ProductName,
            //    price1 = p.Product.ProductDetails.Select(p => p.UnitPrice).Min(),
            //    price2 = p.Product.ProductDetails.Select(p => p.UnitPrice).Max(),
            //    sales = db.OrderDetails.Where(a => a.ProductDetail.Product.ProductId == p.ProductId).Select(a => a.Quantity).Sum(),
            //}).ToList();

            //if (!String.IsNullOrWhiteSpace(keyword))
            //{
            //    keyword.Trim();
            //    string[] keys = keyword.Split(" ");
            //    for (int i = 0; i < keys.Length; i++)
            //    {
            //        q = q.Where(a => a.name.Contains(keys[i]) || a.price1.ToString().Contains(keys[i])
            //        || a.price2.ToString().Contains(keys[i]) || a.sales.ToString().Contains(keys[i])).ToList();
            //    }
            //}


            #region  SortOrder
            //SortOrder
            switch (SortOrder)
            {
                case 1:
                    //全部按讚
                    list = (new MyLikeFactory()).toShowItem(mylike);
                    
                    break;
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
        public List<MyLikeShowItem> LikeSearchItem(int[] filter, int priceMin, int priceMax, int SortOrder, int memberid, string keyword)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            List<Product> LikeProduct = new List<Product>();
            LikeProduct = db.Likes.Where(p => p.MemberId == memberid).Select(p => p.Product).ToList();

            //關鍵字搜尋list(product)foreach做關鍵字的比對，商品名稱/商品描述，每一個比對成功加進新的list<pd>
            List<Product> ListPD = new List<Product>();
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                //keyword.Trim();
                //string[] keys = keyword.Split(" ");
                foreach (var item in LikeProduct) 
                {
                   if (item.ProductName.Contains(keyword) || item.Description.Contains(keyword))
                   {
                    ListPD.Add(item);
                   }
                }
            }
            else
            {
                foreach(var item in LikeProduct)
                {
                    ListPD.Add(item);
                }
            }
            List<MyLikeShowItem> list = (new MyLikeFactory()).SearchItem(ListPD);//有關鍵字的所有like
            //switch(小的篩選器)
            switch (SortOrder)
            {
                case 1:
                    //全部按讚
                    list = (new MyLikeFactory()).SearchItem(ListPD);
                    break;
                case 3:
                    //熱銷排序
                    list = ((new MyLikeFactory()).SearchItem(ListPD)).OrderByDescending(s => s.salesVolume).ToList();
                    break;
                case 4:
                    //價高排序

                    list = (new MyLikeFactory()).SearchItem(ListPD);
                    list = list.OrderByDescending(p => p.Price.Max()).ToList();
                    break;
                case 5:
                    //價低排序

                    list = (new MyLikeFactory()).SearchItem(ListPD);
                    list = list.OrderBy(p => p.Price.Min()).ToList();
                    break;
                default:
                    list = (new MyLikeFactory()).SearchItem(ListPD);
                    break;
            }
            //價格排序
            List<MyLikeShowItem> list1;
            List<MyLikeShowItem> list2;
            list1 = list.Where(p => p.Price.Count == 1).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax).ToList();
            list2 = list.Where(p => p.Price.Count == 2).Where(p => p.Price[0] >= priceMin && p.Price[0] <= priceMax || p.Price[1] >= priceMin && p.Price[1] <= priceMax).ToList();
            list1.AddRange(list2);
            //Pages #todo
            //List<MyLikeShowItem> list3;
            //List<MyLikeShowItem> list4;
            
            //list1 = list1.Skip((pages - 1) * eachpage).Take(eachpage).ToList();
            //list4= list2.Skip((pages - 1) * eachpage).Take(eachpage).ToList();
            //list3.AddRange(list4);
            //TBC
            return list1;
        }

    }
}

