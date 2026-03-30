using SwapShop.Domain.Enitities;

namespace Swap_Shop.Domain.Entities
{
    public class OTP : BaseEntity
    {
        public string token { get; set; }
        public ApplicationUser user { get; set; }
        public string userid { get; set; }
        public bool isActive { get; set; } = true;
        public string OtpReference { get; set; }
    }
}
