using MediatR;
using SwapShop.Application.Commands.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Report
{
    public class AdminReportNoteCommandHandler : IRequestHandler<AdminReportNoteCommand, ResponseDto<string>>
    {
        private readonly IReportService _reportService;

        public AdminReportNoteCommandHandler(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<ResponseDto<string>> Handle(AdminReportNoteCommand request, CancellationToken cancellationToken)
        {
            return await _reportService.AddReportReportAdminNoted(request.Note, request.ReportId);
        }
    }
}
