using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CMessageBoardViewModel
    {
        public MessageBoard messageBoard { get; set; }
        public Product product { get; set; }
        public MemberAccount member { get; set; }
    }
}
