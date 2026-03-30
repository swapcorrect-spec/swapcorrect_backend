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
using SwapShop.Application.Queries.Admin;
using SwapShop.Application.Queries.Auth;
using SwapShop.Application.Queries.ListingItem;
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
        public async Task<IActionResult> GetRecentActivities([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _mediator.Send(new RecentActivitiesQuery { PageNumber = pageNumber, PageSize = pageSize });
            return Ok(result);
        }

        [HttpGet("advanced-analytics")]
        public async Task<IActionResult> GetAdvancedAnalytics()
        {
            var result = await _mediator.Send(new AdvancedAnalyticsQuery());
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
             string? location, decimal lowestRange, decimal highestRange,SwapListingStatus swapListingStatus ,ListingDateFilter listingDate, int pageNumber, int perpageSize)
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
                swapListingStatus = swapListingStatus

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

            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
     
    }
}
