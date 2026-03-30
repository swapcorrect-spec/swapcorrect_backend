using SwapShop.Domain.Dtos.Request.ListingItem;

namespace SwapShop.Domain.Dtos.Response.Report
{
    public class SingleReportDetails
    {
        public string ReportId { get; set; }
        public string ReporterId { get; set; }
        public string ReporterName { get; set; }
        public string ReporterImg { get; set; }
        public string ReportedPersonId { get; set; }
        public string ReportedPersonName { get; set; }
        public string ReportedPersonImg { get; set; }
        public string ReportedPersonRating { get; set; }
        public string ReportedPersonTotalSwap { get; set; }
        public string ReportType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public List<string> Notes { get; set; }
        public List<ListItemMedia> EvidenceImg { get; set; }
        public DateTime Created { get; set; }
    }
}
