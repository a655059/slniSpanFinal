using prjiSpanFinal.Models;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace prjiSpanFinal.ViewModels.newManagement
{
    public class CCreateEventViewModel
    {
        public int OfficialEventListId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime JoinStartDate { get; set; }
        public DateTime JoinEndDate { get; set; }
        public IFormFile EventPic { get; set; }
        public virtual ICollection<SubOfficialEventList> SubOfficialEventLists { get; set; }
    }
}
