using Swap_Shop.Domain.Entities;

namespace SwapShop.Domain.Enitities
{
    public class ConfirmEmailToken : BaseEntity
    {
        public int Token { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
