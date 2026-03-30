using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Dtos.Response.Auth;

namespace SwapShop.Application.Commands.Auth
{
    public class CreateAccountOrUpdateCommand
   : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; }
        public BankAccountSavedRequestDto requestDto { get; set; }
    }
}
