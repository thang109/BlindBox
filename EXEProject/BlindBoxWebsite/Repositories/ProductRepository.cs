using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Repositories
{
    public class ProductRepository : BaseRepository<Order>, IProductRepository
    {
        public ProductRepository(BlindBoxContext context) : base(context)
        {
        }

        public async Task<int> AddNewOrder(Order order)
        {
            using (var context = new BlindBoxContext())
            {
                var lastOrder = await context.Orders.OrderByDescending(x => x.OrderId).FirstOrDefaultAsync();
                var lastId = lastOrder.OrderId + 1;
                order.OrderId = lastId;
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
                return order.OrderId;
            }
        }

        public async Task<int> AddNewOrderItem(OrderItem orderItem)
        {
            using (var context = new BlindBoxContext())
            {
                var lastOrderItem = await context.OrderItems.OrderByDescending(x => x.OrderItemId).FirstOrDefaultAsync();
                var lastId = lastOrderItem.OrderItemId + 1;
                orderItem.OrderItemId = lastId;
                await context.OrderItems.AddAsync(orderItem);
                await context.SaveChangesAsync();
                return orderItem.OrderItemId;
            }
        }

        public async Task<int> AddNewPayment(Payment payment)
        {
            using (var context = new BlindBoxContext())
            {
                var lastPayment = await context.Payments.OrderByDescending(x => x.PaymentId).FirstOrDefaultAsync();
                var lastId = lastPayment.PaymentId + 1;
                payment.PaymentId = lastId;
                await context.Payments.AddAsync(payment);
                await context.SaveChangesAsync();
                return payment.PaymentId;
            }
        }

        public async Task<int> AddNewOrderInfo(OrderInfo orderInfo)
        {
            using (var context = new BlindBoxContext())
            {
                var lastOrderInfo = await context.OrderInfos.OrderByDescending(x => x.OrderInfoId).FirstOrDefaultAsync();
                var lastId = lastOrderInfo.OrderInfoId + 1;
                orderInfo.OrderInfoId = lastId;
                await context.OrderInfos.AddAsync(orderInfo);
                await context.SaveChangesAsync();
                return orderInfo.OrderInfoId;
            }
        }

        public void UpdatePayment(Payment payment)
        {
            var existingPayment = _context.Payments.FirstOrDefault(p => p.PaymentId == payment.PaymentId);
            if (existingPayment != null)
            {
                existingPayment.Status = payment.Status;
                existingPayment.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
            }
        }
    }
}
