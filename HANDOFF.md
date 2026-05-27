# Handoff Notes

This project is a .NET 8 ASP.NET Core MVC demo for a simple expense claim system.

## Current Progress

Completed commits:

```text
c6e471f Initial ASP.NET Core MVC project
b05f70d Add authentication foundation
f193e4f Add expense claim CRUD
fb64d4e Add approval workflow
c2001f5 Add project handoff documentation
712fdb5 Improve dashboard and approval navigation
```

Implemented so far:

- Authentication foundation with Cookie Authentication
- Demo users seeded on startup
- Role claims for Applicant, Approver, Finance, and Admin
- Expense claim CRUD
- Submit workflow from Draft/Rejected to Submitted
- Approval workflow:
  - Submitted to ManagerApproved
  - ManagerApproved to FinanceApproved
  - FinanceApproved to Paid
  - Submitted/ManagerApproved to Rejected
- Approval records for submit, approve, reject, and paid actions
- Localized status and approval action labels
- Role-based dashboard:
  - Applicant: drafts, in-approval claims, rejected claims, and paid claims
  - Approver: claims waiting for manager approval
  - Finance: claims waiting for finance approval or payment
  - Admin: global counts by status and active workflow claims
- Admin claim search:
  - status filter
  - applicant filter
  - date range filters
  - claim number/category/description/applicant keyword search
- Rejection comment requirement in the approval flow

## Important Files

```text
Program.cs
Data/ApplicationDbContext.cs
Data/DatabaseInitializer.cs
Data/SeedData.cs
Models/AppUser.cs
Models/ExpenseClaim.cs
Models/ApprovalRecord.cs
Models/DisplayExtensions.cs
Models/DashboardViewModel.cs
Models/ClaimSearchViewModel.cs
Services/AuthService.cs
Services/ExpenseClaimService.cs
Services/ApprovalService.cs
Controllers/AccountController.cs
Controllers/ExpenseClaimsController.cs
Controllers/ApprovalsController.cs
Controllers/AdminController.cs
Views/Home/Index.cshtml
Views/ExpenseClaims/
Views/Approvals/
Views/Admin/Claims.cshtml
```

## Local Setup

```powershell
dotnet restore
dotnet build
dotnet run --urls http://localhost:5100
```

Then open:

```text
http://localhost:5100
```

The SQLite database is generated at:

```text
App_Data/claims-demo.db
```

`App_Data/*.db`, `bin/`, and `obj/` are ignored by git.

## Demo Accounts

All passwords are `password`.

```text
applicant / password
approver / password
finance / password
admin / password
```

## Architecture Notes

- Keep MVC actions relatively thin.
- Keep business rules in `Services/`.
- Keep EF Core entities and simple view models in `Models/`.
- The project currently does not use ASP.NET Core Identity; auth is intentionally simple for demo speed.
- The project currently does not use EF migrations; `DatabaseInitializer` creates missing demo tables for local development.

## Next Stage Plan

Recommended next implementation stage:

- Admin user management:
  - list demo users
  - update role
  - activate/deactivate accounts
- UX/data quality:
  - make currency and local time formatting consistent
  - add culture-aware currency formatting
  - keep dashboard, list, details, and approval timestamps consistent
  - validate date ranges in claim search
- Search/list improvements:
  - restrict applicant filter to applicant users if that matches the intended product behavior
  - add paging or result limits for Admin claim search
  - make dashboard metric cards link to filtered claim lists
- Workflow hardening:
  - add tests for status transitions and authorization rules
  - make claim number generation safer under concurrent submissions
  - decide whether Admin-created claims should use Admin as applicant or select an applicant
- Documentation:
  - keep README.md, HANDOFF.md, and CODEX_PROMPT.md synchronized after each feature stage

## Suggested Prompt For Next Codex

```text
This is a .NET 8 ASP.NET Core MVC simple expense claim demo.
Read README.md, HANDOFF.md, and git log first.
The project already has login, expense claim CRUD, approval workflow, approval records, localized status labels, a role-based dashboard, Admin claim search, and required rejection comments.
Next stage: implement Admin user management, improve currency/local-time formatting consistency, add paging/filter polish to Admin claim search, and add tests for workflow/authorization rules.
Keep the existing architecture: Controllers for MVC actions, Services for business rules, Data for EF Core, Models for entities/view models, Views for Razor.
```
