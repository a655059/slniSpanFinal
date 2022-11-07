using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CHomeFactory
    {
        iSpanProjectContext db;
        public CHomeFactory()
        {
            db = new iSpanProjectContext();
        }
        public List<Product> rdnProd(List<Product> list)
        {
            if (list == null)
                return list;
            var randowlist = list.Where(p=>p.ProductStatusId!=1|| p.ProductStatusId == 2).OrderBy(p => Guid.NewGuid()).ToList();
            return randowlist;
        }
        public List<CShowItem> toShowItem(List<Product> list)
        {
            //iSpanProjectContext db = new iSpanProjectContext();
            List<CShowItem> res = new List<CShowItem>();
            if (list == null)
            {
                return res;
            }
            foreach (var item in list)
            {
                if (item.ProductStatusId == 1|| item.ProductStatusId == 2)
                {
                    continue;
                }
                var price = db.ProductDetails.Where(p => p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice);
                decimal x = price.Min();
                decimal y = price.Max();
                byte[] pic = db.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                int sales = db.OrderDetails.Where(o => o.Order.StatusId == 7 ||o.Order.StatusId==6).Where(o=> o.ProductDetail.ProductId == item.ProductId).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o=>o);
                var Comments = db.Comments.Where(c => c.OrderDetail.ProductDetail.ProductId == item.ProductId);
                double stars = 0;
                if (Comments.Any()) { 
                    stars = Comments.Select(c =>Convert.ToInt32(c.CommentStar)).ToList().Average();
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
                var Effect = db.AdtoProducts.Where(p => p.ProductId == item.ProductId && p.IsSubActive).Select(p => p.Ad.AdTypeId).ToList();
                if (Effect.Any())
                {
                    a.effects=Effect;
                }
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
            //iSpanProjectContext db = new iSpanProjectContext();
            List<SmallType> res = db.SmallTypes.Where(t => t.BigTypeId == search.BigTypeId).ToList();
            return res;
        }

        public List<WebAd> toRndImg(List<WebAd> list)
        {
            if (list == null) { 
                return list;
            }
            List<WebAd> res = list.OrderBy(p => Guid.NewGuid()).ToList();
            return res;
        }
        public List<CShowFSItem> toShowFSItem(List<Product> list)
        {
            List<CShowFSItem> res = new List<CShowFSItem>();
            if (!list.Any())
            {
                return res;
            }
            foreach(var prod in list)
            {
                if (prod.ProductStatusId == 1 || prod.ProductStatusId==2)
                {
                    continue;
                }
                CShowFSItem obj = new CShowFSItem();
                float Discount = db.SubOfficialEventToProducts.Where(e => e.ProductId == prod.ProductId && e.VerifyId == 2 && !e.SubOfficialEventList.IsFreeDelivery).Select(e => e.SubOfficialEventList.Discount).FirstOrDefault();
                bool DeliveryFree = db.SubOfficialEventToProducts.Where(e => e.ProductId == prod.ProductId && e.VerifyId == 2 && e.SubOfficialEventList.IsFreeDelivery).Select(e => e.SubOfficialEventList.IsFreeDelivery).FirstOrDefault();
                DateTime StartDate = db.SubOfficialEventToProducts.Where(e => e.ProductId == prod.ProductId&&e.VerifyId==2).Select(e => e.SubOfficialEventList.OfficialEventList.StartDate).FirstOrDefault();
                int Sale = db.OrderDetails.Where(o => o.ProductDetail.ProductId == prod.ProductId).Where(o => o.Order.StatusId == 7 || o.Order.StatusId == 6).Where(o=> (o.Order.FinishDate).CompareTo(StartDate)>0).GroupBy(o => o.Quantity).Select(o => o.Key).Sum(o => o);
                byte[] Pic = db.ProductPics.Where(p => p.ProductId == prod.ProductId).Select(p => p.Pic).FirstOrDefault();
                var Detail = db.ProductDetails.Where(p => p.ProductId == prod.ProductId);
                var Price = Detail.Select(p => p.UnitPrice);
                List<decimal> PriceList = new List<decimal>();
                if (Price.Min() == Price.Max()) { 
                    PriceList.Add(Price.Max());
                }
                else
                {
                    PriceList.Add(Price.Max());
                    PriceList.Add(Price.Min());
                }
                int Stock = Detail.Select(p => p.Quantity).Sum(q => q);

                obj.product = prod;
                obj.price = PriceList;
                if(Pic!=null)
                    obj.pic = Pic;
                obj.discount = Discount;                
                obj.stock = Stock;
                obj.sale = Sale;
                if (DeliveryFree)
                    obj.isDeliveryFree = true;
                else if (!DeliveryFree)
                    obj.isDeliveryFree = false;

                var Effect = db.AdtoProducts.Where(p => p.ProductId == prod.ProductId && p.IsSubActive).Select(p => p.Ad.AdTypeId).ToList();
                if (Effect.Any())
                {
                    obj.effects=Effect;
                }

                res.Add(obj);
            }

            return res;
        }
        public List<CShowBBItem> toShowBBItem(List<Product> list)
        {
            List<CShowBBItem> res = new List<CShowBBItem>();
            if (!list.Any())
            {
                return res;
            }
            foreach(var prod in list)
            {
                if (prod.ProductStatusId == 1 || prod.ProductStatusId == 2)
                {
                    continue;
                }
                CShowBBItem obj = new CShowBBItem();
                byte[] Pic = db.ProductPics.Where(p => p.ProductId == prod.ProductId).Select(p => p.Pic).FirstOrDefault();
                if (Pic != null)
                {
                    obj.pic = Pic;
                }
                obj.product = prod;

                var Effect = db.AdtoProducts.Where(p => p.ProductId == prod.ProductId && p.IsSubActive).Select(p => p.Ad.AdTypeId).ToList();
                if (Effect.Any())
                {
                    obj.effects = Effect;
                }

                res.Add(obj);
            }
            return res;
        }


    }
}
