using Demo.Data;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services;

public class ApprovalService(ApplicationDbContext dbContext)
{
    public async Task<List<ExpenseClaim>> GetPendingClaimsAsync(string role)
    {
        var query = dbContext.ExpenseClaims
            .Include(claim => claim.Applicant)
            .OrderBy(claim => claim.SubmittedAt ?? claim.UpdatedAt)
            .AsQueryable();

        query = role switch
        {
            AppRoles.Approver => query.Where(claim => claim.Status == ExpenseClaimStatus.Submitted),
            AppRoles.Finance => query.Where(claim =>
                claim.Status == ExpenseClaimStatus.ManagerApproved
                || claim.Status == ExpenseClaimStatus.FinanceApproved),
            AppRoles.Admin => query.Where(claim =>
                claim.Status == ExpenseClaimStatus.Submitted
                || claim.Status == ExpenseClaimStatus.ManagerApproved
                || claim.Status == ExpenseClaimStatus.FinanceApproved),
            _ => query.Where(_ => false)
        };

        return await query.ToListAsync();
    }

    public async Task<ExpenseClaim?> GetClaimForApprovalAsync(int id, string role)
    {
        var claim = await dbContext.ExpenseClaims
            .Include(item => item.Applicant)
            .Include(item => item.ApprovalRecords.OrderBy(record => record.CreatedAt))
                .ThenInclude(record => record.Actor)
            .SingleOrDefaultAsync(item => item.Id == id);

        if (claim is null || !CanActOn(claim, role))
        {
            return null;
        }

        return claim;
    }

    public async Task<bool> ApproveAsync(int id, int actorId, string role, string? comment)
    {
        var claim = await dbContext.ExpenseClaims.SingleOrDefaultAsync(item => item.Id == id);
        if (claim is null)
        {
            return false;
        }

        if (role == AppRoles.Approver && claim.Status == ExpenseClaimStatus.Submitted)
        {
            AddRecord(claim, actorId, ApprovalAction.ManagerApproved, comment);
            claim.Status = ExpenseClaimStatus.ManagerApproved;
            claim.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            return true;
        }

        if (role == AppRoles.Finance && claim.Status == ExpenseClaimStatus.ManagerApproved)
        {
            AddRecord(claim, actorId, ApprovalAction.FinanceApproved, comment);
            claim.Status = ExpenseClaimStatus.FinanceApproved;
            claim.UpdatedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            return true;
        }

        if (role == AppRoles.Admin)
        {
            if (claim.Status == ExpenseClaimStatus.Submitted)
            {
                AddRecord(claim, actorId, ApprovalAction.ManagerApproved, comment);
                claim.Status = ExpenseClaimStatus.ManagerApproved;
                claim.UpdatedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
                return true;
            }

            if (claim.Status == ExpenseClaimStatus.ManagerApproved)
            {
                AddRecord(claim, actorId, ApprovalAction.FinanceApproved, comment);
                claim.Status = ExpenseClaimStatus.FinanceApproved;
                claim.UpdatedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        return false;
    }

    public async Task<bool> RejectAsync(int id, int actorId, string role, string? comment)
    {
        var claim = await dbContext.ExpenseClaims.SingleOrDefaultAsync(item => item.Id == id);
        if (claim is null || !CanReject(claim, role))
        {
            return false;
        }

        AddRecord(claim, actorId, ApprovalAction.Rejected, comment);
        claim.Status = ExpenseClaimStatus.Rejected;
        claim.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAsPaidAsync(int id, int actorId, string role, string? comment)
    {
        var claim = await dbContext.ExpenseClaims.SingleOrDefaultAsync(item => item.Id == id);
        if (claim is null || !CanMarkAsPaid(claim, role))
        {
            return false;
        }

        AddRecord(claim, actorId, ApprovalAction.Paid, comment);
        claim.Status = ExpenseClaimStatus.Paid;
        claim.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return true;
    }

    public static bool CanApprove(ExpenseClaim claim, string role)
    {
        return (role == AppRoles.Approver && claim.Status == ExpenseClaimStatus.Submitted)
            || (role == AppRoles.Finance && claim.Status == ExpenseClaimStatus.ManagerApproved)
            || (role == AppRoles.Admin && claim.Status is ExpenseClaimStatus.Submitted or ExpenseClaimStatus.ManagerApproved);
    }

    public static bool CanReject(ExpenseClaim claim, string role)
    {
        return (role == AppRoles.Approver && claim.Status == ExpenseClaimStatus.Submitted)
            || (role == AppRoles.Finance && claim.Status == ExpenseClaimStatus.ManagerApproved)
            || (role == AppRoles.Admin && claim.Status is ExpenseClaimStatus.Submitted or ExpenseClaimStatus.ManagerApproved);
    }

    public static bool CanMarkAsPaid(ExpenseClaim claim, string role)
    {
        return (role == AppRoles.Finance || role == AppRoles.Admin)
            && claim.Status == ExpenseClaimStatus.FinanceApproved;
    }

    private static bool CanActOn(ExpenseClaim claim, string role)
    {
        return CanApprove(claim, role) || CanReject(claim, role) || CanMarkAsPaid(claim, role);
    }

    private void AddRecord(ExpenseClaim claim, int actorId, ApprovalAction action, string? comment)
    {
        dbContext.ApprovalRecords.Add(new ApprovalRecord
        {
            ExpenseClaimId = claim.Id,
            ActorId = actorId,
            Action = action,
            Comment = comment?.Trim() ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        });
    }
}
