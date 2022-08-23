using BookApi.Data;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly BookApiDbContext dbContext;

        public BooksController(BookApiDbContext dbContext)

        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBook([FromRoute] Guid id)
        {
            var book = await dbContext.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            return Ok(book); 
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await dbContext.Books.ToListAsync(); 
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(RequestField request)
        {
            //Generate random uuid for book and create a new object passing the request fields
            var book = new Book()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Category = request.Category,
                Description = request.Description
            };

            // Insert book into books db [In memory]
            await dbContext.Books.AddAsync(book);

            // Update database 
            await dbContext.SaveChangesAsync();

            return Ok(book);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid id, RequestField request)
        {
            var book = await dbContext.Books.FindAsync(id);

            // Check if book exist in db
            if(book == null)
            {
                return NotFound();
            }

            book.Title = request.Title;
            book.Category = request.Category;
            book.Description = request.Description;

            await dbContext.SaveChangesAsync();

            return Ok(book);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var book = await dbContext.Books.FindAsync(id);

            if(book == null)
            {
                return BadRequest();
            }

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
