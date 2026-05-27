using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Authorize(Roles = AppRoles.Admin)]
public class AdminController(ExpenseClaimService expenseClaimService) : Controller
{
    public async Task<IActionResult> Claims(ClaimSearchViewModel model)
    {
        model.Applicants = await expenseClaimService.GetApplicantsAsync();
        model.Claims = await expenseClaimService.SearchClaimsAsync(model);

        return View(model);
    }
}
