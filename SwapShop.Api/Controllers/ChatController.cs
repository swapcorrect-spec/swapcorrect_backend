using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Queries.Chat;
using SwapShop.Application.Queries.FavListItems;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ChatController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpGet("active/user")]
        public Task<IActionResult> GetAllActiveRoomChat()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new GetAllActiveUserRoomQuery()
            {
                UserId = userid
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
        [HttpGet("room/messages")]
        public Task<IActionResult> GetRoomMessage(string roomName)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new GetRoomMessageQuery()
            {
                UserId = userid,
                RoomName = roomName
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }
    }
}
