using MediatR;
using SwapShop.Domain.Dtos.Response;
using System.ComponentModel.DataAnnotations;

namespace SwapShop.Application.Queries.Auth
{
    public class LoginQuery : IRequest<ResponseDto<LoginResultDto>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
