using MediatR;
using System.Threading;
using System.Threading.Tasks;
using book_web_api.Dtos;
using book_web_api.Data;
using book_web_api.Models;
using book_web_api.Commands.PublisherCommands;
using Microsoft.EntityFrameworkCore;

namespace book_web_api.Handlers.PublisherHandlers
{
    public class UpsertPublisherHandler : IRequestHandler<UpsertPublisherCommand, PublisherResponseDto>
    {
        private readonly ApiDbContext _context;
        public UpsertPublisherHandler(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<PublisherResponseDto> Handle(UpsertPublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Id == request.PublisherRequestDto.Id, cancellationToken: cancellationToken);

            if (publisher == null)
            {
                var newPublisher = new Publisher()
                {
                    Name = request.PublisherRequestDto.Name
                };

                _context.Publishers.Add(newPublisher);
                await _context.SaveChangesAsync(cancellationToken);

                return new PublisherResponseDto()
                {
                    Id = newPublisher.Id,
                    Name = newPublisher.Name
                };
            }

            publisher.Name = request.PublisherRequestDto.Name;

            _context.Entry(publisher).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return new PublisherResponseDto()
            {
                Id = publisher.Id,
                Name = publisher.Name
            };
        }
    }
}