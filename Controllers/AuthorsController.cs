using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using book_web_api.Dtos;
using MediatR;
using book_web_api.Queries;
using book_web_api.Commands.AuthorCommands;
using System.Linq;

namespace book_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAuthor(int? id)
        {
            var result = await _mediator.Send(new GetAuthorQuery(id));
            return result.Any() ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<AuthorResponseDto>> UpsertAuthor([FromBody] AuthorRequestDto authorRequestDto)
        {
            var result = await _mediator.Send(new UpsertAuthorCommand(authorRequestDto));
            return CreatedAtAction("GetAuthor", new { id = result.Id }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuthor(int id) => await _mediator.Send(new DeleteAuthorCommand(id)) ? NoContent() : NotFound();
    }
}
