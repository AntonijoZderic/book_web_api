using MediatR;

namespace book_web_api.Commands.AuthorCommands
{
    public record DeleteAuthorCommand(int Id) : IRequest<bool>;
}
