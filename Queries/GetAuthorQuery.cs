using book_web_api.Dtos;
using MediatR;
using System.Collections.Generic;

namespace book_web_api.Queries
{
    public record GetAuthorQuery(int? Id) : IRequest<List<AuthorResponseDto>>;
}
