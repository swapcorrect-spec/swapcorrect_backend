using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.FavListItems;
using SwapShop.Application.Commands.UserReviewRating;
using SwapShop.Application.Queries.UserReviewRating;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Review/add-Review")]
        public Task<IActionResult> AddReview(AddReviewCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Review/remove-review")]
        public Task<IActionResult> RemoveReview(AddReviewCommand req)
           => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Review/all-user-review")]
        public Task<IActionResult> GetAllUserReview()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new GetAllUserReviewQuery
            {
                UserId = userId
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Review/review_by_rater")]
        public Task<IActionResult> GetReviewByRater(string raterId, string userId)
        {
            var req = new GetReviewByRaterQuery
            {
                RaterId = raterId,
                UserId = userId
            };

            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [HttpGet("Review/{reviewId}")]
        public Task<IActionResult> GetReviewById(string reviewId)
            => MediatorResponseHelper.Handle(_mediator, new GetReviewByIdQuery { ReviewId = reviewId }, this);

    }
}
