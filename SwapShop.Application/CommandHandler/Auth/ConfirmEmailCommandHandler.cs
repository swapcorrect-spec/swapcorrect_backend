using MediatR;
using ProjectX.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace ProjectX.Application.CommandHandler.Auth
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;

        public ConfirmEmailCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>();

            var result = await _accountService.ConfirmEmailAsync(request.token, request.email);
            if (result.StatusCode == 200)
            {

                response.DisplayMessage = result.DisplayMessage;
                response.Result = result.Result;
                response.StatusCode = result.StatusCode;
                return response;
            }
            response.DisplayMessage = result.DisplayMessage;
            response.ErrorMessages = result.ErrorMessages;
            response.StatusCode = result.StatusCode;
            return response;
        }
    }
}
