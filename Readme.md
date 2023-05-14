## WebAPI (使用 Northwind 資料庫)

### 介紹

本專案為 ASP.NET Core 6.0 的 WebAPI，建立的方式:

1. 建立 WebAPI 專案。
2. 以 DB First 方式建立 Model 與 Context，以及預設的 Restful API。 
3. 分別以 POST 和 GET 方式建立自訂的 SQL 查詢，以及使用 EF Core 更新資料。
4. 實作簡易的 Cookie 認證功能。

### 使用方式

使用方式如下：

1. 編譯並執行此 API，預設會顯示 Swagger 頁面，並列出可用的 API。以下為其中數個 API 的介紹。

2. 使用 POST 傳入員工 (Employee 資料表) 的 LastName、FirstName 和 HomePhone 欄位資料，以 JSON 格式傳送，範例如下：
``` json
{
  "lastName": "Davolio",
  "firstName": "Nancy",
  "homePhone": "(206) 555-9857"
}
```
，傳送的網址為：
> - https://[URL:Port]/api/Northwind/Login
> - ex.https://localhost:7216/api/Northwind/Login

3. 查詢某筆訂單的顧客與訂購資料，請自行修改網址 `[URL:Port]` 和訂單 `[ID]`:
> - https://[URL:Port]/api/Northwind/OrderCustomer/[ID]
> - ex. https://localhost:7216/api/Northwind/OrderCustomer/10249

4. 更新顧客聯絡人姓名，請自行修改網址 `[URL:Port]` 、顧客 `[ID]`和要更新的姓名`[Name]`:
> - https://[URL:Port]/api/Northwind/EditCustomerName/[ID]?ContactName=[Name]
> - ex. https://localhost:7216/api/Northwind/EditCustomerName/ALFKI?ContactName=Mario%20Anders

5. 查詢特定日期間的訂單銷售金額，可使用 POST 傳入含 startDate (起始日期)、endDate (結束日期) 的 JSON 格式資料，範例如下：
``` json
{
  "startDate": "1996-12-24",
  "endDate": "1997-07-01"
}
```
，傳送的網址為：
> - https://[URL:Port]/api/Northwind/PostSalesByYear
> - ex.https://localhost:7216/api/Northwind/PostSalesByYear

6. 使用 POST 方式可插入 Order 資料，範例 JSON 如下：
``` json
{
    "orderId": 11079,
    "customerId": "VINET",
    "employeeId": 5,
    "orderDate": "1997-07-04T00:00:00",
    "requiredDate": "1997-08-01T00:00:00",
    "shippedDate": "1997-07-16T00:00:00",
    "shipVia": 3,
    "freight": 32.3800,
    "shipName": "Vins et alcools Chevalier",
    "shipAddress": "59 rue de l'Abbaye",
    "shipCity": "Reims",
    "shipRegion": null,
    "shipPostalCode": "51100",
    "shipCountry": "France",
    "customer": null,
    "employee": null,
    "shipViaNavigation": null,
    "orderDetails": []
}
```
，傳送的網址為：
> - https://[URL:Port]/api/Northwind/
> - ex. https://localhost:7216/api/Northwind/

7. 查詢名稱中包含特定關鍵字的產品資料，請自行修改下列網址 `[URL:Port]` 和關鍵字 `[Keyword]`:
> - https://[URL:Port]/api/Northwind/Product?keyword=[Keyword]
> - ex. https://localhost:7216/api/Northwind/Product?keyword=su

8. 查詢特定分頁的訂單，請自行修改下列網址 `[URL:Port]` 和分頁 `[Page]`:
> (目前每頁呈現 10 筆資料，以 `OrderId` 欄位遞增排序)
> - https://[URL:Port]/api/Northwind/GetOrdersByPage/[Page]
> - ex. https://localhost:7216/api/Northwind/GetOrdersByPage/3

9. 傳送以下網址登出：
> - https://[URL:Port]/api/Northwind/Logout
> - ex. https://localhost:7216/api/Northwind/Logout


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

6. POST 方式使用的 SQL 查詢語法，改寫自：[MySQL Northwind Queries - Part 1](https://www.geeksengine.com/database/problem-solving/northwind-queries-part-1.php)

7. 參考以下的網頁實作 Cookie 認證，並調整為適合 .NET 6 的形式。
    - [ASP.NET Core Web API 入門教學 - 使用 cookie 驗證但不使用 ASP.NET Core Identity（實作登入登出） - 凱哥寫程式's Blog](https://blog.talllkai.com/ASPNETCore/2021/08/22/CookieAuthentication)
    - [Use cookie authentication without ASP.NET Core Identity - Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0)

8. 主要參閱以下網頁，實作 Dapper 的查詢功能：
    - [菜雞新訓記 (3): 使用 Dapper 來連線到資料庫 CRUD 吧 - 伊果的沒人看筆記本](https://igouist.github.io/post/2021/05/newbie-3-dapper/)

9. 分頁功能的實作: 
    - [ASP.net MVC + API Dapper 分頁寫法 - 老E隨手寫 - 點部落](https://dotblogs.com.tw/bda605/2022/03/12/153046)
