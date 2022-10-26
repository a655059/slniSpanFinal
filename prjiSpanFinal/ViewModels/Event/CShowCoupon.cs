using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class CShowCoupon
    {
        //優惠券名字
        public string couponEventTitle { get; set; }
        //優惠券
        public Coupon coupon { get; set; }

        //還未過期
        public bool isActive {
            get {
                DateTime today = DateTime.Now;

                TimeSpan tsExp = coupon.ExpiredDate.Subtract(today);
                double dayCountExp = tsExp.Seconds;
                if (dayCountExp >= 0) 
                    return true;
                else 
                    return false;
            }
        }
        //在開始前
        public bool isBeforeStart
        {
            get
            {
                DateTime today = DateTime.Now;
                TimeSpan tsREnd = coupon.StartDate.Subtract(today);
                double dayCountRStart = tsREnd.Seconds;

                if (dayCountRStart >= 0)
                    return true;
                else
                    return false;
            }
        }

        //是否開放領取
        public bool isPublish {
            get {
                DateTime today = DateTime.Now;
                TimeSpan tsRStart = today.Subtract(coupon.ReceiveStartDate);
                double dayCountRStart = tsRStart.Seconds;
                TimeSpan tsREnd = coupon.ReceiveEndDate.Subtract(today);
                double dayCountREnd = tsREnd.Seconds;

                if (dayCountRStart >= 0 && dayCountREnd >= 0) 
                    return true;
                else 
                    return false;
            }
        }

        //折數
        public string Discount
        {
            get { return discountformat(coupon.Discount); }
        }
        //發券者
        public MemberAccount memberSetCoupon
        {
            get
            {
                iSpanProjectContext _db = new iSpanProjectContext();                
                return _db.MemberAccounts.Where(a => a.MemberId == coupon.MemberId).FirstOrDefault();
            }
        }
        //是否為免運券
        public bool isFreeDelivery
        {
            get
            {
                return coupon.IsFreeDelivery;
            }
        }
        //低消
        public int minimumOrder
        {
            get
            {
                return coupon.MinimumOrder;
            }
        }
        //紀錄登入帳號
        public MemberAccount loggeduser { get; set; }

        //登入者是否有券
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
        //折數轉換
        private string discountformat(float discount)
        {
            string res = discount.ToString("0.##"); ;
            res = res.Substring(2);
            return res;
        }
        
    }
}
