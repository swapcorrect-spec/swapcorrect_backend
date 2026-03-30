using MediatR;
using SwapShop.Application.Commands.Report;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.OtherService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapShop.Application.CommandHandler.Report
{
    public class ChangeReportStatusCommandHandler
    : IRequestHandler<ChangeReportStatusCommand, ResponseDto<string>>
    {
        private readonly IReportService _reportService;

        public ChangeReportStatusCommandHandler(IReportService reportService)
        {
            _reportService = reportService;
        }

        async Task<ResponseDto<string>> IRequestHandler<ChangeReportStatusCommand, ResponseDto<string>>.Handle(ChangeReportStatusCommand request, CancellationToken cancellationToken)
        {
            return await _reportService.ChangeReportStatus(request.status, request.ReportId);
        }
    }
}
