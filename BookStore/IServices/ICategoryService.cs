using System;
using BookStore.Models;
namespace BookStore.IServices
{
	public interface ICategoryService
	{
		Task<IEnumerable<Category>> GetAllCategories();
		Task<Category> GetByID(string id);
		Task CreateCategory(Category category);
		Task UpdateCategory(string name, string categoryId);
        Task RemoveCategory(string Id);
	}
}

