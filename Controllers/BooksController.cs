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
    public class BooksController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public BooksController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDtoResponse>>> GetBooks()
        {
            var books = await _context.Books.Select(book => new BookDtoResponse()
            {
                Id = book.Id,
                Title = book.Title,
                Publisher = book.Publisher.Name,
                Authors = book.Authors.Select(n => n.Name).ToList()
            }).ToListAsync();

            return books;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDtoResponse>> GetBook(int id)
        {
            var book = await _context.Books.Select(book => new BookDtoResponse()
            {
                Id = book.Id,
                Title = book.Title,
                Publisher = book.Publisher.Name,
                Authors = book.Authors.Select(a => a.Name).ToList()
            }).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, BookDtoRequest bookDtoRequest)
        {
            var bookToUpdate = await _context.Books.FindAsync(id);

            if (bookToUpdate != null)
            {
                bookToUpdate.Title = bookDtoRequest.Title;
                bookToUpdate.Publisher = new Publisher();
                bookToUpdate.Authors = new List<Author>();

                if (bookDtoRequest.PublisherId != null)
                {
                    var publisher = await _context.Publishers.FindAsync(bookDtoRequest.PublisherId);
                    if (publisher != null)
                    {
                        bookToUpdate.Publisher = publisher;
                    }
                }

                if (bookDtoRequest.AuthorIds != null && bookDtoRequest.AuthorIds.Any())
                {
                    foreach (var authorId in bookDtoRequest.AuthorIds)
                    {
                        var author = await _context.Authors.FindAsync(authorId);
                        if (author != null)
                        {
                            bookToUpdate.Authors.Add(author);
                        }
                    }
                }

            _context.Entry(bookToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<BookDtoResponse>> PostBook(BookDtoRequest bookDtoRequest)
        {
            var newBook = new Book()
            {
                Title = bookDtoRequest.Title,
                Publisher = bookDtoRequest.PublisherId == null ? new Publisher() : await _context.Publishers.FindAsync(bookDtoRequest.PublisherId),
                Authors = new List<Author>()
            };

            if (bookDtoRequest.AuthorIds != null && bookDtoRequest.AuthorIds.Any())
            {
                foreach (var id in bookDtoRequest.AuthorIds)
                {
                    var author = await _context.Authors.FindAsync(id);

                    if(author != null)
                    {
                        newBook.Authors.Add(author);
                    }
                }
            }

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            var bookDtoResponse = new BookDtoResponse()
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Publisher = newBook.Publisher?.Name,
                Authors = newBook.Authors.Select(a => a.Name).ToList()
            };

            return CreatedAtAction("GetBook", new { id = newBook.Id }, bookDtoResponse);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
