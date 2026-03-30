using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;

        public ForgetPasswordCommandHandler(IAccountService accountService)
        {
           _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.ForgotPassword(request.Email);
            return result;
        }
    }
}
