namespace SwapShop.Domain.Dtos.Response.Auth
{
    public class DisplayFindUserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Gender { get; set; }
        public bool IsSuspendUser { get; set; }

        public string DeliveryAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public DateTime Created { get; set; }
    }
}