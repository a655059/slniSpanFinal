using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.App
{
    public class CAppMember
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberAcc { get; set; }
        public byte[] MemberPic { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
