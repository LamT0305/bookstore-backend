using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.IServices;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Controllers
{
    [Authorize]
    [Route("api/v1/author")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;
        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }
        // GET: api/values
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var authors = await authorService.GetAllAuthors();
            return Ok(authors);
        }

        // GET api/values/5
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var author = await authorService.GetByID(id);
            if(author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // POST api/values
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody]Author anAuthor)
        {
            await authorService.CreateAuthor(anAuthor);
            return Ok("Created successfully");
        }

        // PUT api/values/5
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(string id, [FromBody]Author anAuthor)
        {
            var isExisted = await authorService.GetByID(id);

            if(isExisted == null)
            {
                return NotFound();
            }

            await authorService.UpdateAuthor(anAuthor, id);
            return Ok("Updated successfully");
        }

        // DELETE api/values/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var isExisted = await authorService.GetByID(id);

            if (isExisted == null)
            {
                return NotFound();
            }

            await authorService.RemoveAuthor(id);
            return Ok("Deleted author successfully");
        }

        // GET api/values/id
        [HttpGet("book-author/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookAuthor(string id)
        {
            var isExisted = await authorService.GetByID(id);
            if(isExisted == null)
            {
                return NotFound();
            }

            var books = await authorService.GetAllBookByAuthor(id);
            return Ok(books);
        }

        [HttpPut("add-book/{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBookAuthor(string id, [FromBody] string bookId)
        {
            var isExisted = await authorService.GetByID(id);
            if (isExisted == null)
            {
                return NotFound();
            }

            // Check if the bookId parameter is not null or empty
            if (string.IsNullOrEmpty(bookId))
            {
                return BadRequest("Invalid bookId");
            }

            await authorService.AddBookAuthor(bookId, id);
            return Ok("Added book to the author's list of books");
        }

    }
}

