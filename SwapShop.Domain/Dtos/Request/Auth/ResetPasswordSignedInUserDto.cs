using System.ComponentModel.DataAnnotations;

namespace SwapShop.Domain.Dtos.Request.Auth
{
    public class ResetPasswordSignedInUserDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
