using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class ApprovalRecord
{
    public int Id { get; set; }

    public int ExpenseClaimId { get; set; }

    public ExpenseClaim? ExpenseClaim { get; set; }

    [Display(Name = "動作人")]
    public int ActorId { get; set; }

    public AppUser? Actor { get; set; }

    [Display(Name = "動作")]
    public ApprovalAction Action { get; set; }

    [StringLength(500)]
    [Display(Name = "備註")]
    public string Comment { get; set; } = string.Empty;

    [Display(Name = "建立時間")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
