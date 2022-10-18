using System;
using System.Collections.Generic;

namespace prjiSpanFinal.ViewModels.Popup
{
    public class PopupViewModel
    {
        public int MemID { get; set; }
        public string MemAcc { get; set; }
        public byte[] MemPic { get; set; }
        public List<int> CMemID { get; set; }
        public List<string> CMemAcc { get; set; }
        public List<string> COneMsg { get; set; }
        public List<byte[]> CMemPic { get; set; }
        public List<int> CHaveread { get; set; }
    }
}
