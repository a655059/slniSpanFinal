using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.LayOutReq
{
    public class SearchBar
    {
        public List<Product> PopItem5(int reqQty)
        {
            iSpanProjectContext _db = new iSpanProjectContext();
            List<Product> q = _db.AdtoProducts.Where(p => p.Product.ProductStatusId == 0).Where(p => p.IsSubActive == true).Select(p => p.Product).ToList();
            List<Product> p =_db.Products.Where(p => p.ProductStatusId == 0).Select(p => p).ToList();
            if (q.Count < reqQty)
            {
                p = (new CHomeFactory()).rdnProd(p);
                q.AddRange(p.Take(reqQty - q.Count));
            }
            return q;
        }
    }
}
