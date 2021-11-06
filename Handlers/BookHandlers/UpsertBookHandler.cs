using MediatR;
using System.Threading;
using System.Threading.Tasks;
using book_web_api.Dtos;
using book_web_api.Data;
using book_web_api.Models;
using book_web_api.Commands.BookCommands;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace book_web_api.Handlers.BookHandlers
{
    public class UpsertBookHandler : IRequestHandler<UpsertBookCommand, BookResponseDto>
    {
        private readonly ApiDbContext _context;
        public UpsertBookHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<BookResponseDto> Handle(UpsertBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.Include(b => b.Authors).Include(b => b.Publisher).FirstOrDefaultAsync(b => b.Id == request.BookRequestDto.Id, cancellationToken: cancellationToken);
            var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id == request.BookRequestDto.PublisherId, cancellationToken: cancellationToken);

            if (book == null)
            {
                var newBook = new Book()
                {
                    Title = request.BookRequestDto.Title,
                    Publisher = publisher,
                    Authors = new List<Author>()
                };

                if (request.BookRequestDto.AuthorIds != null)
                {
                    foreach (var id in request.BookRequestDto.AuthorIds)
                    {
                        var author = await _context.Authors.FindAsync(id);

                        if (author != null)
                        {
                            newBook.Authors.Add(author);
                        }
                    }
                }

                _context.Books.Add(newBook);
                await _context.SaveChangesAsync(cancellationToken);

                return new BookResponseDto()
                {
                    Id = newBook.Id,
                    Title = newBook.Title,
                    Publisher = newBook.Publisher?.Name,
                    Authors = newBook.Authors.Select(a => a.Name).ToList()
                };
            }

            book.Title = request.BookRequestDto.Title;
            book.Publisher = publisher;
            book.Authors.Clear();

            if (request.BookRequestDto.AuthorIds != null)
            {
                foreach (var id in request.BookRequestDto.AuthorIds)
                {
                    var author = await _context.Authors.FindAsync(id);

                    if (author != null)
                    {
                        book.Authors.Add(author);
                    }
                }
            }

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return new BookResponseDto()
            {
                Id = book.Id,
                Title = book.Title,
                Publisher = book.Publisher?.Name,
                Authors = book.Authors.Select(a => a.Name).ToList()
            };
        }
    }
}
