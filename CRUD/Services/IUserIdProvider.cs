using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CRUD.Services
{
    public interface IUserIdProvider
    {
        string GetUserId(HubConnectionContext connection);
    }
}
