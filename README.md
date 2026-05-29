# 簡易請款系統 Demo

這是一個使用 ASP.NET Core MVC 製作的簡易請款系統 demo，目標是呈現清楚、可快速啟動、適合面試展示的 MVC 專案。

## 技術堆疊

- .NET 8
- ASP.NET Core MVC
- EF Core
- SQLite
- Cookie Authentication

## 目前功能

- Cookie-based 登入與登出
- Demo 使用者與角色權限
- 請款單新增、檢視、編輯、刪除
- 請款單送出申請
- 主管核准
- 財務核准
- 標記付款完成
- 請款單明細顯示簽核紀錄
- 請款狀態與簽核動作的中文顯示
- 依 Applicant、Approver、Finance、Admin 顯示不同首頁資訊
- Admin 可依狀態、申請人、日期區間與關鍵字搜尋請款單
- 退回請款單時必須填寫備註
- 日期、時間與金額顯示集中處理，保持畫面格式一致
- 重要流程動作有輕量 logging

## Demo 帳號

所有 demo 帳號的密碼都是 `password`。

| 帳號 | 角色 |
| --- | --- |
| `applicant` | Applicant |
| `approver` | Approver |
| `finance` | Finance |
| `admin` | Admin |

## 本機執行

```powershell
dotnet restore
dotnet build
dotnet run --urls http://localhost:5100
```

開啟瀏覽器：

```text
http://localhost:5100
```

使用 `--urls http://localhost:5100` 可以明確指定 demo 啟動在固定 port，避免受到本機 launch profile 或環境變數影響。

## 本機資料庫

SQLite 資料庫會在應用程式啟動時自動建立：

```text
App_Data/claims-demo.db
```

本機資料庫檔案已由 git 忽略。全新 clone 後第一次啟動時，系統會重新建立資料庫並寫入 demo 使用者。

## Demo 流程

1. 使用 `applicant` 登入。
2. 建立請款草稿。
3. 送出請款單。
4. 使用 `approver` 登入，核准已送出的請款單。
5. 使用 `finance` 登入，核准主管已核准的請款單。
6. 使用 `finance` 標記請款單付款完成。
7. 使用 `admin` 登入，查看首頁統計並搜尋請款單。

## 設計決策

這個 demo 刻意維持小而清楚的架構，方便在面試中說明與操作。專案沒有引入 CQRS、MediatR、Generic Repository、Unit of Work 或大規模 Clean Architecture 分層，因為這些做法會增加展示成本，卻不會讓目前的核心流程更清楚。

- Controllers 負責 MVC request flow、驗證結果處理、重新導向與 View 選擇。
- Services 負責 authentication、請款、首頁資訊、搜尋與簽核狀態轉換等 business rules。
- Services 直接使用 EF Core `DbContext`，讓資料存取保持簡單、可讀。
- SQLite 用於快速本機啟動，不需要額外安裝資料庫服務。
- ASP.NET Identity 被刻意省略，讓 authentication 維持在 demo 範圍內。
- 目前使用單一角色欄位是 demo 簡化；正式系統可改為 `Users`、`Roles`、`UserRoles` 的多角色模型。

## 截圖

準備最終面試簡報時，可在這裡加入實際截圖：

- Login
- Dashboard
- Claim list/detail
- Approval review
- Admin search

## 專案結構

```text
Controllers/   MVC controllers 與 route actions
Data/          EF Core DbContext、seed data、schema 初始化
Models/        Entities、enums、view models
Services/      Authentication、請款與簽核相關 business logic
Views/         Razor views
wwwroot/       靜態資源
```

## 補充說明

這個 demo 目前使用 `EnsureCreated()` 搭配輕量的 schema initializer，而不是 EF Core migrations。這樣可以降低本機 demo setup 成本，讓展示重點放在 MVC 流程與請款簽核邏輯。
