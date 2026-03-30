using MediatR;
using SwapShop.Application.Commands;
using SwapShop.Application.Commands.Admin;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.CommandHandler.Admin
{
    public class SuspendUserCommandHandler : IRequestHandler<SuspendUserCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        public SuspendUserCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<ResponseDto<string>> Handle(SuspendUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.SuspendUserAsync(request.Email);
            return result; 
        }
    }
}
