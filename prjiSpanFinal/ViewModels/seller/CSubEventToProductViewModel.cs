using prjiSpanFinal.Models;
using System.Collections.Generic;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSubEventToProductViewModel
    {
        public SubOfficialEventList SubOfficialEventID { get; set; }
        public List<Product> Products { get; set; }
        public OfficialEventList OfficialEventList { get; set; }
    }
}
