using prjiSpanFinal.Models;
using System.Collections.Generic;
using System.Linq;

namespace prjiSpanFinal.ViewModel
{
    public class CProductListViewModel
    {
        public List<Product> Products { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
    }
}
