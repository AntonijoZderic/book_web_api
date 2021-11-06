using book_web_api.Commands.PublisherCommands;
using book_web_api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace book_web_api.Handlers.PublisherHandlers
{
    public class DeletePublisherHandler : IRequestHandler<DeletePublisherCommand, bool>
    {
        private readonly ApiDbContext _context;
        public DeletePublisherHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await _context.Publishers.Include(p => p.Books).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            if (publisher == null)
            {
                return false;
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}