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
        public async Task<IViewComponentResult> InvokeAsync(int productID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var firstMessageBoardIDs = dbContext.MessageBoards.Where(i => i.ProductId == productID && i.Parent == 0).Select(i => i.MessageBoardId).ToList();
            foreach (var a in firstMessageBoardIDs)
            {
                GetChildren(a);
            }
            return View(messageBoards);
        }
        //private List<int> messageBoardIDs = new List<int>();
        private List<CMessageBoardViewModel> messageBoards = new List<CMessageBoardViewModel>();
        public void GetChildren(int messageBoardID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            var messages = dbContext.MessageBoards.Select(i => new CMessageBoardViewModel
            {
                messageBoard = i,
                product = i.Product,
                member = i.Member,
            });
            var firstMessage = messages.Where(i => i.messageBoard.MessageBoardId == messageBoardID).FirstOrDefault();

            messageBoards.Add(firstMessage);
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
