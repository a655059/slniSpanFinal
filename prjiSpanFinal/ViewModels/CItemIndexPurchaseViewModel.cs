using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CItemIndexPurchaseViewModel
    {
        [Required]
        public int purchaseStyle { get; set; }
        public int purchaseProduct { get; set; }
        public int purchaseCount { get; set; }
    }
}
