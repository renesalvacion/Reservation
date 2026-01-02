using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CRUD.Services
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Return the NameIdentifier claim, or null if not present
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
