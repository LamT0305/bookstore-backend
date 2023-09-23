using System;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface IAuthorService
	{
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> GetByID(string id);
        Task CreateAuthor(Author anAuthor);
        Task UpdateAuthor(Author anAuthor, string Id);
        Task RemoveAuthor(string Id);
        Task<IEnumerable<Book>> GetAllBookByAuthor(string authorID);
        Task AddBookAuthor(string bookId, string authorId);
    }
}

