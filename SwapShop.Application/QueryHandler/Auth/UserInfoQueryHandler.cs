using MediatR;
using SwapShop.Application.Queries.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Auth
{
    public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, ResponseDto<UserInfo>>
    {
        private readonly IAccountService _accountService;


        public UserInfoQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseDto<UserInfo>> Handle(UserInfoQuery request, CancellationToken cancellationToken)
        {
            var result = await _accountService.UserInfoAsync(request.UserId);
            return result;
        }
    }
}
