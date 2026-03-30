using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class ForgetPasswordToken : BaseEntity
    {
        public string gentoken { get; set; }
        public string token { get; set; }
        public ApplicationUser user { get; set; }
        public string userid { get; set; }
    }
}
