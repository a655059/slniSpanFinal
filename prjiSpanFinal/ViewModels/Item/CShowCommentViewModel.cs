using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CShowCommentViewModel
    {
        public MemberAccount buyer { get; set; }
        public Comment comment { get; set; }
        public List<CommentPic> commentPic { get; set; }
        public string style { get; set; }
        public int page { get; set; }
    }
}
