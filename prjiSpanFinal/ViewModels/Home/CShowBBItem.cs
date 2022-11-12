using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CShowBBItem
    {
        public Product product { get; set; }
        public byte[] pic { get; set; }
        public string slogan { get; set; }
        public List<int> effects { get; set; }
    }
}
