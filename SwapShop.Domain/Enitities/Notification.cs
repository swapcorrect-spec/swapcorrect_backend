using Swap_Shop.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapShop.Domain.Enitities
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; } = "General";
        public string? ReferenceId { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
