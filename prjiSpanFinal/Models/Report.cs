using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int ReporterId { get; set; }
        public int ProductId { get; set; }
        public int ReportTypeId { get; set; }
        public string Reason { get; set; }
        public byte[] ReportPic { get; set; }
        public int? ReportStatusId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ReportStatus ReportStatus { get; set; }
        public virtual ReportType ReportType { get; set; }
        public virtual MemberAccount Reporter { get; set; }
    }
}
