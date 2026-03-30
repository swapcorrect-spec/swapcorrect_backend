using MediatR;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;

namespace SwapShop.Application.Commands.Report
{
    public class ChangeReportStatusCommand : IRequest<ResponseDto<string>>
    {
        public string ReportId { get; set; }
        public ReportUserStatus status { get; set; }
    }
}
