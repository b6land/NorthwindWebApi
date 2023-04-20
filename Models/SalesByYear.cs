using Microsoft.EntityFrameworkCore;

namespace NorthwindWebApi.Models
{
    [Keyless]
    public class SalesByYear
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderID { get; set; }
        public string? Subtotal { get; set; }
        public int Year { get; set; }
    }

    public class QuerySalesByDate
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
    }
}
