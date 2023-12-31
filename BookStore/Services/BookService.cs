﻿using System;
using BookStore.Data;
using BookStore.IServices;
using BookStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStore.Services
{
    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IOptions<DatabaseSetting> _dbSettings;

        public BookService(IOptions<DatabaseSetting> dbSetting)
        {
            this._dbSettings = dbSetting;
            var mongoClient = new MongoClient(this._dbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(this._dbSettings.Value.DatabaseName);

            _bookCollection = mongoDatabase.GetCollection<Book>(this._dbSettings.Value.BooksCollection);
            _authorCollection = mongoDatabase.GetCollection<Author>(this._dbSettings.Value.AuthorCollection);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookCollection.Find(e => true).ToListAsync();
        }

        public async Task<Book> GetByID(string id)
        {
            return await _bookCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateBook(Book aBook, IWebHostEnvironment hostingEnvironment)
        {
            //check if the book is existing or not
            
            // Check if the 'aBook' object is null
            if (aBook == null)
            {
                throw new Exception("The 'aBook' parameter cannot be null.");
            }

            // Check if the 'Image' property is null
            if (aBook.Image == null)
            {
                throw new Exception("The 'Image' property cannot be null.");
            }

            try
            {
                // Get the wwwroot/images directory path
                var uploadsPath = Path.Combine(hostingEnvironment.WebRootPath, "images");

                // Create a unique file name (e.g., using a GUID)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + aBook.Image.FileName;

                // Combine the directory path with the unique file name to get the full file path
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await aBook.Image.CopyToAsync(stream);
                }

                // Update the 'ImagePath' property of 'aBook' with the relative path to the image file
                aBook.ImagePath = "/images/" + uniqueFileName;

                // Insert 'aBook' into the collection
                // Note: Here, you should only insert the book information, not the image itself
                await _bookCollection.InsertOneAsync(aBook);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the book: " + ex.Message);
            }
        }





        public async Task UpdateBook(Book aBook, string Id)
        {
            await _bookCollection.ReplaceOneAsync(e => e.Id == Id, aBook);
        }

        public async Task RemoveBook(string Id)
        {
            await _bookCollection.DeleteOneAsync(e => e.Id == Id);
        }

        public async Task AddCategoryToBook(string bookId, string categoryId)
        {
            var filter = Builders<Book>.Filter.Eq("Id", bookId);
            var book = await _bookCollection.Find(filter).FirstOrDefaultAsync();

            if (book != null)
            {
                if (book.BookCategories == null)
                {
                    book.BookCategories = new List<string>();
                }

                if (!book.BookCategories.Contains(categoryId))
                {
                    book.BookCategories.Add(categoryId);
                    await _bookCollection.ReplaceOneAsync(filter, book);
                }
                else
                {
                    throw new Exception($"Already contained category: {categoryId}");
                }
            }
        }

        public async Task<IEnumerable<Book>> SearchBook(string bookName)
        {
            return await _bookCollection.Find(e => e.Title.ToLower().Contains(bookName.ToLower())).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBookByCategory(string aCategory)
        {
            var books = await _bookCollection.Find(e => true).ToListAsync();

            List<Book> bookList = new List<Book>();

            foreach(var e in books)
            {
                foreach (var cat in e.BookCategories)
                {
                    if (cat.ToString().Equals(aCategory))
                    {
                        bookList.Add(e);
                    }
                }
            }

            return bookList;
        }
    }
}

