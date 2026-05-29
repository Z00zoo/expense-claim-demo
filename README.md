# Simple Expense Claim Demo

ASP.NET Core MVC demo project for a simple expense claim system.

## Tech Stack

- .NET 8
- ASP.NET Core MVC
- EF Core
- SQLite
- Cookie Authentication

## Current Features

- Cookie-based login and logout
- Demo users with role-based access
- Expense claim CRUD
- Submit expense claim
- Manager approval
- Finance approval
- Mark claim as paid
- Approval history on claim details
- Localized claim status labels
- Role-based dashboard for applicants, approvers, finance, and admins
- Admin claim search with status, applicant, date range, and keyword filters
- Rejection comments required during approval review

## Demo Accounts

All demo accounts use the password `password`.

| User name | Role |
| --- | --- |
| `applicant` | Applicant |
| `approver` | Approver |
| `finance` | Finance |
| `admin` | Admin |

## Run Locally

```powershell
dotnet restore
dotnet build
dotnet run --urls http://localhost:5100
```

Open:

```text
http://localhost:5100
```

## Local Database

The SQLite database is created automatically on startup:

```text
App_Data/claims-demo.db
```

The local database files are ignored by git. A fresh clone will recreate the database and seed the demo users on first run.

## Demo Flow

1. Log in as `applicant`.
2. Create an expense claim draft.
3. Submit the claim.
4. Log in as `approver` and approve the submitted claim.
5. Log in as `finance` and approve the manager-approved claim.
6. Log in as `finance` and mark the claim as paid.
7. Log in as `admin` to review dashboard counts and search claims.

## Design Decisions

This demo intentionally keeps the architecture small and interview-friendly. It avoids CQRS, MediatR, Generic Repository, Unit of Work, and broad Clean Architecture restructuring because those patterns would add ceremony without improving the core demo flow.

- Controllers handle MVC request flow, validation handoff, redirects, and view selection.
- Services contain business rules for authentication, claims, dashboards, search, and approval transitions.
- EF Core `DbContext` is used directly from services for simple, readable data access.
- SQLite is used so the project can run quickly on a local machine with minimal setup.
- ASP.NET Identity is intentionally omitted to keep authentication simple within the demo scope.
- The current single-role user model is a demo simplification. A production system could model `Users`, `Roles`, and `UserRoles` separately.

## Screenshots

Screenshots can be added here when preparing the final interview deck:

- Login
- Dashboard
- Claim list/detail
- Approval review
- Admin search

## Project Structure

```text
Controllers/   MVC controllers and route actions
Data/          EF Core DbContext, seed data, schema initialization
Models/        Entities, enums, and view models
Services/      Business logic for auth, claims, and approvals
Views/         Razor views
wwwroot/       Static assets
```

## Notes

This demo currently uses `EnsureCreated()` plus a lightweight schema initializer instead of EF Core migrations. That keeps local setup simple for demo purposes.
