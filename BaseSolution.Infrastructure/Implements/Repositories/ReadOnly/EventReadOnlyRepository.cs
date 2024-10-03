using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadOnly
{
    public class EventReadOnlyRepository : IEventReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;

        public EventReadOnlyRepository(AppDbReadOnlyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<EventDto>> GetEventByIdAsync(int id, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (eventEntity == null)
            {
                return RequestResult<EventDto>.Fail("Event not found");
            }

            var eventDto = new EventDto
            {
                Id = eventEntity.Id,
                Name = eventEntity.Name,
                Description = eventEntity.Description,
                Location = eventEntity.Location,
                StartTime = eventEntity.StartTime,
                EndTime = eventEntity.EndTime,
                MaxParticipants = eventEntity.MaxParticipants
            };

            return RequestResult<EventDto>.Succeed(eventDto);
        }

        public async Task<RequestResult<IEnumerable<EventDto>>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            var events = await _dbContext.Events.AsNoTracking().ToListAsync(cancellationToken);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Location = e.Location,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                MaxParticipants = e.MaxParticipants
            }).ToList();

            return RequestResult<IEnumerable<EventDto>>.Succeed(eventDtos);
        }
    }
}
