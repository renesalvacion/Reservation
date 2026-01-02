using CRUD.Model;
using System.Security.Claims;

namespace CRUD.Services
{
    public interface IUserService
    {
        Task<Crud?> GetUserFromClaimsAsync(ClaimsPrincipal claims);
    }
}
