using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerADFactory
    {
        iSpanProjectContext db;
        public CSellerADFactory()
        {
            db= new iSpanProjectContext();
        }

        public List<CShowItem> fgetShowITem(List<Product> list, int nowpages)
        {
            List<CShowItem> res = ADgetShowItem(list);

            if (!list.Any())
                return res;
            return res.Skip((nowpages - 1) * 15).Take(15).ToList();
        }
        public CShowItem fgetCheckedshowItem(List<Product> list)
        {
            CShowItem res = ADgetShowItem(list).First();
            return res;
        }
        public List<Ad> fgetAD(List<Ad> res,int filter)
        {
            //switch (filter)
            //{
            //    case 1:
            //        res = res.Where(a => a.AdName == "內容特效").ToList();
            //        break;
            //    case 2:
            //        res = res.Where(a => a.AdName == "登上發燒").ToList();
            //        break;
            //    default:
            //        res = res.Where(a => a.AdName == "標題高光").ToList();
            //        break;
            //}
            return res;
        }
        public List<Ad> fgetResult(List<Ad> res ,int[] ADIDs)
        {
            res = res.Where(p => ADIDs.Contains(p.AdId)).ToList();
            return res;
        }
        public List<CShowItem> ADgetShowItem(List<Product> list)
        {
            List<CShowItem> res = new List<CShowItem>();
            if (list == null)
            {
                return res;
            }
            foreach (var item in list)
            {
                var price = db.ProductDetails.Where(p => p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice);
                decimal x = price.Min();
                decimal y = price.Max();
                byte[] pic = db.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                int sales = db.OrderDetails.Where(o => o.Order.StatusId == 7 || o.Order.StatusId == 6).Where(o => o.ProductDetail.ProductId == item.ProductId).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o => o);
                var Comments = db.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == item.ProductId);
                double stars = 0;
                if (Comments.Any())
                {
                    stars = Comments.Select(c => Convert.ToInt32(c.CommentStar)).ToList().Average();
                }
                List<decimal> dlist = new List<decimal>();
                if (x == y)
                    dlist.Add(x);
                else
                {
                    dlist.Add(x);
                    dlist.Add(y);
                }
                CShowItem a = new CShowItem();
                a.Product = item;
                a.Price = dlist;
                if (pic != null)
                    a.Pic = pic;
                a.salesVolume = sales;
                a.starCount = stars;
                res.Add(a);
            }
            return res;
        }

        public List<CADSubviewmodel> fgetSubList(int memID)
        {
            List<CADSubviewmodel> res = new List<CADSubviewmodel>();
            List<AdtoProduct> list = db.AdtoProducts.Where(p => p.Product.MemberId == memID).ToList();
            if (list.Any())
            {
                foreach(var item in list) {
                    CADSubviewmodel subs = new CADSubviewmodel
                    {
                        ADtoProd = item
                    };
                    res.Add(subs);
                }
            }
            return res;
        }
    }
}
