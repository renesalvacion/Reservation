using Microsoft.AspNetCore.SignalR;

namespace CRUD.Hubs
{
    public class NotificationHub : Hub
    {
        // Optionally: send a notification to a specific user
        public async Task SendNotificationToUser(int userId, string title, string message)
        {
            // Send to a group corresponding to the user
            await Clients.Group($"User-{userId}").SendAsync("ReceiveNotification", new { title, message });
        }

        // Called when user connects, we add them to their own group
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userIdString = httpContext?.Request.Query["userId"].ToString();

            if (int.TryParse(userIdString, out int userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User-{userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
