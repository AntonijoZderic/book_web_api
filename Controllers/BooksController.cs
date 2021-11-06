using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using book_web_api.Dtos;
using MediatR;
using book_web_api.Queries;
using book_web_api.Commands.BookCommands;
using System.Linq;

namespace book_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetBook(int? id)
        {
            var result = await _mediator.Send(new GetBookQuery(id));
            return result.Any() ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> UpsertBook([FromBody] BookRequestDto bookRequestDto)
        {
            var result = await _mediator.Send(new UpsertBookCommand(bookRequestDto));
            return CreatedAtAction("GetBook", new { id = result.Id }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id) => await _mediator.Send(new DeleteBookCommand(id)) ? NoContent() : NotFound();
    }
}
