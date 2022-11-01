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
        public int layer { get; set; }
        public byte[] userPic { get; set; }
        public int productID { get; set; }
        public int likeCount { get; set; }
        public List<CMessageBoardViewModel> cMessageBoardList { get; set; }
    }
}
