using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class ApprovalDecisionViewModel
{
    public int ExpenseClaimId { get; set; }

    [StringLength(500)]
    [Display(Name = "備註")]
    public string? Comment { get; set; }
}
