using System;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface IOrderService
	{
        Task<IEnumerable<Orders>> GetAllOrdersAsync();

        Task<Orders> GetOrderByIdAsync(string orderId);

        Task<List<Orders>> GetOrdersByCustomerIdAsync(string customerId);

        Task CreateOrderAsync(Orders order);

        Task DeleteOrderAsync(string orderId);
    }
}

