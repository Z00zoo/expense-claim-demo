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
