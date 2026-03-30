using AutoMapper;
using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;


namespace SwapShop.Application.CommandHandler
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ResetPasswordCommandHandler(IMapper mapper, IAccountService accountService)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>();
            var mapRequest = _mapper.Map<ResetPassword>(request);
            var result = await _accountService.ResetPassword(mapRequest);
            if (result.StatusCode == 200)
            {
                //var mappEvent = _mapper.Map<ResetPasswordEvent>(request);
                //await _publisher.Publish(mappEvent, cancellationToken);
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
