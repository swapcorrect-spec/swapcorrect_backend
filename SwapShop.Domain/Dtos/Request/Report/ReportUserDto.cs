using SwapShop.Domain.Dtos.Request.ListingItem;

namespace SwapShop.Domain.Dtos.Request.Report
{
    public class ReportUserDto
    {
        public string ReportedUserId { get; set; }
        public string Description { get; set; }
        public string ReportType { get; set; }
        public List<ListItemMedia> EvidenceMediaFiles { get; set; }
    }
}
