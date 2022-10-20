using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Category
{
    public class CCategoryIndex
    {
        
        public List<SmallType> lSmallType { get; set; }
        public BigType SearchType { get; set; }
        public SmallType SearchSmallType { get; set; }
        public List<CShowItem> cShowItem { get; set; }
        public string SearchKeyword { get; set; }
    }
}
