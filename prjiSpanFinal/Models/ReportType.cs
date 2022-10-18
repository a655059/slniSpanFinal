using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ReportType
    {
        public ReportType()
        {
            Reports = new HashSet<Report>();
        }

        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
