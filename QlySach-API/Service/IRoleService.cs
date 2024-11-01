namespace QlySach_API.Service
{
    public interface IRoleService
    {
        bool UserHasRole(int userId, string roleName);
        Task<bool> UserHasRoleAsync(int userId, string roleName);
    }
}
