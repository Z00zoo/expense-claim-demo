namespace Demo.Models;

public static class AppRoles
{
    public const string Applicant = "Applicant";
    public const string Approver = "Approver";
    public const string Finance = "Finance";
    public const string Admin = "Admin";

    public static readonly string[] All = [Applicant, Approver, Finance, Admin];
}
