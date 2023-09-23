using System;
using BookStore.Models;

namespace BookStore.IServices
{
	public interface IPublisherService
	{
        Task<IEnumerable<Publisher>> GetPublishers();
        Task<Publisher> GetByID(string id);
        Task CreatePublisher(Publisher aPublisher);
        Task UpdatePublisher(Publisher aPublisher, string Id);
        Task RemovePublisher(string Id);
    }
}

