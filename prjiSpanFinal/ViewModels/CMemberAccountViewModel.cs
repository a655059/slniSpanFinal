using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CMemberAccountViewModel
    {
        private MemberAccount _memberAccount;
        public MemberAccount memberAccount
        {
            get { return _memberAccount; }
            set { _memberAccount = value; }
        }
        public CMemberAccountViewModel()
        {
            _memberAccount = new MemberAccount();
        }
        public int MemberId 
        {
            get { return _memberAccount.MemberId; }
            set { _memberAccount.MemberId = value; }
        }
        public string MemberAcc
        {
            get { return _memberAccount.MemberAcc; }
            set { _memberAccount.MemberAcc = value; }
        }
        public string MemberPw
        {
            get { return _memberAccount.MemberPw; }
            set { _memberAccount.MemberPw = value; }
        }
        public bool? TworNot
        {
            get { return _memberAccount.TworNot; }
            set { _memberAccount.TworNot = value; }
        }
        public int RegionId
        {
            get { return _memberAccount.RegionId; }
            set { _memberAccount.RegionId = value; }
        }
        public string Phone
        {
            get { return _memberAccount.Phone; }
            set { _memberAccount.Phone = value; }
        }
        public string Email
        {
            get { return _memberAccount.Email; }
            set { _memberAccount.Email = value; }
        }
        public string BackUpEmail
        {
            get { return _memberAccount.BackUpEmail; }
            set { _memberAccount.BackUpEmail = value; }
        }
        public string Address
        {
            get { return _memberAccount.Address; }
            set { _memberAccount.Address = value; }
        }
        public string NickName
        {
            get { return _memberAccount.NickName; }
            set { _memberAccount.NickName = value; }
        }
        public string Name
        {
            get { return _memberAccount.Name; }
            set { _memberAccount.Name = value; }
        }
        public DateTime Birthday
        {
            get { return _memberAccount.Birthday; }
            set { _memberAccount.Birthday = value; }
        }
        public string Bio
        {
            get { return _memberAccount.Bio; }
            set { _memberAccount.Bio = value; }
        }
        public byte[] MemPic
        {
            get { return _memberAccount.MemPic; }
            set { _memberAccount.MemPic = value; }
        }
        public int MemStatusId
        {
            get { return _memberAccount.MemStatusId; }
            set { _memberAccount.MemStatusId = value; }
        }
    }
}
