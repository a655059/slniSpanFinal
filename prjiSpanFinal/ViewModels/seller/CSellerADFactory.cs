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
            db = new iSpanProjectContext();
        }

        public List<CADProdViewModel> fgetShowITem(List<Product> list, int nowpages)
        {
            List<CADProdViewModel> res = new List<CADProdViewModel>();
            if (!list.Any())
                return res;
            List<CShowItem> listtrans = ADgetShowItem(list);
            
            foreach (var item in listtrans)
            {
                CADProdViewModel r = new CADProdViewModel
                {
                    item = item,
                    dataCount = listtrans.Count()
                };
                res.Add(r);
            }
            return res.Skip((nowpages - 1) * 15).Take(15).ToList();
        }
        public CShowItem fgetCheckedshowItem(List<Product> list)
        {
            CShowItem res = ADgetShowItem(list).First();
            return res;
        }
        public List<CADeffectViewmodel> fgetAD(int filter)
        {
            List<Ad> list = db.Ads.Where(a => a.AdTypeId == filter).OrderBy(a => a.AdId).ToList();
            
            List<CADeffectViewmodel> res = new List<CADeffectViewmodel>();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    Adtype type = db.Adtypes.Where(a => a.AdTypeId == item.AdTypeId).FirstOrDefault();
                    CADeffectViewmodel r = new CADeffectViewmodel
                    {
                        //ADeffect = item,
                        ADID=item.AdId,
                        ADFee=item.AdFee,
                        ADPeriod=item.AdPeriod,
                        TypeName = type.AdType1,
                        TypeNameDescription = type.AdTyepDescription,
                    };
                    res.Add(r);
                }
            }
            return res;
        }
        public List<CADeffectViewmodel> fgetResult(List<Ad> list ,int[] ADIDs)
        {
            list = list.Where(p => ADIDs.Contains(p.AdId)).ToList();
            List<CADeffectViewmodel> res = new List<CADeffectViewmodel>();
            if (list.Any())
            {
                foreach(var item in list)
                {
                    Adtype type = db.Adtypes.Where(a => a.AdTypeId == item.AdTypeId).FirstOrDefault();
                    CADeffectViewmodel r = new CADeffectViewmodel()
                    {
                        ADID = item.AdId,
                        ADFee = item.AdFee,
                        ADPeriod = item.AdPeriod,
                        TypeName = type.AdType1,
                        TypeNameDescription = type.AdTyepDescription,
                    };
                    res.Add(r);
                }
            }
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
                if (item.ProductStatusId != 0)
                {
                    continue;
                }
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
            iSpanProjectContext db2 = new iSpanProjectContext();
            List<CADSubviewmodel> res = new List<CADSubviewmodel>();
            List<AdtoProduct> list = db.AdtoProducts.Where(p => p.Product.MemberId == memID).ToList();

            if (list.Any())
            {
                foreach(var item in list) {
                    var prod = db2.Products.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                    var adtype = db2.Ads.Where(a => a.AdId == item.AdId).Select(a => a.AdType.AdType1).FirstOrDefault();
                    CADSubviewmodel subs = new CADSubviewmodel
                    {
                        ADtoProd = item,
                        prod = prod,
                        adTypeName=adtype,
                    };
                    
                    //subs.adtype = adtype.Where(t => t.AdTypeId == subs.ads.AdTypeId).FirstOrDefault();
                    res.Add(subs);
                }
            }
            return res;
        }

        public List<CADSubviewmodel> fgetSubList(List<CADSubviewmodel> res,string keyword, int[] filter1, int[] filter2,int Sort, int page)
        {

            //keyword
            if (keyword != null)
            {
                keyword.Trim();
                res=res.Where(r => r.prod.ProductName.ToUpper().Contains(keyword.ToUpper())).ToList();
            }

            if (filter1.Any())
            {
                List<string> keys = new List<string>();
                foreach(var item in filter1)
                {
                    keys.Add(db.Adtypes.Where(t => t.AdTypeId == item).Select(t => t.AdType1).FirstOrDefault());
                }
                res=res.Where(r => keys.Contains(r.adTypeName)).ToList();
            }
            if (filter2.Any())
            {
                if (filter2.Length == 1)
                {
                    if (filter2[0] == 0)
                    {
                        res=res.Where(r => !r.isSubActive).ToList();
                    }
                    else if (filter2[0] == 1)
                    {
                        res=res.Where(r => r.isSubActive).ToList();
                    }
                }                
            }
            
            switch (Sort)
            {
                //單 desc 雙asc
                //編號 
                case 1:
                    res = res.OrderByDescending(r => r.ADtoProd.AdtoProductId).ToList();
                    break;
                case 2:
                    res = res.OrderBy(r => r.ADtoProd.AdtoProductId).ToList();
                    break;
                //剩餘時間
                case 3:
                    res = res.OrderByDescending(r => r.RemainingForSort).ToList();
                    break;
                case 4:
                    res = res.OrderBy(r => r.RemainingForSort).ToList();
                    break;
                //曝光
                case 5:
                    res = res.OrderByDescending(r => r.ExpoTimes).ToList();
                    break;
                case 6:
                    res = res.OrderBy(r => r.ExpoTimes).ToList();
                    break;
                //點擊
                case 7:
                    res = res.OrderByDescending(r => r.ClickTimes).ToList();
                    break;
                case 8:
                    res = res.OrderBy(r => r.ClickTimes).ToList();
                    break;

                default:
                    break;
            }

            foreach(var item in res)
            {
                item.dataCount = res.Count();
            }

            return res.Skip((page - 1) * 10).Take(10).ToList();
        }
    }
}
