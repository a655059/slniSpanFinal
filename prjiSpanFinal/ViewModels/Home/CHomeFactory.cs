using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CHomeFactory
    {
        public List<Product> rdnProd(List<Product> list)
        {
            if (list == null)
                return list;
            var randowlist = list.OrderBy(p => Guid.NewGuid()).ToList();
            return randowlist;
        }
        public List<CShowItem> toShowItem(List<Product> list)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            List<CShowItem> res = new List<CShowItem>();
            if (list == null)
            {
                return res;
            }
            foreach (var item in list)
            {
                if (item.ProductStatusId != 0)
                {
                    continue;
                }
                decimal x = db.ProductDetails.Where(p => p.Quantity > 0 && p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                decimal y = db.ProductDetails.Where(p => p.Quantity > 0 && p.ProductId == item.ProductId).OrderByDescending(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                byte[] pic = db.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                int sales = db.OrderDetails.Where(o => o.Order.StatusId == 7 && o.ProductDetail.ProductId == item.ProductId).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o=>o);
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
                res.Add(a);
            }
            return res;
        }

        public List<CShowItem> toShowMore(List<CShowItem> list, int a, int b)
        {
            List<CShowItem> res = list.Skip(a).Take(b).ToList();
            return res;
        }
        public List<SmallType> searchTypeSmall(BigType search)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            List<SmallType> res = db.SmallTypes.Where(t => t.BigTypeId == search.BigTypeId).ToList();
            return res;
        }
    }
}
