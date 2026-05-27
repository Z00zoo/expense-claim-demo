using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using Demo.Services;

namespace Demo.Controllers;

public class HomeController(ExpenseClaimService expenseClaimService) : Controller
{
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return View();
        }

        var dashboard = await expenseClaimService.GetDashboardAsync(GetCurrentUserId(), GetCurrentRole());

        return View(dashboard);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
