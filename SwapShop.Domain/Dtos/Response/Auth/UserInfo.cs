namespace SwapShop.Domain.Dtos.Response
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
     
        public string Gender { get; set; }
        public bool IsSuspendUser { get; set; }
        public bool IsFlag { get; set; } = false;
        public bool IsTwoFactorEnable { get; set; }
        public DateTime LastLoginTime { get; set; } 
        public string DeliveryAddress { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public IList<string>? UserRole { get; set; }
        public int ListingCount { get; set; } 
        public int SwapCount { get; set; } 
        public double Rating { get; set; } 

        public DateTime Created { get; set; }

    }
}
