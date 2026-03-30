using AutoMapper;
using MediatR;
using SwapShop.Application.Queries.Auth;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.QueryHandler.Auth
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ResponseDto<LoginResultDto>>
    {

        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        public LoginQueryHandler(IMapper mapper, IAccountService accountService)
        {

            _mapper = mapper;
            _accountService = accountService;
        }
        public async Task<ResponseDto<LoginResultDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {

            var map = _mapper.Map<SignInModel>(request);
            var userLogin = await _accountService.LoginUser(map);
            return userLogin;
        }
    }
}
