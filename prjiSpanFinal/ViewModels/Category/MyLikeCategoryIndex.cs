using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Category
{
    public class MyLikeCategoryIndex
    {
        public List<SmallType> lSmallType { get; set; }
        public BigType SearchType { get; set; }
        public SmallType SearchSmallType { get; set; }
        public List<MyLikeShowItem> MyLikeShowItem { get; set; }
        public string SearchKeyword { get; set; }
    }
}
