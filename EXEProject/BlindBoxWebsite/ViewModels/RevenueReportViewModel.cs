using BlindBoxWebsite.Models;

namespace BlindBoxWebsite.ViewModels
{
    public class RevenueReportViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public List<MonthlyRevenue> MonthlyRevenue { get; set; }
        public List<RevenueByCity> RevenueByCity { get; set; }
        public List<OrderInfo> OrderInfo { get; set; }
        public List<Order> Order { get; set; }
    }

    public class MonthlyRevenue
    {
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class RevenueByCity
    {
        public string City { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
