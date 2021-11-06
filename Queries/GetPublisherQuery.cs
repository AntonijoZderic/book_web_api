using book_web_api.Dtos;
using MediatR;
using System.Collections.Generic;

namespace book_web_api.Queries
{
    public record GetPublisherQuery(int? Id) : IRequest<List<PublisherResponseDto>>;
}