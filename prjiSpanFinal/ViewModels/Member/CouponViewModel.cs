using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class CouponViewModel
    {
        public Coupon coupon { get; set; }
        public string StartDate {
            get {
                if (coupon != null)
                {
                    return coupon.StartDate.ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
        }
        public string ExpDate
        {
            get
            {
                if (coupon != null)
                {
                    return coupon.ExpiredDate.ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    return "";
                }
            }
        }
        public bool IsNearExpDate {
            get
            {
                if (coupon != null)
                {
                    DateTime today = DateTime.Now;
                    TimeSpan tsREnd = coupon.ExpiredDate.Subtract(today);
                    if (tsREnd.TotalDays <= 30 && tsREnd.TotalSeconds > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            
        }
        public bool IsOverExpDate { get {
                if (coupon != null) {
                    DateTime today = DateTime.Now;
                    TimeSpan tsREnd = coupon.ExpiredDate.Subtract(today);
                    if (tsREnd.TotalSeconds < 0)
                    {
                        return true;
                    }
                }
                return false;
            } }
    }
}
