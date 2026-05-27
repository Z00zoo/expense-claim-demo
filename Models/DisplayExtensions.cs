namespace Demo.Models;

public static class DisplayExtensions
{
    public static string ToDisplayName(this ExpenseClaimStatus status)
    {
        return status switch
        {
            ExpenseClaimStatus.Draft => "草稿",
            ExpenseClaimStatus.Submitted => "待主管簽核",
            ExpenseClaimStatus.ManagerApproved => "待財務簽核",
            ExpenseClaimStatus.FinanceApproved => "待付款",
            ExpenseClaimStatus.Rejected => "已退回",
            ExpenseClaimStatus.Paid => "已付款",
            _ => status.ToString()
        };
    }

    public static string ToBadgeClass(this ExpenseClaimStatus status)
    {
        return status switch
        {
            ExpenseClaimStatus.Draft => "text-bg-secondary",
            ExpenseClaimStatus.Submitted => "text-bg-warning",
            ExpenseClaimStatus.ManagerApproved => "text-bg-info",
            ExpenseClaimStatus.FinanceApproved => "text-bg-primary",
            ExpenseClaimStatus.Rejected => "text-bg-danger",
            ExpenseClaimStatus.Paid => "text-bg-success",
            _ => "text-bg-secondary"
        };
    }

    public static string ToDisplayName(this ApprovalAction action)
    {
        return action switch
        {
            ApprovalAction.Submitted => "送出申請",
            ApprovalAction.ManagerApproved => "主管核准",
            ApprovalAction.FinanceApproved => "財務核准",
            ApprovalAction.Rejected => "退回",
            ApprovalAction.Paid => "標記已付款",
            _ => action.ToString()
        };
    }
}
