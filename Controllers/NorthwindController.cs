using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindWebApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NorthwindController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public NorthwindController(NorthwindContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <remarks> 暫時使用員工名稱與電話代替帳號、密碼 </remarks>
        /// <param name="value"> 員工資料 </param>
        /// <returns> 登入成功或失敗訊息 </returns>
        [HttpPost("Login")]
        public string Login(Employee value)
        {
            var user = (from a in _context.Employees
                        where a.LastName == value.LastName
                        && a.HomePhone == value.HomePhone
                        select a).SingleOrDefault();

            if (user == null)
            {
                return "帳號密碼錯誤";
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.EmployeeId.ToString()),
                    new Claim("LastName", user.LastName),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return "登入成功";
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpDelete("Logout")]
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 未登入時操作其它 API 時顯示的訊息
        /// </summary>
        /// <returns> 未登入訊息 </returns>
        [HttpGet("NotLogin")]
        public string NotLogin()
        {
            return "未登入";
        }

        // GET: api/Northwind
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        /// <summary>
        /// 產品搜尋
        /// </summary>
        /// <param name="keyword"> 關鍵字 </param>
        /// <returns> 產品列表 </returns>
        [Authorize]
        [HttpGet("Product")]
        public async Task<ActionResult<List<Product>>> GetProducts(string keyword)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            return await _context.QueryProducts(keyword);
        }

        // GET: api/Northwind/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Northwind/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Northwind/PostSalesByYear
        [HttpPost("PostSalesByYear")]
        public async Task<ActionResult<List<SalesByYear>>> PostSalesByYear(QuerySalesByDate salesByDate)
        {
            if (_context.SalesByYears == null)
            {
                return NotFound();
            }

            if(salesByDate.startDate.HasValue == false || salesByDate.endDate.HasValue == false)
            {
                return BadRequest();
            }

            return await _context.QuerySalesByYear(salesByDate.startDate.Value, salesByDate.endDate.Value);
        }

        // POST: api/Northwind
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'NorthwindContext.Orders'  is null.");
          }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Northwind/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// 輸入訂單 ID，取得該訂單的產品與客戶相關資料
        /// </summary>
        /// <param name="id"> 訂單 ID </param>
        /// <returns> 多筆包含產品、客戶名稱等欄位的資料 </returns>
        [Authorize]
        [HttpGet("OrderCustomer/{id}")]
        public async Task<ActionResult<List<OrderCustomer>>> GetOrderDetailsAndCustomer(int id)
        {
            if (_context.OrderCustomers == null)
            {
                return NotFound();
            }

            return await _context.QueryOrderCustomer(id) ;
        }

        /// <summary>
        /// 編輯顧客的聯絡人姓名
        /// </summary>
        /// <param name="id"> 顧客 ID </param>
        /// <param name="ContactName"> 聯絡人姓名 </param>
        /// <returns> 無 </returns>
        [Authorize]
        [HttpPut("EditCustomerName/{id}")]
        public async Task<IActionResult> PutCustomer(string id, string ContactName)
        {
            var customer = new Customer() { CustomerId = id, ContactName = ContactName };
            try
            {
                _context.Customers.Attach(customer);
                _context.Entry(customer).Property(x => x.ContactName).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 檢查顧客是否存在
        /// </summary>
        /// <param name="id"> 顧客 ID </param>
        /// <returns> 是否存在 </returns>
        private bool CustomerExists(string id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }

        /// <summary>
        /// 檢查訂單是否存在
        /// </summary>
        /// <param name="id"> 訂單 ID </param>
        /// <returns> 是否存在 </returns>
        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
