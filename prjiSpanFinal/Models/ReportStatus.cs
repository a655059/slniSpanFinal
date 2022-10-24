using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ReportStatus
    {
        public ReportStatus()
        {
            Reports = new HashSet<Report>();
        }

        public int ReportStatusId { get; set; }
        public string ReportStatusName { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
