
using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using QlySach_API.Model.Entity;
using System.Linq;

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
        public bool UserHasFunctionality(int userId, Functionality functionality)
        {
            return appDbContext.Users
                .Include(u => u.Role)
                .Any(u => u.Id == userId && u.Role.functionalities.Contains(functionality));
        }

        public async Task<bool> UserHasFunctionalityAsync(int userId, Functionality functionality)
        {
            return await appDbContext.Users
                .Include(u => u.Role)
                .AnyAsync(u => u.Id == userId && u.Role.functionalities.Contains(functionality));
        }
    }
}
