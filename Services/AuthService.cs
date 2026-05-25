using Demo.Data;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services;

public class AuthService(ApplicationDbContext dbContext, PasswordService passwordService)
{
    public async Task<AppUser?> ValidateUserAsync(string userName, string password)
    {
        var normalizedUserName = userName.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .SingleOrDefaultAsync(item => item.UserName.ToLower() == normalizedUserName && item.IsActive);

        if (user is null || !passwordService.VerifyPassword(password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}
