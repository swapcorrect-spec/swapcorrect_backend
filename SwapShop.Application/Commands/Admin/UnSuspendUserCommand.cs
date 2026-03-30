using MediatR;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Commands.Admin
{
    public class UnSuspendUserCommand : IRequest<ResponseDto<string>>
    {
        public string Email { get; set; }
    }
}
