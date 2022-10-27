using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class CouponFactory
    {
        iSpanProjectContext _db;

        public CouponFactory()
        {
            _db = new iSpanProjectContext();
        }

        public List<CouponViewModel> cGetCoupon(int MemID)
        {
            List<Coupon> listC = _db.CouponWallets.Where(c => c.MemberId == MemID).OrderBy(c=>c.Coupon.ExpiredDate).Select(c=>c.Coupon).ToList();
            List<CouponViewModel> listCmv = new List<CouponViewModel>();
            if (listC.Any()) { 
                foreach(var c in listC)
                {
                    CouponViewModel n = new CouponViewModel
                    {
                        coupon = c
                    };
                    listCmv.Add(n);
                }
            }
            return listCmv;
        }

        public List<CouponViewModel> cTransToViewModel(List<Coupon> list)
        {
            List<CouponViewModel> res = new List<CouponViewModel>();
            if (list.Any())
            {
                foreach (var c in list)
                {
                    CouponViewModel n = new CouponViewModel
                    {
                        coupon = c
                    };
                    res.Add(n);
                }
            }
            return res;
        }

        public List<CouponViewModel> fReturnCoupon(int filter, int sort ,int MemID)
        {
            var Couponlist = _db.CouponWallets.Where(c => c.MemberId == MemID && c.IsExpired == false).OrderBy(c => c.Coupon.ExpiredDate).Select(c => c.Coupon);
            List<CouponViewModel> resListCoupon = new List<CouponViewModel>();
            if (Couponlist == null)
            {
                return resListCoupon;
            }
            //Filter
            switch (filter)
            {
                //商場
                case 1:
                    Couponlist = Couponlist.Where(c => c.MemberId == 1);
                    break;
                //賣場
                case 2:
                    Couponlist = Couponlist.Where(c => c.MemberId != 1);
                    break;
                //預設全部
                default:
                    break;
            }
            //Sort
            switch(sort)
            {
                case 2:
                    //可使用
                    foreach (var coupons in Couponlist)
                    {
                        DateTime today = DateTime.Now;
                        TimeSpan tsREnd = coupons.ExpiredDate.Subtract(today);
                        if (tsREnd.TotalSeconds > 0)
                        {
                            CouponViewModel a = new CouponViewModel
                            {
                                coupon = coupons,
                            };
                            resListCoupon.Add(a);
                        }
                    }
                    break;
                case 3:
                    //即將到期 1個月
                    foreach (var coupons in Couponlist)
                    {
                        DateTime today = DateTime.Now;
                        TimeSpan tsREnd = coupons.ExpiredDate.Subtract(today);
                        if (tsREnd.TotalDays <= 30 && tsREnd.TotalSeconds > 0)
                        {

                        
                            CouponViewModel a = new CouponViewModel
                            {
                                coupon = coupons,
                            };
                            resListCoupon.Add(a);
                        }
                    }
                    break;
                case 4:
                    //已使用
                    Couponlist = _db.CouponWallets.Where(c => c.MemberId == MemID && c.IsExpired == true).OrderBy(c=>c.Coupon.ExpiredDate).Select(c => c.Coupon);
                    if (filter == 1)
                        Couponlist = Couponlist.Where(c => c.MemberId == 1);
                    else if (filter == 2)
                        Couponlist = Couponlist.Where(c => c.MemberId != 1);
                    resListCoupon = cTransToViewModel(Couponlist.ToList());
                    break;
                case 5:
                    //失效
                    foreach (var coupons in Couponlist)
                    {
                        DateTime today = DateTime.Now;
                        TimeSpan tsREnd = coupons.ExpiredDate.Subtract(today);
                        if (tsREnd.TotalSeconds < 0)
                        {
                            CouponViewModel a = new CouponViewModel
                            {
                                coupon = coupons,
                            };
                            resListCoupon.Add(a);
                        }
                    }
                    break;
                default:
                    resListCoupon = cTransToViewModel(Couponlist.ToList());
                    break;
            }
            return resListCoupon;
        }
    }
}
