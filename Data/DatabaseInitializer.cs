using Microsoft.EntityFrameworkCore;

namespace Demo.Data;

public static class DatabaseInitializer
{
    public static void EnsureSchema(ApplicationDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS "ExpenseClaims" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_ExpenseClaims" PRIMARY KEY AUTOINCREMENT,
                "ClaimNo" TEXT NOT NULL,
                "ApplicantId" INTEGER NOT NULL,
                "ClaimDate" TEXT NOT NULL,
                "Amount" TEXT NOT NULL,
                "Category" TEXT NOT NULL,
                "Description" TEXT NOT NULL,
                "Status" TEXT NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                "UpdatedAt" TEXT NOT NULL,
                "SubmittedAt" TEXT NULL,
                CONSTRAINT "FK_ExpenseClaims_Users_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
            );
            """);
        dbContext.Database.ExecuteSqlRaw("""
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_ExpenseClaims_ClaimNo"
            ON "ExpenseClaims" ("ClaimNo");
            """);
        dbContext.Database.ExecuteSqlRaw("""
            CREATE INDEX IF NOT EXISTS "IX_ExpenseClaims_ApplicantId"
            ON "ExpenseClaims" ("ApplicantId");
            """);
    }
}
