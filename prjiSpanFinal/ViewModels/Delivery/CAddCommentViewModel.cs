using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CAddCommentViewModel
    {
        public int orderDetailID { get; set; }
        public byte[] productDetailPic { get; set; }
        public string style { get; set; }
        public string productName { get; set; }

        public List<CAddCommentViewModel> cAddComments { get; set; }
    }
}
