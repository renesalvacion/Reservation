using CRUD.Data;
using CRUD.Hubs;
using CRUD.Model;
using CRUD.Services;
using Microsoft.AspNetCore.SignalR;

public class NotificationService : INotificationService
{
    private readonly CrudDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(
        CrudDbContext context,
        IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task NotifyUserAsync(int userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // ✅ THIS IS WHAT YOU WERE MISSING
        await _hubContext.Clients
            .Group($"User-{userId}")
            .SendAsync("ReceiveNotification", new
            {
                id = notification.Id,
                title,
                message,
                createdAt = notification.CreatedAt
            });
    }
}
