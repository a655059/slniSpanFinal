using System.Collections.Generic;

namespace prjiSpanFinal.ViewModels.Order
{
    public class OrderReturnViewModel
    {
        public string TypeName { get; set; }
        public string ReasonName { get; set; }
        public string ReasonText { get; set; }
        public List<byte[]> pics { get; set; }
        public int TypeID { get; set; }
    }
}
