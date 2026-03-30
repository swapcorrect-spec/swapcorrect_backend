using Swap_Shop.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwapShop.Domain.Enitities
{
    public class Payments : BaseEntity
    {
        public string Amount { get; set; }
        public string OrderReferenceId { get; set; }
        public string Description { get; set; }
        public string SwapId { get; set; }
        public SwappingProceeding Swap { get; set; }
        public string FeeType { get; set; }

        public string PaymentType { get; set; }
        public string RoomName { get; set; }
        public string PaymentChannel { get; set; }
        public DateTime CreatedPaymentTime { get; set; } = DateTime.UtcNow;
        public DateTime CompletePaymentTime { get; set; }
        public bool IsActive { get; set; } = true;
        public string PaymentStatus { get; set; } = "CREATED";

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
