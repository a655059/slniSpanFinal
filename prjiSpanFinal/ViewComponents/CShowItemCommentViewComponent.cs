using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class CShowItemCommentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int productID, int page)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var comments = dbContext.Comments.Where(i => i.OrderDetail.ProductDetail.ProductId == productID).Select(i => new
            {
                comment = i,
                buyer = i.OrderDetail.Order.Member,
                style = i.OrderDetail.ProductDetail.Style,
            }).ToList();
            var commentPic = dbContext.CommentPics.ToList();
            List<CShowCommentViewModel> cShowCommentList = new List<CShowCommentViewModel>();
            foreach(var a in comments)
            {
                var q = commentPic.Where(i => i.CommentId == a.comment.CommentId).ToList();
                CShowCommentViewModel cShowComment = new CShowCommentViewModel
                {
                    buyer = a.buyer,
                    comment = a.comment,
                    commentPic = q,
                    style = a.style,
                    page = page,
                };
                cShowCommentList.Add(cShowComment);
            }
            List<CShowCommentViewModel> x = cShowCommentList.OrderByDescending(i => i.comment.CommentTime).Skip(10*(page-1)).Take(10).ToList();
            return View(x);
        }
    }
}
