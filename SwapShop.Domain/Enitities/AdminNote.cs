using Swap_Shop.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapShop.Domain.Enitities
{
    public class AdminNote : BaseEntity
    {
        public string AdminId { get; set; }
        [ForeignKey("AdminId")]
        public ApplicationUser Admin { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
        public string Level { get; set; }
    }
}
