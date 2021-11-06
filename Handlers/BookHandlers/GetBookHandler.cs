using book_web_api.Data;
using book_web_api.Dtos;
using book_web_api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.BookHandlers
{
    public record GetBookHandler : IRequestHandler<GetBookQuery, List<BookResponseDto>>
    {
        private readonly ApiDbContext _context;
        public GetBookHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<List<BookResponseDto>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            if (request.Id != null)
            {
                return await _context.Books.Where(b => b.Id == request.Id).Select(b => new BookResponseDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Publisher = b.Publisher.Name,
                    Authors = b.Authors.Select(a => a.Name).ToList()
                }).ToListAsync(cancellationToken: cancellationToken);
            }

            return await _context.Books.Select(b => new BookResponseDto()
            {
                Id = b.Id,
                Title = b.Title,
                Publisher = b.Publisher.Name,
                Authors = b.Authors.Select(a => a.Name).ToList()
            }).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
