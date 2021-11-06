using book_web_api.Dtos;
using MediatR;

namespace book_web_api.Commands.BookCommands
{
    public record UpsertBookCommand(BookRequestDto BookRequestDto) : IRequest<BookResponseDto>;
}
