using MediatR;
using SwapShop.Domain.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.Commands.Auth
{
    public class UpdateUserRoleCommand : IRequest<ResponseDto<string>>
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
