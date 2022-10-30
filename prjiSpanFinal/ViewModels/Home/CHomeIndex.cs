using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CHomeIndex
    {
        public List<SmallType> lSmallType { get; set; }
        public List<BigType> lBigType { get; set; }
        public List<CShowItem> cShowItem { get; set; }
        public List<WebAd> WebADCarousel{get;set;}
        public List<WebAd> WebADSmall { get; set; }
        public List<WebAd> WebADBig { get; set; }

    }
}
