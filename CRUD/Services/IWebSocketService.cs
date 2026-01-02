using System.Net.WebSockets;

namespace CRUD.Services
{
    public interface IWebSocketService
    {
        Task HandleUserWebSocketAsync(WebSocket socket, int userId);
        Task HandleAdminWebSocketAsync(WebSocket socket, int adminId);
    }
}
