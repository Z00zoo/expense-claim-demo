using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class ClaimSearchViewModel
{
    [Display(Name = "狀態")]
    public ExpenseClaimStatus? Status { get; set; }

    [Display(Name = "申請人")]
    public int? ApplicantId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "起始日期")]
    public DateTime? FromDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "結束日期")]
    public DateTime? ToDate { get; set; }

    [StringLength(100)]
    [Display(Name = "關鍵字")]
    public string? Keyword { get; set; }

    public List<AppUser> Applicants { get; set; } = [];

    public List<ExpenseClaim> Claims { get; set; } = [];
}
