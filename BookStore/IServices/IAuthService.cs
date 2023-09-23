using System;
using BookStore.Dtos;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface IAuthService
	{
		Task CreateUserAsync(Customer request);
		Task<string> LoginAsync(UserDto request);
		Task<Customer> getCurrentUser(string token);
	}
}

