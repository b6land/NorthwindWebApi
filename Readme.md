## WebAPI (使用 Northwind 資料庫)

### 使用方式

使用方式如下：
1. 編譯並執行此 API，預設會顯示 Swagger 頁面，會列出可用的 API。
2. 查詢某筆訂單的顧客與訂購資料，請自行修改網址 `[URL:Port]` 和訂單 `[ID]`:
> - https://[URL:Port]/api/Northwind/OrderCustomer/[ID]
> - ex. https://localhost:7216/api/Northwind/OrderCustomer/10249
3. 更新顧客聯絡人姓名，請自行修改網址 `[URL:Port]` 、顧客 `[ID]`和要更新的姓名`[Name]`:
> - https://[URL:Port]/api/Northwind/EditCustomerName/[ID]?ContactName=`[Name]`
> - ex. https://localhost:7216/api/Northwind/EditCustomerName/ALFKI?ContactName=Mario%20Anders

### 介紹

本專案為 ASP.NET Core 6.0 的 WebAPI，建立的方式:

1. 建立 WebAPI 專案。
2. 以 DB First 方式建立 Model 與 Context，以及預設的 Restful API。 
3. 建立自訂的 SQL 查詢，以及使用 EF Core 更新資料。

### NOTE

1. 使用的資料庫為 Northwind 資料庫 (介紹網誌): [SQL Server Sample Databases (經典 / 示範資料庫) - The Skeptical Software Engineer](https://sdwh.dev/posts/2021/12/SQL-Server-Sample-Databases/)
2. 解決因為下載到不一致的 Entity Framework Core 版本，導致建立 Controller 失敗: [c# - TypeLoadException: Method 'Create' in type ... does not have an implementation (Error Below) - Stack Overflow](https://stackoverflow.com/questions/65778821/)
3. 嘗試詢問 ChatGPT 3.5，以理解大致所需的步驟，輸入的提示包含:
```
- 我要如何使用 Northwind 資料庫與 .Net Core 3.1 建立小型的 Restful API 專案 ?
- 繼續
- 我改用了 ASP.NET Core 6 建立 WebAPI 專案，並且建立了範例的 Northwind 資料庫，接下來如何建立 Restful API ?
- 已經建立空的 DbContext 類別，如何連接至 SQL Server 上的 Northwind 資料庫
- 什麼是 scaffolded 控制器 ?
```
4. 使用命令列快速建立 Model、Context 和 Restful API: [我與 ASP.NET Core 的 30天 :: 第 12 屆 iThome 鐵人賽](https://ithelp.ithome.com.tw/users/20129389/ironman/3185)
5. 建立專案: [教學課程：使用 ASP.NET Core 建立 Web API - Microsoft Learn](https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)
