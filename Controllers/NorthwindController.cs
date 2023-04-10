using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindWebApi.Models;

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

        // GET: api/Northwind
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Northwind/5
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

        // POST: api/Northwind
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
