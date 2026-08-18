using Microsoft.AspNetCore.SignalR;

namespace SwapShop.Infrastructure.OtherService.Implementation
{
    public class NotificationHub : Hub
    {
        // Clients call this after connecting to associate their userId with the connection
        public async Task RegisterUser(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                // Add connection to a user-specific group so we can target by userId
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
