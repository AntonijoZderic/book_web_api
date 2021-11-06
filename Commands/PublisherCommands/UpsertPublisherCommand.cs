using book_web_api.Dtos;
using MediatR;

namespace book_web_api.Commands.PublisherCommands
{
    public record UpsertPublisherCommand(PublisherRequestDto PublisherRequestDto) : IRequest<PublisherResponseDto>;
}
