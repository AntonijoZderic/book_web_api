using book_web_api.Dtos;
using MediatR;
using System.Collections.Generic;

namespace book_web_api.Queries
{
    public record GetBookQuery(int? Id) : IRequest<List<BookResponseDto>>;
}
