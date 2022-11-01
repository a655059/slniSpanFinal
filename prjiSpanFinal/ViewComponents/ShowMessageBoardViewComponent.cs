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
            byte[] userPic = null;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                string memberString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
                userPic = JsonSerializer.Deserialize<MemberAccount>(memberString).MemPic;
            }

            var firstMessageBoards = dbContext.MessageBoards.Where(i => i.ProductId == productID && i.Parent == 0).Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
                layer = 0,
                userPic = userPic,
            });
            foreach (var a in firstMessageBoards)
            {
                messageBoards.Add(a);
                GetChildren(a);
            }
            return View(messageBoards);
        }
        public void GetChildren(CMessageBoardViewModel firstMessage)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
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
                    messageBoards.Add(a);
                    GetChildren(a);
                }
            }
        }
    }
}
