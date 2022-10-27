using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class Card賣場ViewModel
    {
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        //販賣數量跟銷售應該是一樣的東西
        public int SoldQuantity { get; set; }
        public DateTime AddedTime { get; set; }
        public double StarCount { get; set; }
        public byte[] Producpic { get; set; }

    }
}
