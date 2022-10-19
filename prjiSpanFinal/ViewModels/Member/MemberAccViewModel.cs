using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class MemberAccViewModel
    {
        private MemberAccount _MemAcc;
        public MemberAccount memACC
        {
            get { return _MemAcc; }
            set { _MemAcc = value; }
        }
        public MemberAccViewModel()
        {
            _MemAcc = new MemberAccount();
        }
        public int MemberId
        {
            get { return _MemAcc.MemberId; }
            set { _MemAcc.MemberId = value; }
        }
        public string MemberAcc
        {
            get { return _MemAcc.MemberAcc; }
            set { _MemAcc.MemberAcc = value; }
        }
        public string MemberPw
        {
            get { return _MemAcc.MemberPw; }
            set { _MemAcc.MemberPw = value; }
        }
        public bool? IsTw
        {
            get { return _MemAcc.IsTw; }
            set { _MemAcc.IsTw = value; }
        }
        public int RegionId
        {
            get { return _MemAcc.RegionId; }
            set { _MemAcc.RegionId = value; }
        }
        public string Phone
        {
            get { return _MemAcc.Phone; }
            set { _MemAcc.Phone = value; }
        }
        public string Email
        {
            get { return _MemAcc.Email; }
            set { _MemAcc.Email = value; }
        }
        public string BackUpEmail
        {
            get { return _MemAcc.BackUpEmail; }
            set { _MemAcc.BackUpEmail = value; }
        }
        public string Address
        {
            get { return _MemAcc.Address; }
            set { _MemAcc.Address = value; }
        }
        public string NickName
        {
            get { return _MemAcc.NickName; }
            set { _MemAcc.NickName = value; }
        }
        public string Name
        {
            get { return _MemAcc.Name; }
            set { _MemAcc.Name = value; }
        }
        public DateTime Birthday
        {
            get { return _MemAcc.Birthday; }
            set { _MemAcc.Birthday = value; }
        }
        public byte[] MemPic
        {
            get { return _MemAcc.MemPic; }
            set { _MemAcc.MemPic = value; }
        }
        public int MemStatusId
        {
            get { return _MemAcc.MemStatusId; }
            set { _MemAcc.MemStatusId = value; }
        }
        public int Gender
        {
            get { return _MemAcc.Gender; }
            set { _MemAcc.Gender = value; }
        }
        public int Balance { get; set; }
        public string ServiceTime { get; set; }
        public string SellerCaution { get; set; }
        public string AfterSales { get; set; }
        public string RenewProduct { get; set; }
        public string SellerType { get; set; }
        public string regionName { get; set; }
        public string gender { get; set; }
        public string TW { get; set; }
        public string PhoneMail { get; set; }

    }

}
