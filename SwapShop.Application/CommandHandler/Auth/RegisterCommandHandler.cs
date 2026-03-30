using AutoMapper;
using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;


        public RegisterCommandHandler(IMapper mapper, IAccountService accountService)
        {
           
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>();
            var mapRequest = _mapper.Map<SignUp>(request);
            var RegisterUser = await _accountService.RegisterUser(mapRequest, request.Role);
            if (RegisterUser.StatusCode == 200)
            {
               
                response.DisplayMessage = RegisterUser.DisplayMessage;
                response.Result = RegisterUser.Result;
                response.StatusCode = RegisterUser.StatusCode;
                return response;
            }
            response.DisplayMessage = RegisterUser.DisplayMessage;
            response.ErrorMessages = RegisterUser.ErrorMessages;
            response.StatusCode = RegisterUser.StatusCode;
            return response;

        }
    }
}
