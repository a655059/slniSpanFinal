namespace prjiSpanFinal.Models
{
    public  class CReportHu
    {
       public virtual CMemberHu CMemberHu { get; set; }
        public int ReportId { get; set; }
        public int ReporterID { get; set; }
       public virtual CProductHu CProductHu { get; set; }
        public string ProductName { get; set; }
        public string ReportType { get; set; }
        public string Reason { get; set; }
        public string ReportStatus { get; set; }
    }
}
