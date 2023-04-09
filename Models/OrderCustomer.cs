using Microsoft.EntityFrameworkCore;

namespace NorthwindWebApi.Models
{
    [Keyless]
    public class OrderCustomer
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ContactName { get; set; }
        public string? Phone { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
