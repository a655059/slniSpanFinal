using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class MyCouponViewModel
    {
        public List<CouponViewModel> ListCoupon { get; set; }
        public int MemberID { get; set; }
    }
}
