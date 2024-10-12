using BlindBoxWebsite.Models;

namespace BlindBoxWebsite.Interfaces
{
    public interface IProductRepository
    {
        Task<int> AddNewOrder(Order order);
        Task<int> AddNewOrderItem(OrderItem orderItem);
        Task<int> AddNewPayment(Payment payment);
    }
}
