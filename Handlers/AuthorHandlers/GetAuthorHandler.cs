using book_web_api.Data;
using book_web_api.Dtos;
using book_web_api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.AuthorHandlers
{
    public record GetAuthorHandler : IRequestHandler<GetAuthorQuery, List<AuthorResponseDto>>
    {
        private readonly ApiDbContext _context;
        public GetAuthorHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<List<AuthorResponseDto>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            if (request.Id != null)
            {
                return await _context.Authors.Where(a => a.Id == request.Id).Select(a => new AuthorResponseDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Books = a.Books.Select(b => b.Title).ToList()
                }).ToListAsync(cancellationToken: cancellationToken);
            }

            return await _context.Authors.Select(a => new AuthorResponseDto()
            {
                Id = a.Id,
                Name = a.Name,
                Books = a.Books.Select(b => b.Title).ToList()
            }).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
