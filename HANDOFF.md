# 交接 notes

這是一個 .NET 8 ASP.NET Core MVC 簡易請款系統 demo，目標是用清楚、低負擔的架構展示請款與簽核流程。

## 目前進度

已完成的 commits：

```text
c6e471f Initial ASP.NET Core MVC project
b05f70d Add authentication foundation
f193e4f Add expense claim CRUD
fb64d4e Add approval workflow
c2001f5 Add project handoff documentation
712fdb5 Improve dashboard and approval navigation
c42e57b Update handoff documentation
8490745 Polish demo formatting and logging
```

目前已實作：

- Cookie Authentication 登入基礎
- 啟動時建立 demo 使用者
- Applicant、Approver、Finance、Admin 角色 claims
- 請款單 CRUD
- Draft/Rejected 送出後轉為 Submitted
- 簽核流程：
  - Submitted 轉為 ManagerApproved
  - ManagerApproved 轉為 FinanceApproved
  - FinanceApproved 轉為 Paid
  - Submitted/ManagerApproved 可轉為 Rejected
- 送出、核准、退回、付款完成都會建立 approval records
- 請款狀態與簽核動作有中文顯示文字
- 依角色顯示不同 dashboard：
  - Applicant：草稿、簽核中、已退回、已付款
  - Approver：待主管簽核案件
  - Finance：待財務簽核與待付款案件
  - Admin：各狀態總數與進行中案件
- Admin 案件搜尋：
  - 狀態篩選
  - 申請人篩選
  - 日期區間篩選
  - 單號、類別、說明、申請人關鍵字搜尋
- 退回請款單時必須填寫備註
- 日期、時間與金額顯示已集中在 display extensions
- 重要流程動作已有輕量 logging

## 重要檔案

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

## 本機 setup

```powershell
dotnet restore
dotnet build
dotnet run --urls http://localhost:5100
```

接著開啟：

```text
http://localhost:5100
```

SQLite 資料庫會產生在：

```text
App_Data/claims-demo.db
```

`App_Data/*.db`、`bin/`、`obj/` 已由 git 忽略。

## Demo 帳號

所有密碼都是 `password`。

```text
applicant / password
approver / password
finance / password
admin / password
```

## 架構 notes

- MVC actions 保持相對精簡。
- Business rules 放在 `Services/`。
- EF Core entities 與簡單 view models 放在 `Models/`。
- 專案目前沒有使用 ASP.NET Core Identity；authentication 刻意保持簡單，方便 demo。
- 專案目前沒有使用 EF migrations；`DatabaseInitializer` 會替本機開發建立缺少的 demo tables。
- 不要為了 demo 擴大架構範圍；除非使用者明確要求，避免加入 CQRS、MediatR、Generic Repository、Unit of Work 或大型分層重構。

## 後續可考慮項目

建議下一階段仍以小範圍 polish 為主：

- Admin user management：
  - 列出 demo users
  - 更新角色
  - 啟用/停用帳號
- UX/data quality：
  - Admin claim search 驗證日期區間
  - 視需要限制 applicant filter 只顯示 Applicant users
- Search/list improvements：
  - Admin claim search 加上 paging 或 result limit
  - Dashboard metrics 可連到對應篩選清單
- Workflow hardening：
  - 為狀態轉換與 authorization rules 加測試
  - 讓 claim number generation 在並行情境下更安全
  - 決定 Admin 建立請款單時是否應指定 applicant
- Documentation：
  - 每次功能階段結束後，同步 README.md、HANDOFF.md、CODEX_PROMPT.md

## 給下一個 Codex 的建議 prompt

```text
這是一個 .NET 8 ASP.NET Core MVC 簡易請款系統 demo。
請先閱讀 README.md、HANDOFF.md、CODEX_PROMPT.md 和 git log。
專案已經有登入、請款單 CRUD、送出流程、主管/財務簽核、付款完成、approval records、中文狀態顯示、角色 dashboard、Admin 案件搜尋、退回必填備註、一致的日期/金額顯示，以及輕量 logging。
目前專案已足夠作為面試 demo。除非明確要求，不要擴大系統範圍。
保持既有架構：Controllers 處理 MVC flow，Services 放 business rules，Data 放 EF Core DbContext/seed/schema 初始化，Models 放 entities/enums/view models，Views 放 Razor pages。
```
