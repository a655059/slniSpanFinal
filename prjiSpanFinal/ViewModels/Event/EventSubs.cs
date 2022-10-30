using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventSubs
    {
        iSpanProjectContext _db;
        public EventSubs()
        {
            _db= new iSpanProjectContext();
        }
        public SubOfficialEventList SubEvent
        {
            get; set;
        }
        public List<CShowItem> SubEventProducts
        {
            get
            {
                List<CShowItem> res =(new CHomeFactory()).toShowItem(_db.SubOfficialEventToProducts.Where(p => p.SubOfficialEventListId == SubEvent.SubOfficialEventListId).Where(p=>p.Product.ProductStatusId==0).Select(p => p.Product).ToList());

                return res;
            }
        }
    }
}
