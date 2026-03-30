using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Queries.Auth
{
    public class UserInfoQuery : IRequest<ResponseDto<UserInfo>>
    {
        public string UserId { get; set; }
    }
}
