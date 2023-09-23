using System;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface IBookService
	{
        Task<IEnumerable<Book>> GetBooks();
        Task<Book> GetByID(string id);
        Task CreateBook(Book aBook, IWebHostEnvironment hostingEnvironment);
        Task UpdateBook(Book aBook, string Id);
        Task RemoveBook(string Id);
        Task AddCategoryToBook(string bookId, string categoryId);
        Task<IEnumerable<Book>> SearchBook(string bookName);
        Task<IEnumerable<Book>> GetBookByCategory(string aCategory);
    }
}

