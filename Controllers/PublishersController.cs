using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using book_web_api.Dtos;
using MediatR;
using book_web_api.Queries;
using book_web_api.Commands.PublisherCommands;
using System.Linq;

namespace book_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublishersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherResponseDto>>> GetPublisher(int? id)
        {
            var result = await _mediator.Send(new GetPublisherQuery(id));
            return result.Any() ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PublisherResponseDto>> UpsertPublisher([FromBody] PublisherRequestDto publisherRequestDto)
        {
            var result = await _mediator.Send(new UpsertPublisherCommand(publisherRequestDto));
            return CreatedAtAction("GetPublisher", new { id = result.Id }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePublisher(int id) => await _mediator.Send(new DeletePublisherCommand(id)) ? NoContent() : NotFound();
    }
}
