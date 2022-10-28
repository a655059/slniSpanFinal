using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class CSalesCourtFactory
    {

        iSpanProjectContext dbContext = new iSpanProjectContext();

        public List<CShowItem> toShowItem(List<Product> list)
        {

            List<CShowItem> res = new List<CShowItem>();

            if (list == null) return res;
            foreach (var item in list)
            {
                if (item.ProductStatusId != 0) continue;
                //這邊detail中的數量  是代表一個規格還有剩下的庫存
                var price = dbContext.ProductDetails.Where(a => a.Quantity > 0 && a.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).
                    Select(a => a.UnitPrice);
                decimal x = price.Min();
                decimal y = price.Max();
                byte[] pic = dbContext.ProductPics.FirstOrDefault(a => a.ProductId == item.ProductId).Pic;
                //6 7 訂單狀態為    待評價    已完成
                int sales = dbContext.OrderDetails.Where(a => a.Order.StatusId == 7 || a.Order.StatusId == 6)
                    .Where(a => a.ProductDetail.ProductId == item.ProductId).GroupBy(a => a.Quantity).
                    Select(a => a.Key).Sum();

                double stars = 0;
                if (dbContext.Comments.Where(a => a.OrderDetail.ProductDetail.ProductId == item.ProductId).Any()) {
                    stars = dbContext.Comments.Select(a => Convert.ToDouble(a.CommentStar)).ToList().Average();
                }
                List<decimal> dlist = new List<decimal>();
                if (x == y)
                {
                    dlist.Add(x);
                }
                else {              //先加低價再加高價
                    dlist.Add(x);
                    dlist.Add(y);
                }

                CShowItem itm = new CShowItem();
                itm.Product = item;
                itm.Price = dlist;

                if (pic != null) itm.Pic = pic;
                itm.salesVolume = sales;
                itm.starCount = stars;
                res.Add(itm);


            }

            return res;

        }


        public List<SmallType> searchTypeSmall(BigType search) {
            List<SmallType> res = dbContext.SmallTypes.Where(a => a.BigTypeId == search.BigTypeId).ToList();

            return res;
        }
    }
        
}
