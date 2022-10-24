using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CSubmitCommentViewModel
    {
        public int orderDetailID { get; set; }
        public int commentStar { get; set; }
        public string quality { get; set; }
        public string colorDifference { get; set; }
        public string picMatch { get; set; }
        public string other { get; set; }
        public int shipperStar { get; set; }
        public List<IFormFile> photos { get; set; }
    }
}
