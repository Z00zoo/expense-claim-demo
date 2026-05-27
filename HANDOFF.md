# Handoff Notes

This project is a .NET 8 ASP.NET Core MVC demo for a simple expense claim system.

## Current Progress

Completed commits:

```text
c6e471f Initial ASP.NET Core MVC project
b05f70d Add authentication foundation
f193e4f Add expense claim CRUD
fb64d4e Add approval workflow
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

## Important Files

```text
Program.cs
Data/ApplicationDbContext.cs
Data/DatabaseInitializer.cs
Data/SeedData.cs
Models/AppUser.cs
Models/ExpenseClaim.cs
Models/ApprovalRecord.cs
Services/AuthService.cs
Services/ExpenseClaimService.cs
Services/ApprovalService.cs
Controllers/AccountController.cs
Controllers/ExpenseClaimsController.cs
Controllers/ApprovalsController.cs
Views/ExpenseClaims/
Views/Approvals/
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

- Status display localization:
  - `Draft` -> `草稿`
  - `Submitted` -> `待主管簽核`
  - `ManagerApproved` -> `待財務簽核`
  - `FinanceApproved` -> `待付款`
  - `Rejected` -> `已退回`
  - `Paid` -> `已付款`
- Role-based dashboard:
  - Applicant: my drafts, in approval, rejected, paid
  - Approver: submitted claims waiting for manager approval
  - Finance: manager-approved claims and claims waiting for payment
  - Admin: global counts by status
- Admin claim search page:
  - status filter
  - applicant filter
  - date range
  - claim number or keyword search
- Admin user management:
  - list demo users
  - update role
  - activate/deactivate accounts
- UX/data quality:
  - require comment when rejecting
  - use localized status labels throughout views
  - make currency and local time formatting consistent

## Suggested Prompt For Next Codex

```text
This is a .NET 8 ASP.NET Core MVC simple expense claim demo.
Read README.md, HANDOFF.md, and git log first.
The project already has login, expense claim CRUD, approval workflow, and approval records.
Next stage: implement status localization, a role-based dashboard, and an Admin claim search page.
Keep the existing architecture: Controllers for MVC actions, Services for business rules, Data for EF Core, Models for entities/view models, Views for Razor.
```
