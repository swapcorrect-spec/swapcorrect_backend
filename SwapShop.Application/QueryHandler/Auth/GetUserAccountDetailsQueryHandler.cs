using MediatR;
using SwapShop.Application.Queries.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enitities;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Auth
{
    public class GetUserAccountDetailsQueryHandler
   : IRequestHandler<GetUserAccountDetailsQuery, ResponseDto<BankAccount>>
    {
        private readonly IAccountService _accountService;

        public GetUserAccountDetailsQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseDto<BankAccount>> Handle(GetUserAccountDetailsQuery request, CancellationToken cancellationToken)
        {

            return await _accountService.GetUserAccountDetails(request.UserId);
        }
    }
}
