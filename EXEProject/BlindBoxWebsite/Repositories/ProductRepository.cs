using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X9;

namespace BlindBoxWebsite.Repositories
{
    public class ProductRepository : BaseRepository<Order>, IProductRepository
    {
        public ProductRepository(BlindBoxContext context) : base(context)
        {
        }

        public async Task<int> AddNewOrder(Order order)
        {
            using var context = new BlindBoxContext();
            var lastOrder = await context.Orders.OrderByDescending(x => x.OrderId).FirstOrDefaultAsync();
            var lastId = lastOrder.OrderId + 1;
            order.OrderId = lastId;
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            return order.OrderId;
        }

        public async Task<int> AddNewOrderItem(OrderItem orderItem)
        {
            var lastOrderItem = await _context.OrderItems.OrderByDescending(x => x.OrderItemId).FirstOrDefaultAsync();
            var lastId = lastOrderItem.OrderItemId + 1;
            orderItem.OrderItemId = lastId;
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItem.OrderItemId;
        }

        public async Task<int> AddNewPayment(Payment payment)
        {
            var lastPayment = await _context.Payments.OrderByDescending(x => x.PaymentId).FirstOrDefaultAsync();
            var lastId = lastPayment.PaymentId + 1;
            payment.PaymentId = lastId;
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment.PaymentId;
        }

        public async Task<int> AddNewOrderInfo(OrderInfo orderInfo)
        {
            var lastOrderInfo = await _context.OrderInfos.OrderByDescending(x => x.OrderInfoId).FirstOrDefaultAsync();
            var lastId = lastOrderInfo.OrderInfoId + 1;
            orderInfo.OrderInfoId = lastId;
            await _context.OrderInfos.AddAsync(orderInfo);
            await _context.SaveChangesAsync();
            return orderInfo.OrderInfoId;
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

        public void UpdateStockBlindBox(int blindBoxId, int quantity)
        {
            var blindBox = _context.BlindBoxes.FirstOrDefault(b => b.BlindBoxId == blindBoxId);

            if(blindBox != null)
            {
                blindBox.Stock -= quantity;
                _context.BlindBoxes.Update(blindBox);
                _context.SaveChanges();
            }
        }
    }
}
