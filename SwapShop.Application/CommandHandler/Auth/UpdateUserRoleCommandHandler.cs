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
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public UpdateUserRoleCommandHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<ResponseDto<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.UpdateUserRole(request.UserId, request.Role);
        }
    }
}
