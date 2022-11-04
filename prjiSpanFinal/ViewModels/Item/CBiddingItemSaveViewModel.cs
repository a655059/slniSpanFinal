using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CBiddingItemSaveViewModel
    {
        public string itemName { get; set; }
        public int bigType { get; set; }
        public int smallType { get; set; }
        public string startPrice { get; set; }
        public DateTime startDate { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endDate { get; set; }
        public DateTime endTime { get; set; }
        public int count { get; set; }
        public string description { get; set; }
        public List<IFormFile> itemPhoto { get; set; }
    }
}
