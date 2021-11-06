using MediatR;
using System.Threading;
using System.Threading.Tasks;
using book_web_api.Dtos;
using book_web_api.Data;
using book_web_api.Models;
using book_web_api.Commands.AuthorCommands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace book_web_api.Handlers.AuthorHandlers
{
    public class UpsertAuthorHandler : IRequestHandler<UpsertAuthorCommand, AuthorResponseDto>
    {
        private readonly ApiDbContext _context;
        public UpsertAuthorHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<AuthorResponseDto> Handle(UpsertAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == request.AuthorRequestDto.Id, cancellationToken: cancellationToken);
            
            if (author == null)
            {
                var newAuthor = new Author()
                {
                    Name = request.AuthorRequestDto.Name
                };

                _context.Authors.Add(newAuthor);
                await _context.SaveChangesAsync(cancellationToken);

                return new AuthorResponseDto()
                {
                    Id = newAuthor.Id,
                    Name = newAuthor.Name
                };
            }

            author.Name = request.AuthorRequestDto.Name;

            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthorResponseDto()
            {
                Id = author.Id,
                Name = author.Name,
                Books = author.Books.Select(b => b.Title).ToList()
            };
        }
    }
}
