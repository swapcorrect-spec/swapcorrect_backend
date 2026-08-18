using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapShop.Api.ResponsHandler;
using SwapShop.Application.Commands.Notification;
using SwapShop.Application.Queries.Notification;
using System.IdentityModel.Tokens.Jwt;

namespace SwapShop.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Get paginated notifications for the authenticated user.</summary>
        [HttpGet]
        public Task<IActionResult> GetMyNotifications(
            [FromQuery] bool? unreadOnly,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var query = new GetUserNotificationsQuery
            {
                UserId = userId,
                UnreadOnly = unreadOnly,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return MediatorResponseHelper.Handle(_mediator, query, this);
        }

        /// <summary>Get unread notification count (badge).</summary>
        [HttpGet("unread-count")]
        public Task<IActionResult> GetUnreadCount()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var query = new GetUnreadNotificationCountQuery { UserId = userId };
            return MediatorResponseHelper.Handle(_mediator, query, this);
        }

        /// <summary>Mark a single notification as read.</summary>
        [HttpPut("{notificationId}/read")]
        public Task<IActionResult> MarkAsRead(string notificationId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var command = new MarkNotificationReadCommand
            {
                NotificationId = notificationId,
                UserId = userId
            };
            return MediatorResponseHelper.Handle(_mediator, command, this);
        }

        /// <summary>Mark all notifications as read for the authenticated user.</summary>
        [HttpPut("read-all")]
        public Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var command = new MarkAllNotificationsReadCommand { UserId = userId };
            return MediatorResponseHelper.Handle(_mediator, command, this);
        }

        /// <summary>Admin: send a notification to any user.</summary>
        [HttpPost("send")]
        public Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command)
            => MediatorResponseHelper.Handle(_mediator, command, this);
    }
}
