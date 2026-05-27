using Demo.Data;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services;

public class ExpenseClaimService(ApplicationDbContext dbContext)
{
    public async Task<List<ExpenseClaim>> GetClaimsForUserAsync(int userId, string role)
    {
        var query = dbContext.ExpenseClaims
            .Include(claim => claim.Applicant)
            .OrderByDescending(claim => claim.UpdatedAt)
            .AsQueryable();

        if (role != AppRoles.Admin)
        {
            query = query.Where(claim => claim.ApplicantId == userId);
        }

        return await query.ToListAsync();
    }

    public async Task<ExpenseClaim?> GetClaimForUserAsync(int id, int userId, string role)
    {
        var claim = await dbContext.ExpenseClaims
            .Include(item => item.Applicant)
            .Include(item => item.ApprovalRecords.OrderBy(record => record.CreatedAt))
                .ThenInclude(record => record.Actor)
            .SingleOrDefaultAsync(item => item.Id == id);

        if (claim is null || !CanView(claim, userId, role))
        {
            return null;
        }

        return claim;
    }

    public async Task<ExpenseClaim> CreateDraftAsync(ExpenseClaimEditViewModel model, int applicantId)
    {
        var claim = new ExpenseClaim
        {
            ClaimNo = await GenerateClaimNoAsync(),
            ApplicantId = applicantId,
            ClaimDate = model.ClaimDate,
            Amount = model.Amount,
            Category = model.Category.Trim(),
            Description = model.Description.Trim(),
            Status = ExpenseClaimStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.ExpenseClaims.Add(claim);
        await dbContext.SaveChangesAsync();

        return claim;
    }

    public async Task<bool> UpdateDraftAsync(int id, ExpenseClaimEditViewModel model, int userId, string role)
    {
        var claim = await GetClaimForUserAsync(id, userId, role);
        if (claim is null || !CanEdit(claim, userId, role))
        {
            return false;
        }

        claim.ClaimDate = model.ClaimDate;
        claim.Amount = model.Amount;
        claim.Category = model.Category.Trim();
        claim.Description = model.Description.Trim();
        claim.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDraftAsync(int id, int userId, string role)
    {
        var claim = await GetClaimForUserAsync(id, userId, role);
        if (claim is null || !CanDelete(claim, userId, role))
        {
            return false;
        }

        dbContext.ExpenseClaims.Remove(claim);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SubmitAsync(int id, int userId, string role)
    {
        var claim = await GetClaimForUserAsync(id, userId, role);
        if (claim is null || !CanSubmit(claim, userId, role))
        {
            return false;
        }

        claim.Status = ExpenseClaimStatus.Submitted;
        claim.SubmittedAt = DateTime.UtcNow;
        claim.UpdatedAt = DateTime.UtcNow;
        dbContext.ApprovalRecords.Add(new ApprovalRecord
        {
            ExpenseClaimId = claim.Id,
            ActorId = userId,
            Action = ApprovalAction.Submitted,
            Comment = "送出請款申請",
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
        return true;
    }

    public static bool CanView(ExpenseClaim claim, int userId, string role)
    {
        return role == AppRoles.Admin || claim.ApplicantId == userId;
    }

    public static bool CanEdit(ExpenseClaim claim, int userId, string role)
    {
        return CanView(claim, userId, role)
            && claim.Status is ExpenseClaimStatus.Draft or ExpenseClaimStatus.Rejected;
    }

    public static bool CanDelete(ExpenseClaim claim, int userId, string role)
    {
        return CanView(claim, userId, role) && claim.Status == ExpenseClaimStatus.Draft;
    }

    public static bool CanSubmit(ExpenseClaim claim, int userId, string role)
    {
        return CanEdit(claim, userId, role);
    }

    public static ExpenseClaimEditViewModel ToEditViewModel(ExpenseClaim claim)
    {
        return new ExpenseClaimEditViewModel
        {
            Id = claim.Id,
            ClaimDate = claim.ClaimDate,
            Amount = claim.Amount,
            Category = claim.Category,
            Description = claim.Description
        };
    }

    private async Task<string> GenerateClaimNoAsync()
    {
        var today = DateTime.Today;
        var prefix = $"CL{today:yyyyMMdd}";
        var count = await dbContext.ExpenseClaims.CountAsync(claim => claim.ClaimNo.StartsWith(prefix));

        return $"{prefix}{count + 1:000}";
    }
}
