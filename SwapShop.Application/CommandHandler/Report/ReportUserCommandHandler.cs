
using MediatR;
using SwapShop.Application.Commands.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;

namespace SwapShop.Application.CommandHandler.Report
{
    public class ReportUserCommandHandler : IRequestHandler<ReportUserCommand, ResponseDto<string>>
    {
        private readonly IReportService _reportService;

        public ReportUserCommandHandler(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<ResponseDto<string>> Handle(ReportUserCommand request, CancellationToken cancellationToken)
        {
            return await _reportService.ReportUser(request.req, request.UserId);
        }
    }
}
