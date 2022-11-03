using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ShowMessageBoardViewComponent : ViewComponent
    {
        List<CMessageBoardViewModel> messageBoards = new List<CMessageBoardViewModel>();
        public async Task<IViewComponentResult> InvokeAsync(int productID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            MemberAccount user = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                user = JsonSerializer.Deserialize<MemberAccount>(memberString);
            }
            var firstMessageBoards = dbContext.MessageBoards.Where(i => i.ProductId == productID && i.Parent == 0).Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
                layer = 0,
            }).ToList();
            foreach (var a in firstMessageBoards)
            {
                var likeCounts = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == a.messageBoard.MessageBoardId).ToList();
                
                a.likeCount = 0;
                a.isLike = false;
                if (likeCounts.Count > 0)
                {
                    a.likeCount = likeCounts.Count;
                }
                if (user != null)
                {
                    var userLike = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == a.messageBoard.MessageBoardId && i.MemberId == user.MemberId).FirstOrDefault();
                    if (userLike != null)
                    {
                        a.isLike = true;
                    }
                }
                messageBoards.Add(a);
                GetChildren(a);
            }
            
            CMessageBoardViewModel cMessageBoard = new CMessageBoardViewModel
            {
                cMessageBoardList = messageBoards,
                productID = productID,
                user = user
            };
            return View(cMessageBoard);
        }
        public void GetChildren(CMessageBoardViewModel firstMessage)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            MemberAccount user = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                user = JsonSerializer.Deserialize<MemberAccount>(memberString);
            }
            var children = dbContext.MessageBoards.Where(i => i.Parent == firstMessage.messageBoard.MessageBoardId).Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
                layer = firstMessage.layer + 1
            }).ToList();
            if (children.Count > 0)
            {
                foreach (var a in children)
                {
                    var likeCounts = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == a.messageBoard.MessageBoardId).ToList();
                    a.likeCount = 0;
                    a.isLike = false;
                    if (likeCounts.Count > 0)
                    {
                        a.likeCount = likeCounts.Count;
                    }
                    if (user != null)
                    {
                        var userLike = dbContext.MessageBoardLikes.Where(i => i.MessageBoardId == a.messageBoard.MessageBoardId && i.MemberId == user.MemberId).FirstOrDefault();
                        if (userLike != null)
                        {
                            a.isLike = true;
                        }
                    }
                    messageBoards.Add(a);
                    GetChildren(a);
                }
            }
        }
    }
}
