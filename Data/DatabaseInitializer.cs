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
        dbContext.Database.ExecuteSqlRaw("""
            CREATE TABLE IF NOT EXISTS "ApprovalRecords" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_ApprovalRecords" PRIMARY KEY AUTOINCREMENT,
                "ExpenseClaimId" INTEGER NOT NULL,
                "ActorId" INTEGER NOT NULL,
                "Action" TEXT NOT NULL,
                "Comment" TEXT NOT NULL,
                "CreatedAt" TEXT NOT NULL,
                CONSTRAINT "FK_ApprovalRecords_ExpenseClaims_ExpenseClaimId" FOREIGN KEY ("ExpenseClaimId") REFERENCES "ExpenseClaims" ("Id") ON DELETE CASCADE,
                CONSTRAINT "FK_ApprovalRecords_Users_ActorId" FOREIGN KEY ("ActorId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
            );
            """);
        dbContext.Database.ExecuteSqlRaw("""
            CREATE INDEX IF NOT EXISTS "IX_ApprovalRecords_ExpenseClaimId"
            ON "ApprovalRecords" ("ExpenseClaimId");
            """);
        dbContext.Database.ExecuteSqlRaw("""
            CREATE INDEX IF NOT EXISTS "IX_ApprovalRecords_ActorId"
            ON "ApprovalRecords" ("ActorId");
            """);
    }
}
