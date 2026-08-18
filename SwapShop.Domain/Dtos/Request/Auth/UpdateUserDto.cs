
using System.ComponentModel.DataAnnotations;

namespace SwapShop.Domain.Dtos.Request.Auth
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}