using AutoMapper;
using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Domain.Dtos.Request.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler
{
    public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UpdateUserInfoCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<string>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>();
            var mapRequest = _mapper.Map<UpdateUserDto>(request);
            var result = await _accountService.UpdateUser(request.UserId, mapRequest);
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
