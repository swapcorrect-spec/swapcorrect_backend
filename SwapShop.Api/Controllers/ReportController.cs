using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Application.Commands.Report;
using SwapShop.Application.Commands.UserReviewRating;
using SwapShop.Application.Queries.Report;
using SwapShop.Domain.Dtos.Request.Report;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("user")]
        public Task<IActionResult> ReportUser(ReportUserDto req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new ReportUserCommand()
            {
                UserId = userid,
                req = req
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }

        [HttpPost("Add/report/note")]
        public Task<IActionResult> AddReview(AdminReportNoteCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [HttpPost("change/report/status")]
        public Task<IActionResult> ChangeReportStatus(ChangeReportStatusCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [HttpGet("paginated/all")]
        public Task<IActionResult> GetAllReportOnAdmin([FromQuery]SearchUserReportPaginatedQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [HttpGet("single/details")]
        public Task<IActionResult> GetSingleReportDetails([FromQuery]GetSingleReportDetailsQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
    }
}
