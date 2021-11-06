using book_web_api.Data;
using book_web_api.Dtos;
using book_web_api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.PublisherHandlers
{
    public record GetPublisherHandler : IRequestHandler<GetPublisherQuery, List<PublisherResponseDto>>
    {
        private readonly ApiDbContext _context;
        public GetPublisherHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<List<PublisherResponseDto>> Handle(GetPublisherQuery request, CancellationToken cancellationToken)
        {
            if (request.Id != null)
            {
                return await _context.Publishers.Where(p => p.Id == request.Id).Select(p => new PublisherResponseDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Books = p.Books.Select(b => b.Title).ToList()
                }).ToListAsync(cancellationToken: cancellationToken);
            }

            return await _context.Publishers.Select(p => new PublisherResponseDto()
            {
                Id = p.Id,
                Name = p.Name,
                Books = p.Books.Select(b => b.Title).ToList()
            }).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}