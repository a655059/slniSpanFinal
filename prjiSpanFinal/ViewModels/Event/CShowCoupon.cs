using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class CShowCoupon
    {
        
        public string couponEventTitle { get; set; }
        public Coupon coupon { get; set; }
        public bool isActive {
            get {
                DateTime today = DateTime.Now;
                TimeSpan tsStart = today.Subtract(coupon.StartDate);
                double dayCountStart = tsStart.Days;
                TimeSpan tsExp = coupon.ExpiredDate.Subtract(today);
                double dayCountExp = tsExp.Days;

                if (dayCountStart>=0 && dayCountExp >= 0) 
                    return true;
                else 
                    return false;
            }
        }
        public bool isPublish {
            get {
                DateTime today = DateTime.Now;
                TimeSpan tsRStart = today.Subtract(coupon.ReceiveStartDate);
                double dayCountRStart = tsRStart.Days;
                TimeSpan tsREnd = coupon.ReceiveEndDate.Subtract(today);
                double dayCountREnd = tsREnd.Days;

                if (dayCountRStart >= 0 && dayCountREnd >= 0) 
                    return true;
                else 
                    return false;
            }
        }
        public string Discount
        {
            get { return discountformat(coupon.Discount); }
        }
        public MemberAccount memberSetCoupon
        {
            get
            {
                iSpanProjectContext _db = new iSpanProjectContext();                
                return _db.MemberAccounts.Where(a => a.MemberId == coupon.MemberId).FirstOrDefault();
            }
        }
        public bool isFreeDelivery
        {
            get
            {
                return coupon.IsFreeDelivery;
            }
        }
        public int minimumOrder
        {
            get
            {
                return coupon.MinimumOrder;
            }
        }
        public MemberAccount loggeduser { get; set; }
        public bool isLoggedHasCoupon
        {
            get
            {
                iSpanProjectContext _db = new iSpanProjectContext();
                if (loggeduser != null)
                {
                    if (_db.CouponWallets.Where(w => w.MemberId == loggeduser.MemberId && w.CouponId == coupon.CouponId).Any())
                        return true;
                    else
                        return false;
                }
                else
                    return false;
                
            }
            set { }
        }
        

        private string discountformat(float discount)
        {
            string res = discount.ToString("0.##"); ;
            res = res.Substring(2);
            return res;
        }
        
    }
}
