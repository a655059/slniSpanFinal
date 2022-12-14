using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class C賣場ViewModel
    {
        public string? SellerName { get; set; }

        public List<string>? CourtCategoryName { get; set; }
        public string? CourtDescription { get; set; }

        public List<Card賣場ViewModel>? CardProduct { get; set; }

        public double? star;

        public byte[]? Picture { get; set; }

        public List<SmallType>? lSmallType { get; set; }

        public BigType? SearchType { get; set; }

        public SmallType? SearchSmallType { get; set; }

        public List<CShowItem>? cShowItem { get; set; }

        public string? SearchKeyword { get; set; }

        public int? SellerId { get; set; }

        public int MemberId { get; set; }
    }
}
