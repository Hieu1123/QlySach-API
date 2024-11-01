using Microsoft.EntityFrameworkCore;
using QlySach_API.Data;
using QlySach_API.Model.Auth;
using QlySach_API.Model.Entity;


namespace QlySach_API.Service
{
    public class AuthService
    {
        private readonly AppDbContext appDbContext;
        private readonly TokenGenerator tokenGenerator;
        public AuthService(AppDbContext appDbContext, TokenGenerator tokenGenerator)
        {
            this.appDbContext = appDbContext;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<AuthResponse> AuthenticateAsync(string username, string password)
        {
            var user = await appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.userName == username && u.Password == password);
            if (user == null)
            {
                return null;
            }

            string token = tokenGenerator.GeneratorToken(user, user.Role.nameRole);
            return new AuthResponse
            {
                jwtToken = token,
                roleName = user.Role.nameRole,
            };
        }

        public bool isUserInRole(User user, string nameRole)
        {
            return user?.Role?.nameRole == nameRole;
        }
    }
}
