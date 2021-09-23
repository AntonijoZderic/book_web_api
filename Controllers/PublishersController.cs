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
    public class PublishersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PublishersController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDtoResponse>>> GetPublishers()
        {
            var publishers = await _context.Publishers.Select(publisher => new PublisherDtoResponse()
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Books = publisher.Books.Select(b => b.Title).ToList()
            }).ToListAsync();

            return publishers;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublisherDtoResponse>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.Select(publisher => new PublisherDtoResponse()
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Books = publisher.Books.Select(b => b.Title).ToList()
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (publisher == null)
            {
                return NotFound();
            }

            return publisher;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutPublisher(int id, PublisherDtoRequest publisherDtoRequest)
        {
            var publisherToUpdate = await _context.Publishers.FindAsync(id);

            if (publisherToUpdate != null)
            {
                publisherToUpdate.Name = publisherDtoRequest.Name;

                _context.Entry(publisherToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDtoResponse>> PostPublisher(PublisherDtoRequest publisherDtoRequest)
        {
            var newPublisher = new Publisher()
            {
                Name = publisherDtoRequest.Name,
                Books = new List<Book>()
            };

            _context.Publishers.Add(newPublisher);
            await _context.SaveChangesAsync();

            var publisherDtoResponse = new PublisherDtoResponse()
            {
                Id = newPublisher.Id,
                Name = newPublisher.Name,
                Books = newPublisher.Books.Select(b => b.Title).ToList(),
            };

            return CreatedAtAction("GetPublisher", new { id = newPublisher.Id }, publisherDtoResponse);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
