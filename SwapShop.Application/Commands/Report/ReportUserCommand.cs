using MediatR;
using SwapShop.Domain.Dtos.Request.Report;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Report
{
    public class ReportUserCommand : IRequest<ResponseDto<string>>
    {
        public ReportUserDto req { get; set; }
        public string UserId { get; set; }
    }
}
