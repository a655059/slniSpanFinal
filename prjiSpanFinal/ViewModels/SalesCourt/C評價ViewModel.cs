using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class C評價ViewModel
    {
        public int Memberid { get; set; }
        //這邊應該是平均星星
        public double StarCount { get; set; }

        //根據條件產生的優良 普通 評價   不同時間限制的次數

        public BestCommentCount BestCommentCountMethod { get; set; }
        public MediumCommentCount MediumCommentCountMethod { get; set; }
        public WorstCommentCount WorstCommentCountMethod { get; set; }


        public byte[] SellerPhoto { get; set; }
        //平均出貨天數
        public string AvgShippingDate { get; set; }
        //交關過會員數
        public int BuyerCount { get; set; }
        //被加入最愛
        public int AddLoveCount { get; set; }
        //棄單次數
        public int AbandonOrder { get; set; }


        public List<Card評價內容ViewModel> CardContent { get; set; }

    }
}
