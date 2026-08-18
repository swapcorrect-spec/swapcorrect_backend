using MediatR;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseDto<LoginResultDto>>
    {
        private readonly IAccountService _accountService;

        public RefreshTokenCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseDto<LoginResultDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}
