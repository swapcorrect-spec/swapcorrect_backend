using MediatR;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Auth
{
    public class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;

        public ResendConfirmationEmailCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public Task<ResponseDto<string>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
            => _accountService.ResendConfirmationEmail(request.Email);
    }
}