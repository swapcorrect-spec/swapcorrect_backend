using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class UserActivitylog : BaseEntity
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string ActivitiesType { get; set; }
        public string ActivitiesDescription { get; set; }
    }
}
