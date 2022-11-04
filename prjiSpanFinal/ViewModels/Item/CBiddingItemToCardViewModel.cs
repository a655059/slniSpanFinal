using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CBiddingItemToCardViewModel
    {
        public Bidding bidding { get; set; }
        public byte[] productPic { get; set; }
        public Product product { get; set; }
        public string sellerAcc { get; set; }
        public int currentBiddingPrice { get; set; }
        public int biddingCount { get; set; }

    }
}
