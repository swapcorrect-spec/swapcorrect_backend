using MediatR;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Auth
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;

        public LogoutCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.LogoutAsync(request.UserId);
        }
    }
}
