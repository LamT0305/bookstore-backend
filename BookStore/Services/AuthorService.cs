using System;
using MongoDB.Driver;
using BookStore.Models;
using Microsoft.Extensions.Options;
using BookStore.Data;
using BookStore.IServices;

namespace BookStore.Services
{
	public class AuthorService : IAuthorService
	{
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IOptions<DatabaseSetting> _dbSettings;

		public AuthorService(IOptions<DatabaseSetting> dbSetting)
		{
            this._dbSettings = dbSetting;
            var mongoClient = new MongoClient(this._dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(this._dbSettings.Value.DatabaseName);

            _authorCollection = mongoDatabase.GetCollection<Author>(this._dbSettings.Value.AuthorCollection);
            _bookCollection = mongoDatabase.GetCollection<Book>(this._dbSettings.Value.BooksCollection);
        }

        public async Task AddBookAuthor(string bookId, string authorId)
        {
            var authorFilter = Builders<Author>.Filter.Eq("Id", authorId);
            var author = await _authorCollection.FindSync(authorFilter).FirstOrDefaultAsync();

            if (author != null && bookId != null)
            {
                if (author.BookIds == null)
                {
                    author.BookIds = new List<string>();
                }

                if (!author.BookIds.Contains(bookId))
                {
                    author.BookIds.Add(bookId);
                }

                // Update the author document in the Authors collection
                await _authorCollection.ReplaceOneAsync(authorFilter, author);
            }
        }

        public async Task CreateAuthor(Author anAuthor)
            => await _authorCollection.InsertOneAsync(anAuthor);

        public async Task<IEnumerable<Author>> GetAllAuthors()
            => await _authorCollection.FindSync(e => true).ToListAsync();

        public async Task<IEnumerable<Book>> GetAllBookByAuthor(string authorID)
            => await _bookCollection.FindSync(e => e.AuthorId == authorID).ToListAsync();

        public async Task<Author> GetByID(string id)
            => await _authorCollection.FindSync(e => e.Id == id).FirstOrDefaultAsync();

        public async Task RemoveAuthor(string Id)
            => await _authorCollection.DeleteOneAsync(e => e.Id == Id);

        public async Task UpdateAuthor(Author anAuthor, string Id)
            => await _authorCollection.ReplaceOneAsync(e => e.Id == Id, anAuthor);
    }
}

