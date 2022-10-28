using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class COPayParametersViewModel
    {
        public string tradeNO { get; set; }
        public string tradeDate { get; set; }
        public int totalAmount { get; set; }
        public string itemName { get; set; }
       
        public string clientBackURL { get; set; }
        public string checkMacValue { get; set; }
    }
}
