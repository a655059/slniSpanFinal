using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.App
{
    public class CShowItem
    {
        public int id { get; set; }
        public string Name { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public byte[] Pic { get; set; }
        public int salesVolume { get; set; }
        public double starCount { get; set; }
        public string st { get; set; }
        public int stID { get; set; }
        public List<string> stList { get; set; }
        public List<int> stIDList { get; set; }
        public bool IsFavourite { get; set; }

        public List<int> quantiles { get; set; }

        public List<int> shipIDList { get; set; }
        public List<int> payIDList { get; set; }
        public DateTime time { get; set; }

        public List<string> styles {get; set; }

        public string selleracc { get; set; }
    }
}