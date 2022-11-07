using System;
using System.Collections.Generic;


namespace prjiSpanFinal.Models.App
{
    public partial class CMemberAccount
    {

        public int MemberId { get; set; }
        public string MemberAcc { get; set; }
        public string MemberPw { get; set; }
        public bool? IsTw { get; set; }
        public int RegionId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BackUpEmail { get; set; }
        public string Address { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] MemPic { get; set; }
        public int MemStatusId { get; set; }
        public int Gender { get; set; }
        public int Balance { get; set; }
        public string Description { get; set; }
        public int ReportedTime { get; set; }
        public bool IsAcceptAd { get; set; }
    }
}
