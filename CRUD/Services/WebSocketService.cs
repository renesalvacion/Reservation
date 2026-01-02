using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CRUD.DTO;
using CRUD.Model;

namespace CRUD.Services
{
    public class WebSocketService : IWebSocketService
    {
        private static readonly List<ClientConnection> _clients = new();

        // Interface implementations
        public Task HandleUserWebSocketAsync(WebSocket socket, int userId)
        {
            // Use userId as token for simplicity or map it to actual JWT if needed
            string token = $"user-{userId}";
            return HandleWebSocketAsync(socket, token);
        }

        public Task HandleAdminWebSocketAsync(WebSocket socket, int adminId)
        {
            string token = $"admin-{adminId}";
            return HandleWebSocketAsync(socket, token);
        }

        // Main WebSocket handler
        public async Task HandleWebSocketAsync(WebSocket webSocket, string token)
        {
            var role = GetRoleFromToken(token);

            var client = new ClientConnection
            {
                Socket = webSocket,
                Role = role
            };

            lock (_clients)
            {
                _clients.Add(client);
            }

            Console.WriteLine($"✅ WebSocket connected ({role})");

            var buffer = new byte[4096];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer),
                        CancellationToken.None
                    );

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    var content = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var message = new WsMessageDto
                    {
                        Sender = role,
                        Content = content,
                        CreatedAt = DateTime.UtcNow
                    };

                    await BroadcastAsync(message, webSocket, role);
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"⚠️ WS error: {ex.Message}");
            }
            finally
            {
                lock (_clients)
                {
                    _clients.Remove(client);
                }

                if (webSocket.State == WebSocketState.Open ||
                    webSocket.State == WebSocketState.CloseReceived)
                {
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closed",
                        CancellationToken.None
                    );
                }

                Console.WriteLine($"❌ WebSocket disconnected ({role})");
            }
        }

        // Broadcast messages to other clients, optional role filtering
        private static async Task BroadcastAsync(WsMessageDto message, WebSocket sender, string senderRole)
        {
            var json = JsonSerializer.Serialize(message);
            var data = Encoding.UTF8.GetBytes(json);

            List<ClientConnection> closed = new();

            List<Task> sendTasks = new();

            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    if (client.Socket.State != WebSocketState.Open)
                    {
                        closed.Add(client);
                        continue;
                    }

                    if (client.Socket == sender)
                        continue; // don't echo back

                    // Example: admin sees all, user sees only user messages
                    if (senderRole == "Admin" || client.Role != "Admin")
                    {
                        sendTasks.Add(client.Socket.SendAsync(
                            new ArraySegment<byte>(data),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None
                        ));
                    }
                }

                foreach (var c in closed)
                    _clients.Remove(c);
            }

            await Task.WhenAll(sendTasks);
        }

        // Extract role from token
        public static string GetRoleFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var roleClaim = jwt.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Role ||
                    c.Type == "role" ||
                    c.Type == "roles"
                );

                return roleClaim?.Value ?? "User";
            }
            catch
            {
                return "User";
            }
        }

        private class ClientConnection
        {
            public WebSocket Socket { get; set; }
            public string Role { get; set; }
        }
    }
}
