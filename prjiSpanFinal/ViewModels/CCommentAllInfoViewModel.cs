using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CCommentAllInfoViewModel
    {
        public CCommentViewModel CComment { get; set; }
        public List<CommentPic> CommentPics { get; set; }
        public CMemberAccountViewModel CMemberAccount { get; set; }
        public List<string> ProductStyles { get; set; }
    }
}
