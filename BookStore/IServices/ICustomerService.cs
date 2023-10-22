using System;
using BookStore.Dtos;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface ICustomerService
	{
        Task UpdateCustomer(CustomerDto aCustomer, string Id);

        Task<IEnumerable<Orders>> ViewOrdersHistory(string customerId);

        Task<List<CartItems>> ViewCartUser(string CustomerId);

        Task<CartDto> AddToCart(string CustomerId, string BookId, int quantity);

        Task UpdateCartUser(string customerId, string BookId, int quantity);

        Task RemoveCartItems(string CustomerId, string BookId);

        Task CreateOrder(string customerId);

        string GetIdByToken(string token);
    }
}

