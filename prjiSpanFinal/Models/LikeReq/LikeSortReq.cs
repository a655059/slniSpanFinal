using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.Models.LikeReq
{
    public class LikeSortReq
    {
        public List<MylikeViewModel> SortTab(int sort, int tab, int id)
        {
            iSpanProjectContext dbcontext = new iSpanProjectContext();
            List<MylikeViewModel> q = dbcontext.Likes.Select(p=> new MylikeViewModel()
            {

                //ProductName = p.Product.ProductName,
                //ProductID = p.ProductId,
                //memberID = p.MemberId,
                //MylikeID = p.LikeId,
                //Quantity = p.Product.ProductDetails.Select(q => q.Quantity).FirstOrDefault(),
                //ProductDetailID = p.Product.ProductDetails.Select(q => q.ProductDetailId).FirstOrDefault(),
            }).ToList();
            return q;
        }
    }
}

