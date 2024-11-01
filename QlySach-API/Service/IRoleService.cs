using QlySach_API.Model.Entity;

namespace QlySach_API.Service
{
    public interface IRoleService
    {
        bool UserHasRole(int userId, string roleName);
        Task<bool> UserHasRoleAsync(int userId, string roleName);

        bool UserHasFunctionality (int userId, Functionality functionality);
        Task<bool> UserHasFunctionalityAsync(int userId, Functionality functionality);
    }
}
