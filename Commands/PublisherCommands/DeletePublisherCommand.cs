using MediatR;

namespace book_web_api.Commands.PublisherCommands
{
    public record DeletePublisherCommand(int Id) : IRequest<bool>;
}