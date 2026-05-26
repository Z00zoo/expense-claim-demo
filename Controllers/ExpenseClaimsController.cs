using System.Security.Claims;
using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Authorize]
public class ExpenseClaimsController(ExpenseClaimService expenseClaimService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var claims = await expenseClaimService.GetClaimsForUserAsync(GetCurrentUserId(), GetCurrentRole());

        return View(claims);
    }

    public async Task<IActionResult> Details(int id)
    {
        var claim = await expenseClaimService.GetClaimForUserAsync(id, GetCurrentUserId(), GetCurrentRole());
        if (claim is null)
        {
            return NotFound();
        }

        return View(claim);
    }

    [Authorize(Roles = $"{AppRoles.Applicant},{AppRoles.Admin}")]
    public IActionResult Create()
    {
        return View(new ExpenseClaimEditViewModel { ClaimDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{AppRoles.Applicant},{AppRoles.Admin}")]
    public async Task<IActionResult> Create(ExpenseClaimEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var claim = await expenseClaimService.CreateDraftAsync(model, GetCurrentUserId());

        TempData["StatusMessage"] = "請款草稿已建立。";
        return RedirectToAction(nameof(Details), new { id = claim.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var claim = await expenseClaimService.GetClaimForUserAsync(id, GetCurrentUserId(), GetCurrentRole());
        if (claim is null)
        {
            return NotFound();
        }

        if (!ExpenseClaimService.CanEdit(claim, GetCurrentUserId(), GetCurrentRole()))
        {
            return Forbid();
        }

        return View(ExpenseClaimService.ToEditViewModel(claim));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ExpenseClaimEditViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await expenseClaimService.UpdateDraftAsync(id, model, GetCurrentUserId(), GetCurrentRole());
        if (!updated)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款單已更新。";
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var claim = await expenseClaimService.GetClaimForUserAsync(id, GetCurrentUserId(), GetCurrentRole());
        if (claim is null)
        {
            return NotFound();
        }

        if (!ExpenseClaimService.CanDelete(claim, GetCurrentUserId(), GetCurrentRole()))
        {
            return Forbid();
        }

        return View(claim);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var deleted = await expenseClaimService.DeleteDraftAsync(id, GetCurrentUserId(), GetCurrentRole());
        if (!deleted)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款草稿已刪除。";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(int id)
    {
        var submitted = await expenseClaimService.SubmitAsync(id, GetCurrentUserId(), GetCurrentRole());
        if (!submitted)
        {
            return Forbid();
        }

        TempData["StatusMessage"] = "請款單已送出，等待主管簽核。";
        return RedirectToAction(nameof(Details), new { id });
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
