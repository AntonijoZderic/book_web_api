using MediatR;

namespace book_web_api.Commands.BookCommands
{
    public record DeleteBookCommand(int Id) : IRequest<bool>;
}
