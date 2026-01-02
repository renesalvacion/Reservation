using CRUD.Data;
using CRUD.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;

[Authorize]
public class MessengerHub : Hub
{
    private readonly CrudDbContext _context;

    public MessengerHub(CrudDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(int? recipientId, string content)
    {
        var senderId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(senderId))
            throw new HubException("Cannot send message: UserIdentifier is null.");

        var senderName = Context.User?.FindFirst("name")?.Value ?? "Unknown";
        var senderRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value ?? "User";

        // ✅ SAVE TO DATABASE
        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            SenderName = senderName,
            SenderRole = senderRole,
            RecipientId = recipientId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        // DTO to send to clients
        var messageDto = new
        {
            chatMessage.Id,
            chatMessage.SenderId,
            chatMessage.SenderName,
            chatMessage.SenderRole,
            chatMessage.RecipientId,
            chatMessage.Content,
            chatMessage.CreatedAt
        };

        if (recipientId.HasValue)
        {
            await Clients.User(recipientId.Value.ToString())
                .SendAsync("ReceiveMessage", messageDto);

            await Clients.Caller
                .SendAsync("ReceiveMessage", messageDto);
        }
        else
        {
            await Clients.All
                .SendAsync("ReceiveMessage", messageDto);
        }
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"User connected: {Context.UserIdentifier}");
        return base.OnConnectedAsync();
    }
}
