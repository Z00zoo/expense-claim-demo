using Demo.Models;
using Demo.Services;

namespace Demo.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext dbContext, PasswordService passwordService)
    {
        if (dbContext.Users.Any())
        {
            return;
        }

        dbContext.Users.AddRange(
            CreateUser("applicant", "申請人", AppRoles.Applicant, passwordService),
            CreateUser("approver", "主管簽核者", AppRoles.Approver, passwordService),
            CreateUser("finance", "財務人員", AppRoles.Finance, passwordService),
            CreateUser("admin", "系統管理員", AppRoles.Admin, passwordService));

        dbContext.SaveChanges();
    }

    private static AppUser CreateUser(
        string userName,
        string displayName,
        string role,
        PasswordService passwordService)
    {
        return new AppUser
        {
            UserName = userName,
            DisplayName = displayName,
            Role = role,
            PasswordHash = passwordService.HashPassword("password")
        };
    }
}
