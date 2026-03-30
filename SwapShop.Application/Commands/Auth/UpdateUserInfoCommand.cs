using MediatR;
using SwapShop.Domain.Dtos.Response;
using System.ComponentModel.DataAnnotations;

namespace SwapShop.Application.Commands
{
    public class UpdateUserInfoCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
