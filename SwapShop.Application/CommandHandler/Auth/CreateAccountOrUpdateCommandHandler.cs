using MediatR;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Auth
{
    public class CreateAccountOrUpdateCommandHandler
  : IRequestHandler<CreateAccountOrUpdateCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;

        public CreateAccountOrUpdateCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public Task<ResponseDto<string>> Handle(CreateAccountOrUpdateCommand request, CancellationToken cancellationToken)
        {
            return _accountService.CreateAccountOrUpdate(request.UserId, request.requestDto);
        }
    }
}
