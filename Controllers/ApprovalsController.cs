using System.Security.Claims;
using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Authorize(Roles = $"{AppRoles.Approver},{AppRoles.Finance},{AppRoles.Admin}")]
public class ApprovalsController(ApprovalService approvalService) : Controller
{
    public async Task<IActionResult> Pending()
    {
        var claims = await approvalService.GetPendingClaimsAsync(GetCurrentRole());

        return View(claims);
    }

    public async Task<IActionResult> Review(int id)
    {
        var claim = await approvalService.GetClaimForApprovalAsync(id, GetCurrentRole());
        if (claim is null)
        {
            return NotFound();
        }

        return View(claim);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(ApprovalDecisionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Review), new { id = model.ExpenseClaimId });
        }

        var approved = await approvalService.ApproveAsync(
            model.ExpenseClaimId,
            GetCurrentUserId(),
            GetCurrentRole(),
            model.Comment);

        if (!approved)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款單已核准。";
        return RedirectToAction(nameof(Pending));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(ApprovalDecisionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Review), new { id = model.ExpenseClaimId });
        }

        var rejected = await approvalService.RejectAsync(
            model.ExpenseClaimId,
            GetCurrentUserId(),
            GetCurrentRole(),
            model.Comment);

        if (!rejected)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款單已退回。";
        return RedirectToAction(nameof(Pending));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsPaid(ApprovalDecisionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Review), new { id = model.ExpenseClaimId });
        }

        var paid = await approvalService.MarkAsPaidAsync(
            model.ExpenseClaimId,
            GetCurrentUserId(),
            GetCurrentRole(),
            model.Comment);

        if (!paid)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款單已標記付款完成。";
        return RedirectToAction(nameof(Pending));
    }

    private int GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(value, out var userId) ? userId : 0;
    }

    private string GetCurrentRole()
    {
        return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
    }
}
