using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class EventReadWriteRepository : IEventReadWriteRepository
    {
        private readonly AppDbReadWriteContext _dbContext;

        public EventReadWriteRepository(AppDbReadWriteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<int>> AddEventAsync(Event entity, CancellationToken cancellationToken)
        {
            await _dbContext.Events.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return RequestResult<int>.Succeed(entity.Id);
        }

        public async Task<RequestResult<int>> UpdateEventAsync(Event entity, CancellationToken cancellationToken)
        {
            _dbContext.Events.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return RequestResult<int>.Succeed(entity.Id);
        }

        public async Task<RequestResult<int>> DeleteEventAsync(int id, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events.FindAsync(new object[] { id }, cancellationToken);
            if (eventEntity == null)
            {
                return RequestResult<int>.Fail("Event not found");
            }

            _dbContext.Events.Remove(eventEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return RequestResult<int>.Succeed(id);
        }
    }
}
