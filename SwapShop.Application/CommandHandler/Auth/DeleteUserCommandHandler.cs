using AutoMapper;
using MailKit.Net.Imap;
using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public DeleteUserCommandHandler(IMapper mapper, IAccountService accountService)
        {
            _mapper = mapper;
            _accountService = accountService;
        }

        public async Task<ResponseDto<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<string>();

            var RegisterUser = await _accountService.DeleteUser(request.id);
            if (RegisterUser.StatusCode == 200)
            {
               // var result = _mapper.Map<DeleteUserEvent>(request);
               // await _publisher.Publish(mappEvent, cancellationToken);
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
