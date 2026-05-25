using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Role { get; set; } = AppRoles.Applicant;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
