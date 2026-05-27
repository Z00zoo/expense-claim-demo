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
- Implement status localization
- Implement a role-based dashboard
- Implement an Admin claim search page
- Optionally implement Admin user management
- Improve UX/data quality, especially requiring comments on rejection

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
