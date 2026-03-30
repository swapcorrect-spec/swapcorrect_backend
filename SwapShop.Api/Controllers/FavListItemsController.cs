using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.FavListItems;
using SwapShop.Application.Queries.FavListItems;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/listing/favourite")]
    [ApiController]
    public class FavListItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FavListItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add_fav_list")]
        public Task<IActionResult> AddFavourite(string ListId)

        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new AddFavouritesCommand()
            {
                listingId = ListId,
                userId = userid
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }


        [HttpPost("remove_fav_list")]
        public Task<IActionResult> RemoveFavourite(string ListId)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new RemoveFavouritesCommand()
            {
                listingId = ListId,
                userId = userid
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }




        [HttpGet("favourite/getUserFaviourite")]
        public Task<IActionResult> GetUserFavourite()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new UserFavouriteQuery()
            {
                UserId = userid
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }




    }
}
