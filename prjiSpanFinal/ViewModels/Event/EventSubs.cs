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

        public SubOfficialEventList SubEvent
        {
            get; set;
        }
        public List<EventShowItem> SubEventProducts
        {
            get;
            set;
        }
    }
}
