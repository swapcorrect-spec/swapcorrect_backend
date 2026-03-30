using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Enum;

namespace SwapShop.Domain.Enitities
{
    public class UserReport : BaseEntity
    {
        public string Description { get; set; }
        public string ReportType { get; set; }
        
       public List<ReportMedia> ReportMedias { get; set; }
       public List<ReportAdminNote> ReportAdminNotes { get; set; }
        public string Status { get; set; }=
        ReportUserStatus.New.ToString();
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string ReportedUserId { get; set; }
        public ApplicationUser ReportedUser { get; set; }
    }
}
