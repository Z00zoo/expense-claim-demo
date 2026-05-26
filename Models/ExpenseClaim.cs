using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

public class ExpenseClaim
{
    public int Id { get; set; }

    [Display(Name = "請款單號")]
    [StringLength(20)]
    public string ClaimNo { get; set; } = string.Empty;

    [Display(Name = "申請人")]
    public int ApplicantId { get; set; }

    public AppUser? Applicant { get; set; }

    [Required(ErrorMessage = "請輸入請款日期")]
    [DataType(DataType.Date)]
    [Display(Name = "請款日期")]
    public DateTime ClaimDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "請輸入請款金額")]
    [Range(1, 99999999, ErrorMessage = "請款金額必須大於 0")]
    [DataType(DataType.Currency)]
    [Display(Name = "金額")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "請輸入請款類別")]
    [StringLength(50)]
    [Display(Name = "類別")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入說明")]
    [StringLength(500)]
    [Display(Name = "說明")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "狀態")]
    public ExpenseClaimStatus Status { get; set; } = ExpenseClaimStatus.Draft;

    [Display(Name = "建立時間")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "更新時間")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Display(Name = "送出時間")]
    public DateTime? SubmittedAt { get; set; }
}
