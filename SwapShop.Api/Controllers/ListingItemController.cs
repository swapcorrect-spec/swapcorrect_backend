using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.ListItem;
using SwapShop.Application.Queries.ListingItem;
using SwapShop.Domain.Dtos.Request.ListingItem;
using SwapShop.Domain.Dtos.Response;
using SwapShop.Domain.Enum;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Route("api/listing_item")]
    [ApiController]
    public class ListingItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ListingItemController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("create")]
        public Task<IActionResult> CreateListing(ListItemReq req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = _mapper.Map<ListItemCommand>(req);
            mapData.UserId = userid;
            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
       
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("start/swap_now")]
        public Task<IActionResult> Startswaping(string listingId)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new StartSwapCommand()
            {
                UserId = userid,
                ListingId = listingId
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        /// <summary>Closes only the swap proceeding; the listing remains independently available.</summary>
        /// <param name="swapId">The swap proceeding ID, not the listing ID.</param>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("close/swap_now")]
        [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseDto<string>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> CloseSwaping([FromQuery] string swapId)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = new CloseSwapCommand()
            {
                UserId = userid,
                SwapId = swapId
            };

            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }

        [HttpGet("GetItemByRaterHotPick")]
        public Task<IActionResult> GetItemByRaterHotPick(int limit, string? userId)
        {
            var req = new GetItemByRaterHotPickQuery
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value : null,
                limit = limit
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [HttpGet("listing_details")]
        public Task<IActionResult> GetSingleListingAsync(string listingId, string? userId)
        {
            var req = new GetSingleListingQuery
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value : null,
                ListingId = listingId
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
        [HttpGet("category/all")]
        public Task<IActionResult> GetAllItemCategory()
        {
            var req = new GetAllCategoryQuery
            {

            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
        [HttpGet("recommended")]
        public Task<IActionResult> GetRecommendedItem(int limit, string? userId)
        {
            var req = new GetItemByUserPreviousWantItemQuery
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value : null,
                limit = limit
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
        [HttpGet("electronic")]
        public Task<IActionResult> GetElectronicItem(int limit, string? userId)
        {
            var req = new GetItemElectronicsQuery
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value : null,
                limit = limit
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
        [HttpGet("paginated/search_item")]
        public Task<IActionResult> GetPaginatedSearchItem(string? userId, string? searhParam, string? listingUserId, string? categoryId, 
            string? location, decimal lowestRange,decimal highestRange, ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var req = new SearchpaginatedListingQuery
            {
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value : null,
                searhParam = searhParam,
                categoryId = categoryId,
                pageNumber = pageNumber,
                perpageSize = perpageSize,
                highestRange = highestRange,
                location = location,
                lowestRange = lowestRange,
                listingDateType = listingDate,
                listingUserId = listingUserId

            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update-listing")]
        public Task<IActionResult> UpdateListing([FromBody] UpdateListingReq req)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var command = new UpdateListingCommand
            {
                Req = req,
                UserId = userId
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("switch-swap-status")]
        public Task<IActionResult> SwitchSwapStatus(SwitchSwapingStatusReq switchSwapingStatusReq)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var command = new SwitchSwapStatusCommand
            {
                UserId = userId,
                SwapId = switchSwapingStatusReq.SwapId,
                Status = switchSwapingStatusReq.status 
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("single-swap-proceeding")]
        public Task<IActionResult> GetSingleSwapProceeding(string swapProceedId)
        {
            var req = new SingleSwapProceedingQuery
            {
                SwapProceedId = swapProceedId
            };
                            
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("dashboard-card")]
        public Task<IActionResult> GetUserDashboardCard()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var req = new UserDashboardCardQuery
            {
                UserId = userId
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("remove-single-listing")]
        public Task<IActionResult> RemoveSingleListing(string listingId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var command = new RemoveSingleListingCommand
            {
                UserId = userId,
                ListingId = listingId
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("close-listing")]
        public Task<IActionResult> CloseListing(string listingId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var command = new CloseListingCommand
            {
                UserId = userId,
                ListingId = listingId
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("paginated/search_swap")]
        public Task<IActionResult> SearchPaginatedListingSwap(string? listingUserId, string? searhParam, SwapListingEnumStatus swapListingStatus, 
            ListingDateFilter listingDate, int pageNumber, int perpageSize)
        {
            var req = new SearchPaginatedListingSwapQuery
            {
                CurrentUserId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value,
                ListingUserId = listingUserId,
                SearhParam = searhParam,
                SwapListingStatus = swapListingStatus,
                ListingDate = listingDate,
                PageNumber = pageNumber,
                PerPageSize = perpageSize
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }


    }
}
