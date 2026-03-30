using MediatR;
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
    public class UnsuspendUserCommandHandler : IRequestHandler<UnSuspendUserCommand, ResponseDto<string>>
    {
        private readonly IAccountService _accountService;
        public UnsuspendUserCommandHandler(IAccountService accountService)
        {
            _accountService = accountService; 
        }
        public async Task<ResponseDto<string>> Handle(UnSuspendUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountService.UnSuspendUserAsync(request.Email);
            return result;
        }
    }
}
