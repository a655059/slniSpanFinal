using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Header
{
    public class CHeader2ViewModel
    {
        public MemberAccount loggedMember
        {
            get;
            set;
        }
        public int Carts
        {
            get
            {
                iSpanProjectContext _db = new iSpanProjectContext();
                if (loggedMember != null)
                {
                    int CardCount = _db.OrderDetails.Where(o => o.Order.MemberId == loggedMember.MemberId && o.Order.StatusId == 1).Count();
                    return CardCount;
                }
                else
                    return 0;
            }
        }
    }
}
