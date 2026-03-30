using Swap_Shop.Domain.Entities;
using SwapShop.Domain.Enitities;

namespace Austistic.Core.Entities
{

    public class UserRoom : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Room Room { get; set; }
        public string SwapperId { get; set; }
        public ApplicationUser Swapper { get; set; }
    }

  

}
