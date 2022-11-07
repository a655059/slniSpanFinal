using prjiSpanFinal.Models;
using System.Collections.Generic;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSubEventToProductViewModel
    {
        public SubOfficialEventList SubOfficialEventID { get; set; }
        public List<Product> Products { get; set; }
        public List<OfficialEventList> OfficialEventList { get; set; }
        public List<SubOfficialEventList> SubOfficialEventList { get; set; }


        //回傳參加資料
        public int ProductID { get; set; }
        public int SubOfficialEventIDBack { get; set; }
    }
}
