using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Application.Commands.Auth;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands;
using SwapShop.Application.Commands.Auth;
using SwapShop.Application.Queries.Auth;
using SwapShop.Domain.Dtos.Request.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AuthController(IMediator mediator,
            IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("user/confirm-email")]
        public Task<IActionResult> ConfirmEmail(ConfirmEmailCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [HttpPost("user/register")]
        public Task<IActionResult> RegisterUser(RegisterCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [HttpPost("user/login")]
        public Task<IActionResult> LoginUser(LoginQuery req)
            => MediatorResponseHelper.Handle(_mediator, req, this);
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user/info")]
        public Task<IActionResult> UserInfo()
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var req = new UserInfoQuery()
            {
                UserId = userid
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }

        [HttpGet("general/user/info")]
        public Task<IActionResult> GeneralUserInfo(string userId)
        {

            var req = new UserInfoQuery()
            {
                UserId = userId
            };
            return MediatorResponseHelper.Handle(_mediator, req, this);
        }


        [HttpPost("user/forget_password")]
        public Task<IActionResult> ForgetPassword(ForgetPasswordCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [HttpPost("user/reset_password")]
        public Task<IActionResult> ResetPassword(ResetPasswordCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("user/update_user_info")]
        public Task<IActionResult> UpdateUserInfo(UpdateUserDto req)
        {

            var userid = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var mapData = _mapper.Map<UpdateUserInfoCommand>(req);
            mapData.UserId = userid;
            return MediatorResponseHelper.Handle(_mediator, mapData, this);
        }
        [HttpPost("user/delete_user")]
        public Task<IActionResult> DeletUser(DeleteUserCommand req)
           => MediatorResponseHelper.Handle(_mediator, req, this);

        [HttpPost("google-login")]
        public Task<IActionResult> GoogleLogin(GoogleLoginCommand req)
            => MediatorResponseHelper.Handle(_mediator, req, this);

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("update/user_role")]
        public Task<IActionResult> UpdateUserRole([FromQuery] string role)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var command = new UpdateUserRoleCommand
            {
                UserId = userId,
                Role = role
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("user/reset_password_signedIn_user")]
        public Task<IActionResult> ResetPasswordSignedInUser([FromQuery] string newPassword)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var command = new ResetPasswordSignedInUserCommand
            {
                UserId = userId,
                NewPassword = newPassword
            };

            return MediatorResponseHelper.Handle(_mediator, command, this);
        }


    }
}
