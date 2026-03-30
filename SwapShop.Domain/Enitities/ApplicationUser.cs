using Microsoft.AspNetCore.Identity;
using SwapShop.Domain.Enitities;

namespace Swap_Shop.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; } 
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public bool IsSuspend { get; set; } = false;
        public bool IsFlag { get; set; } = false;
        public bool IsTwoFactorEnable { get; set; } = false;
        public DateTime LastLoginTime { get; set; } = DateTime.UtcNow;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsOnline { get; set; } = false;
        public string? LastSeen { get; set; }
        public ICollection<UserActivitylog> UserActivitylog { get; set; }
      
        public ICollection<ListingItem> UserListItems { get; set; }
        public ICollection<Saved_Item> Saved_Item { get; set; }
    }
}
