using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CShowBuyerCountViewModel
    {
        public MemberAccount buyer { get; set; }
        public int buyCount { get; set; }
        public string finishDate { get; set; }
        public int page { get; set; }
    }
}
