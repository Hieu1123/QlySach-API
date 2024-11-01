
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

        public bool UserHasFunctionality(int userId, Functionality functionality)
        {
            var user = appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == userId);

            return user?.Role.Functionalities.Contains(functionality) ?? false;  
        }

        public async Task<bool> UserHasFunctionalityAsync(int userId, Functionality functionality)
        {
            var user = await appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Role.Functionalities.Contains(functionality) ?? false;
        }

        public bool UserHasRole(int userId, string roleName)
        {
            var user = appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == userId);

            return user.Role.nameRole == roleName;
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName)
        {
            var user = await appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user.Role.nameRole == roleName;
        }
    }
}
