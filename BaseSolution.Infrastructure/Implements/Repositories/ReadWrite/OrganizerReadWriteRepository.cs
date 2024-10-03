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
    public class OrganizerReadWriteRepository : IOrganizerReadWriteRepository
    {
        private readonly AppDbReadWriteContext _dbContext;

        public OrganizerReadWriteRepository(AppDbReadWriteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<int>> AddOrganizerAsync(Organizer entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Organizers.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Unable to create organizer", new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "Organizer"
                    }
                });
            }
        }


        public async Task<RequestResult<int>> UpdateOrganizerAsync(Organizer entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Organizers.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Không thể cập nhật người tổ chức: " + ex.Message);
            }
        }

        public async Task<RequestResult<int>> DeleteOrganizerAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var organizer = await _dbContext.Organizers.FindAsync(new object[] { id }, cancellationToken);
                if (organizer == null)
                {
                    return RequestResult<int>.Fail("Không tìm thấy người tổ chức.");
                }

                _dbContext.Organizers.Remove(organizer);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Không thể xóa người tổ chức: " + ex.Message);
            }
        }
    }
}
