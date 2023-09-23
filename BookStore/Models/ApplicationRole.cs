using System;
using AspNetCore.Identity.MongoDbCore.Models;

namespace BookStore.Models
{
	public class ApplicationRole : MongoIdentityRole<Guid>
	{
		public ApplicationRole()
		{
		}
	}
}

