using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
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
           
            return View();
        }
    }
}
