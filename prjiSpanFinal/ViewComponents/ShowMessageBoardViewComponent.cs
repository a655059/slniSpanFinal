using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class ShowMessageBoardViewComponent : ViewComponent
    {
        List<CMessageBoardViewModel> messageBoards = new List<CMessageBoardViewModel>();
        public async Task<IViewComponentResult> InvokeAsync(int productID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var messages = dbContext.MessageBoards.Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
            });
            var firstMessageBoards = messages.Where(i => i.messageBoard.ProductId == productID && i.messageBoard.Parent == 0).ToList();
            foreach (var a in firstMessageBoards)
            {
                messageBoards.Add(a);
                GetChildren(a.messageBoard.MessageBoardId);
            }
            return View(messageBoards);
        }
        public void GetChildren(int messageBoardID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var messages = dbContext.MessageBoards.Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
            });
            var children = messages.Where(i => i.messageBoard.Parent == messageBoardID).ToList();
            if (children.Count > 0)
            {
                foreach (var a in children)
                {
                    messageBoards.Add(a);
                    GetChildren(a.messageBoard.MessageBoardId);
                }
            }
        }
    }
}
