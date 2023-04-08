本專案版本為 ASP.NET Core 6.0，使用的步驟與參考資料:

1. 建立專案: https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio
2. 以 DB First 方式建立 Model 與 Context，並建立 Restful API: https://ithelp.ithome.com.tw/users/20129389/ironman/3185

NOTE:
1. 使用的資料庫為 Northwind 資料庫 (介紹網誌): https://sdwh.dev/posts/2021/12/SQL-Server-Sample-Databases/
2. 解決因為下載到不一致的 Entity Framework Core 版本，導致建立 Controller 失敗: https://stackoverflow.com/questions/65778821/typeloadexception-method-create-in-type-does-not-have-an-implementation
3. 嘗試詢問 ChatGPT 3.5，以理解大致所需的步驟，輸入的提示包含:
```
- 我要如何使用 Northwind 資料庫與 .Net Core 3.1 建立小型的 Restful API 專案 ?
- 繼續
- 我改用了 ASP.NET Core 6 建立 WebAPI 專案，並且建立了範例的 Northwind 資料庫，接下來如何建立 Restful API ?
- 已經建立空的 DbContext 類別，如何連接至 SQL Server 上的 Northwind 資料庫
- 什麼是 scaffolded 控制器 ?
```