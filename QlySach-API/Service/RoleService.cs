
using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;

namespace QlySach_API.Service
{
    public class RoleService : IRoleService
    {   
        private readonly AppDbContext appDbContext;
        public RoleService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool UserHasRole(int userId, string roleName)
        {
            return appDbContext.Users
                .Include(u => u.Role)
                .Any(u => u.Id == userId && u.Role.nameRole == roleName);
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName)
        {
            return await appDbContext.Users
                .Include(u => u.Role)
                .AnyAsync(u => u.Id == userId && u.Role.nameRole == roleName);
        }
    }
}
