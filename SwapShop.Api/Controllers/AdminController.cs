using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands;
using SwapShop.Application.Commands.Admin;
using SwapShop.Application.Commands.Auth;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Application.Commands.Report;
using SwapShop.Application.Queries.Admin;
using SwapShop.Application.Queries.Auth;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Application.Queries.Notification;
using SwapShop.Application.Queries.Payment;
using SwapShop.Application.Queries.Report;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response.Admin;
using SwapShop.Domain.Dtos.Response.Auth;
using SwapShop.Domain.Enum;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [HttpGet("admin-dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] PeriodicFilter filter)
        {
            //var result = await _mediator.Send(new GetDashboardSummaryQuery());
            //return Ok(result);
            var query = new GetDashboardSummaryQuery
            {
                filter = filter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("recent-activities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? userId = null)
        {
            var result = await _mediator.Send(new RecentActivitiesQuery { PageNumber = pageNumber, PageSize = pageSize, UserId = userId });
            return Ok(result);
        }

        [HttpGet("advanced-analytics")]
        public async Task<IActionResult> GetAdvancedAnalytics(
            [FromQuery] AnalyticsMetricFilter metricFilter = AnalyticsMetricFilter.All,
            [FromQuery] PeriodicFilter periodicFilter = PeriodicFilter.AllTime)
        {
            var result = await _mediator.Send(new AdvancedAnalyticsQuery
            {
                MetricFilter = metricFilter,
                PeriodicFilter = periodicFilter
            });
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationFilterDto filter, [FromQuery] AdminFilterType filterType = AdminFilterType.All, [FromQuery] int days = 30)
        {
            var query = new GetUsersQuery
            {
                Filter = filter,
                FilterType = filterType,
                Days = days
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("swaps-activity")]
        public async Task<IActionResult> GetSwapsActivity([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] PeriodicFilter filter = PeriodicFilter.AllTime)
        {
            var query = new SwapsActivityQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                filter = filter
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("swaps-details-chathistory/{id}")]
        public async Task<IActionResult> GetSwapDetails(string id)
        {
            var query = new SwapDetailsQuery
            {
                SwapId = id
            };

            var response = await _mediator.Send(query);
            return Ok(response);
        }


        [HttpPost("Admin/suspend-user")]
        public Task<IActionResult> SuspendUser(SuspendUserCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [HttpPost("Admin/unsuspend-user")]
        public Task<IActionResult> UnSuspendUser(UnSuspendUserCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [HttpGet("paginated/search_item")]
        public Task<IActionResult> GetPaginatedSearchItem(string? userId, string? searhParam, string? listingUserId, string? categoryId,
             string? location, decimal lowestRange, decimal highestRange, SwapListingStatus swapListingStatus, ListingReiviewStage reviewStage, ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var req = new AdminSearchpaginatedListingQuery
            {
                UserId = userId,
                searhParam = searhParam,
                categoryId = categoryId,
                pageNumber = pageNumber,
                perpageSize = perpageSize,
                highestRange = highestRange,
                location = location,
                lowestRange = lowestRange,
                listingDateType = listingDate,
                listingUserId = listingUserId,
                swapListingStatus = swapListingStatus,
                reviewStage = reviewStage

            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("review/update")]
        public Task<IActionResult> UpdateListing(UpdateListingReview req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new AdminReviewCommand()
            {
                UserId = userid,
                ListingId = req.ListingId,
                review = req.review,
                RejectionNote = req.RejectionNote,
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("flag-content")]
        public Task<IActionResult> FlagContent([FromBody] FlagContentCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("withdrawal/treat")]
        public Task<IActionResult> TreatWithdrawal([FromBody] TreatWithdrawalCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("withdrawals")]
        public Task<IActionResult> GetWithdrawals([FromQuery] GetWithdrawalsQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("transactions")]
        public Task<IActionResult> GetAllTransactions([FromQuery] GetAllTransactionsQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("transaction-stats")]
        public async Task<IActionResult> GetTransactionStats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTransactionStatsQuery(), cancellationToken);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user-stats")]
        public async Task<IActionResult> GetUserStats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserStatsQuery(), cancellationToken);
            return Ok(result);
        }

        // ── Report Management ────────────────────────────────────────────────

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("reports")]
        public Task<IActionResult> GetAllReports([FromQuery] SearchUserReportPaginatedQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("reports/details")]
        public Task<IActionResult> GetReportDetails([FromQuery] GetSingleReportDetailsQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("reports/change-status")]
        public Task<IActionResult> ChangeReportStatus([FromBody] ChangeReportStatusCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("reports/add-note")]
        public Task<IActionResult> AddReportNote([FromBody] AdminReportNoteCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        /// <summary>Get all platform notifications. Optionally filter by userId and/or type.</summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetAllNotifications(
            [FromQuery] string? userId,
            [FromQuery] string? type,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllNotificationsQuery
            {
                UserId = userId,
                Type = type,
                PageNumber = pageNumber,
                PageSize = pageSize
            }, cancellationToken);
            return Ok(result);
        }
    }
}
