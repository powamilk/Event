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
    public class RegistrationReadWriteRepository : IRegistrationReadWriteRepository
    {
        private readonly AppDbReadWriteContext _dbContext;

        public RegistrationReadWriteRepository(AppDbReadWriteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<int>> AddRegistrationAsync(Registration entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Registrations.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Không thể tạo đăng ký", new[]
                {
                    new ErrorItem { Error = ex.Message, FieldName = nameof(RegistrationReadWriteRepository.AddRegistrationAsync) }
                });
            }
        }

        public async Task<RequestResult<int>> UpdateRegistrationAsync(Registration entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Registrations.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Không thể cập nhật đăng ký", new[]
                {
                    new ErrorItem { Error = ex.Message, FieldName = nameof(RegistrationReadWriteRepository.UpdateRegistrationAsync) }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteRegistrationAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var registration = await _dbContext.Registrations.FindAsync(new object[] { id }, cancellationToken);
                if (registration == null)
                {
                    return RequestResult<int>.Fail("Không tìm thấy đăng ký.");
                }

                _dbContext.Registrations.Remove(registration);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Không thể xóa đăng ký", new[]
                {
                    new ErrorItem { Error = ex.Message, FieldName = nameof(RegistrationReadWriteRepository.DeleteRegistrationAsync) }
                });
            }
        }
    }
}
