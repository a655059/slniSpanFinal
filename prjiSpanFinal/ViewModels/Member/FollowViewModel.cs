using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjiSpanFinal.Models;

namespace prjiSpanFinal.ViewModels.Member
{
    public class FollowViewModel
    {
        public string name { get; set; }
        public string acc { get; set; }
        public DateTime time { get; set; }
        public int id { get; set; }
        public int pid { get; set; }
        public byte[] pic { get; set; }
    }
}
