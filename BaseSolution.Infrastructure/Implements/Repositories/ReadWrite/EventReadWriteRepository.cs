using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Common;
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
            try
            {
                await _dbContext.Events.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Error occurred while adding event", new List<ErrorItem>
        {
            new ErrorItem
            {
                FieldName = "AddEvent",
                Error = ex.Message
            }
        });
            }
        }


        public async Task<RequestResult<int>> UpdateEventAsync(Event entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Events.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Có lỗi xảy ra khi cập nhật sự kiện.", new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "EventReadWriteRepository.UpdateEventAsync"
                    }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteEventAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var eventEntity = await _dbContext.Events.FindAsync(new object[] { id }, cancellationToken);
                if (eventEntity == null)
                {
                    return RequestResult<int>.Fail("Sự kiện không tồn tại.");
                }

                _dbContext.Events.Remove(eventEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Có lỗi xảy ra khi xóa sự kiện.", new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "EventReadWriteRepository.DeleteEventAsync"
                    }
                });
            }
        }
    }
}
