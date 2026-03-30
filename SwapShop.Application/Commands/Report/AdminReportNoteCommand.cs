using MediatR;
using SwapShop.Domain.Dtos.Response;

namespace SwapShop.Application.Commands.Report
{
    public class AdminReportNoteCommand : IRequest<ResponseDto<string>>
    {
        public string Note { get; set; }
        public string ReportId { get; set; }
    }
}
