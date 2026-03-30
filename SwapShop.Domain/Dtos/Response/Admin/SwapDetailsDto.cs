

namespace SwapShop.Domain.Dtos.Response.Admin
{
    public class SwapDetailsDto
    {
        // General Swap Information
        public string SwapId { get; set; }
        public string Status { get; set; }
        public DateTime InitiatedOn { get; set; }
        public DateTime? LastActivity { get; set; }

        // Owner Information
        public string OwnerId { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerProfilePicture { get; set; }
        public string OwnerItemId { get; set; }
        public string OwnerItemName { get; set; }
        public string OwnerItemImageUrl { get; set; }

        // Swapper Information
        public string SwapperId { get; set; }
        public string SwapperFullName { get; set; }
        public string SwapperProfilePicture { get; set; }
        public string SwapperItemId { get; set; }
        public string SwapperItemName { get; set; }
        public string SwapperItemImageUrl { get; set; }

        // Chat History
        public List<SwapChatDto> ChatHistory { get; set; } = new();
    }
}
