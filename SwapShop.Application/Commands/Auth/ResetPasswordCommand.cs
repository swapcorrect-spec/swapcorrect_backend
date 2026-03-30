using MediatR;
using SwapShop.Domain.Dtos.Response;
using System.ComponentModel.DataAnnotations;

namespace SwapShop.Application.Commands
{
    public class ResetPasswordCommand : IRequest<ResponseDto<string>>
    {
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
