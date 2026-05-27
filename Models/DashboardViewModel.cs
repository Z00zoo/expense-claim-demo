namespace Demo.Models;

public class DashboardViewModel
{
    public string Role { get; set; } = string.Empty;

    public List<DashboardMetricViewModel> Metrics { get; set; } = [];

    public List<ExpenseClaim> ActionClaims { get; set; } = [];

    public List<ExpenseClaim> RecentClaims { get; set; } = [];
}

public class DashboardMetricViewModel
{
    public string Label { get; set; } = string.Empty;

    public int Count { get; set; }

    public ExpenseClaimStatus? Status { get; set; }
}
