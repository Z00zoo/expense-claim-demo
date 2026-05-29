# Codex 交接 prompt

需要請另一個 Codex 環境接手時，可以使用下面這段 prompt。

```text
這是一個 .NET 8 ASP.NET Core MVC 簡易請款系統 demo。

請先閱讀這些檔案：
- README.md
- HANDOFF.md
- CODEX_PROMPT.md
- git log --oneline

專案目前已有：
- Cookie login authentication
- Demo users 與 roles
- 請款單 CRUD
- 請款單送出流程
- 主管核准
- 財務核准
- 標記付款完成
- Approval records
- 請款狀態與簽核動作的中文顯示
- 依角色顯示的 dashboard
- Admin claim search
- 退回請款單時必填備註
- 一致的日期、local time 與金額顯示
- 重要流程動作的輕量 logging

Demo 帳號密碼皆為 `password`：
- applicant
- approver
- finance
- admin

本機執行：

dotnet restore
dotnet build
dotnet run --urls http://localhost:5100

SQLite 資料庫會自動產生在：

App_Data/claims-demo.db

資料庫與 build output 已由 git 忽略。

目前專案已足夠作為面試 demo。除非使用者明確要求，請不要擴大系統範圍。

可以考慮的後續小範圍工作：
- Admin user management：
  - list demo users
  - update role
  - activate/deactivate accounts
- Admin claim search polish：
  - validate date ranges
  - consider restricting applicant filter to applicant users
  - add paging or result limits
  - make dashboard metrics link to filtered lists
- Workflow hardening：
  - add tests for status transitions and authorization rules
  - make claim number generation safer under concurrent submissions
  - decide how Admin-created claims should assign applicant ownership
- Documentation：
  - keep README.md, HANDOFF.md, and CODEX_PROMPT.md synchronized after each feature stage

保持既有架構：
- Controllers 處理 MVC actions
- Services 放 business rules
- Data 放 EF Core DbContext、seed data、schema initialization
- Models 放 entities、enums、view models
- Views 放 Razor pages

不要引入：
- ASP.NET Identity
- CQRS
- MediatR
- Generic Repository
- UnitOfWork
- Clean Architecture restructuring
- Multi-role authorization tables
- Attachment upload
- Email notifications
- Large new features

開始修改前，請先檢查目前檔案與 git status。
修改後，請執行 dotnet build，並確認主要 demo flow 沒有壞掉。
除非使用者明確要求，請不要 commit。
```
