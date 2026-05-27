# Codex Handoff Prompt

Use this prompt when asking another Codex environment to continue development.

```text
This is a .NET 8 ASP.NET Core MVC simple expense claim demo.

Read these files first:
- README.md
- HANDOFF.md
- git log --oneline

The project already has:
- Cookie login authentication
- Demo users and roles
- Expense claim CRUD
- Submit workflow
- Manager approval
- Finance approval
- Mark as paid
- Approval records
- Localized status and approval action labels
- Role-based dashboard
- Admin claim search
- Required rejection comments

Demo accounts all use password `password`:
- applicant
- approver
- finance
- admin

Run locally with:

dotnet restore
dotnet build
dotnet run --urls http://localhost:5100

The SQLite database is generated automatically at:

App_Data/claims-demo.db

The database and build outputs are ignored by git.

Next stage:
- Implement Admin user management:
  - list demo users
  - update role
  - activate/deactivate accounts
- Improve formatting consistency:
  - culture-aware currency formatting
  - local time handling across list, detail, dashboard, and approval views
- Polish Admin claim search:
  - validate date ranges
  - consider restricting applicant filter to applicant users
  - add paging or result limits
  - make dashboard metrics link to filtered lists
- Harden workflow behavior:
  - add tests for status transitions and authorization rules
  - make claim number generation safer under concurrent submissions
  - decide how Admin-created claims should assign applicant ownership

Keep the existing architecture:
- Controllers for MVC actions
- Services for business rules
- Data for EF Core DbContext, seed data, and schema initialization
- Models for entities, enums, and view models
- Views for Razor pages

Before editing, inspect the current files and git status.
After editing, run dotnet build and verify the key workflow still works.
Do not commit unless explicitly asked.
```
