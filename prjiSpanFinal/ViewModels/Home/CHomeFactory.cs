﻿using prjiSpanFinal.Models;
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
            var randowlist = list.OrderBy(p => Guid.NewGuid()).ToList();
            return randowlist;
        }
        public List<CShowItem> toShowItem(List<Product> list)
        {
            iSpanProjectContext db = new iSpanProjectContext();
            List<CShowItem> res = new List<CShowItem>();
            foreach(var item in list)
            {
                decimal x = db.ProductDetails.Where(p => p.Quantity > 0&&p.ProductId==item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                decimal y = db.ProductDetails.Where(p => p.Quantity > 0 && p.ProductId == item.ProductId).OrderByDescending(p => p.UnitPrice).Select(p => p.UnitPrice).FirstOrDefault();
                List<decimal> dlist = new List<decimal>();
                if (x == y)
                    dlist.Add(x);
                else
                {
                    dlist.Add(x);
                    dlist.Add(y);
                }
                CShowItem a = new CShowItem
                {
                    Product=item,
                    Price = dlist,
                    Pic = db.ProductPics.Where(p=>p.ProductId==item.ProductId).Select(p => p.Pic).FirstOrDefault(),
                };
                res.Add(a);
            }
            return res;
        }

        public List<CShowItem> toShowMore(List<CShowItem> list,int a,int b)
        {
            List<CShowItem> res = list.Skip(a).Take(b).ToList();
            return res;
        }
    }
}