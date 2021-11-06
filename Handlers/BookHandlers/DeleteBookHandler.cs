using book_web_api.Commands.BookCommands;
using book_web_api.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.BookHandlers
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly ApiDbContext _context;
        public DeleteBookHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.Id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
