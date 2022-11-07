using Microsoft.AspNetCore.Http;
using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public bool IsAcceptAd
        {
            get { return _MemAcc.IsAcceptAd; }
            set { _MemAcc.IsAcceptAd = value; }
        }
        public int Balance { get; set; }
        public string ServiceTime { get; set; }
        public string SellerCaution { get; set; }
        public string AfterSales { get; set; }
        public string RenewProduct { get; set; }
        public string SellerType { get; set; }
        public string regionName { get; set; }
        public string countryName { get; set; }
        public string gender { get; set; }
        public string TW { get; set; }
        public string PhoneMail { get; set; }
        public IFormFile File1 { get; set; }

        public string PWHasH()
        {
            string afterhash = "abcdefghijklmnopqrstuvwxyz";
            string number = "0123456789";
            string ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random crandom = new Random();
            int sub1 = crandom.Next(2);
            int sub2 = crandom.Next(10);
            string newpw= afterhash.Substring(sub1, sub2) + number.Substring(sub1, sub2)+ABC.Substring(sub2, sub1);
            //using (SHA256 mysha256 = SHA256.Create())
            //{
            //    //現在輸入的pw變成byte陣列
            //    byte[] bytes = Encoding.UTF8.GetBytes(pw);
            //    var hash = mysha256.ComputeHash(bytes);
            //    afterhash = Convert.ToBase64String(hash);
            //}
            return newpw;
        }
    }

}
