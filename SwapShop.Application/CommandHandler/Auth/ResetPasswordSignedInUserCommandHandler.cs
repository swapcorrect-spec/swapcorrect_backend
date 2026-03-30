using AutoMapper;
using MediatR;
using SwapShop.Application.Commands.Auth;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.CommandHandler.Auth
{
    public class ResetPasswordSignedInUserCommandHandler : IRequestHandler<ResetPasswordSignedInUserCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public ResetPasswordSignedInUserCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<ResponseDto<string>> Handle(ResetPasswordSignedInUserCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.ResetPasswordSignedInUser(request.UserId, request.NewPassword);
        }
    }
}
