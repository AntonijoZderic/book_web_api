using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using book_web_api.Data;
using book_web_api.Models;
using book_web_api.Dtos;

namespace book_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public AuthorsController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDtoResponse>>> GetAuthors()
        {
            var authors = await _context.Authors.Select(author => new AuthorDtoResponse()
            {
                Id = author.Id,
                Name = author.Name,
                Books = author.Books.Select(b => b.Title).ToList()
            }).ToListAsync();

            return authors;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorDtoResponse>> GetAuthor(int id)
        {
            var author = await _context.Authors.Select(author => new AuthorDtoResponse()
            {
                Id = author.Id,
                Name = author.Name,
                Books = author.Books.Select(b => b.Title).ToList()
            }).FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorDtoRequest authorDtoRequest)
        {
            var authorToUpdate = await _context.Authors.FindAsync(id);

            if (authorToUpdate != null)
            {
                authorToUpdate.Name = authorDtoRequest.Name;

                _context.Entry(authorToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();  
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDtoResponse>> PostAuthor(AuthorDtoRequest authorDtoRequest)
        {
            var newAuthor = new Author
            {
                Name = authorDtoRequest.Name,
                Books = new List<Book>()
            };

            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

            var authorDtoResponse = new AuthorDtoResponse()
            {
                Id = newAuthor.Id,
                Name = newAuthor.Name,
                Books = newAuthor.Books.Select(b => b.Title).ToList(),
            };

            return CreatedAtAction("GetAuthor", new { id = newAuthor.Id }, authorDtoResponse);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
