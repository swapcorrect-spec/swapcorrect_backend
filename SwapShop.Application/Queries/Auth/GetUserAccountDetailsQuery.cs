using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enitities;

namespace SwapShop.Application.Queries.Auth
{
    public class GetUserAccountDetailsQuery
   : IRequest<ResponseDto<BankAccount>>
    {
        public string UserId { get; set; }
    }
}
