using book_web_api.Commands.AuthorCommands;
using book_web_api.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.AuthorHandlers
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly ApiDbContext _context;
        public DeleteAuthorHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.FindAsync(request.Id);

            if (author == null)
            {
                return false;
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
