using book_web_api.Dtos;
using MediatR;

namespace book_web_api.Commands.AuthorCommands
{
    public record UpsertAuthorCommand(AuthorRequestDto AuthorRequestDto) : IRequest<AuthorResponseDto>;
}
