using CRUD.Data;
using CRUD.Model;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class UserService : IUserService
    {
        private readonly CrudDbContext _context;

        public UserService(CrudDbContext context)
        {
            _context = context;
        }

        public async Task<Crud?> GetUserFromClaimsAsync(ClaimsPrincipal claims)
        {
            // Extract the user ID from JWT claims
            var userIdClaim = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return null;

            return await _context.Cruds.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
