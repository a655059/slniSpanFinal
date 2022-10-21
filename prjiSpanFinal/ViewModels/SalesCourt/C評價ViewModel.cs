using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class C評價ViewModel
    {
        //這邊應該是平均星星
        public double StarCount { get; set; }

        //根據條件產生的優良 普通 評價

        //public Table評價ViewModel Comment;
        public List<string> BestComments { get; set; }
        public int BestCommentsCount { get; set; }

        public List<string> MediumComments { get; set; }
        public List<string> WorstComments { get; set; }
        public byte[] SellerPhoto { get; set; }
        public double? AvgShippingDate { get; set; }
        public int BuyerCount { get; set; }
        public int AddLoveCount { get; set; }
        public int? AbandonOrder { get; set; }
        //優良 普通 評價的期間
        public int? CommentPeriod { get; set; }

        public List<Card評價內容ViewModel> CardContent { get; set; }

    }
}
