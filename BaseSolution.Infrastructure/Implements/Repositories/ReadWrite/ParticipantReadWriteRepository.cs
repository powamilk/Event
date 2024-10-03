using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.ValueObjects.Common;
using BaseSolution.Application.ValueObjects.Response;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Implements.Repositories.ReadWrite
{
    public class ParticipantReadWriteRepository : IParticipantReadWriteRepository
    {
        private readonly AppDbReadWriteContext _dbContext;

        public ParticipantReadWriteRepository(AppDbReadWriteContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<int>> AddParticipantAsync(Participant entity, CancellationToken cancellationToken)
        {
            try
            {
                var existingParticipant = await _dbContext.Participants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Email == entity.Email, cancellationToken);

                if (existingParticipant != null)
                {
                    return RequestResult<int>.Fail("Email đã tồn tại", new[]
                    {
                new ErrorItem
                {
                    FieldName = "ParticipantReadWriteRepository.AddParticipantAsync",
                    Error = "Email đã tồn tại"
                }
            });
                }

                await _dbContext.Participants.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Tạo người tham gia thất bại.", new[]
                {
            new ErrorItem
            {
                FieldName = "ParticipantReadWriteRepository.AddParticipantAsync",
                Error = ex.Message
            }
        });
            }
        }

        public async Task<RequestResult<int>> UpdateParticipantAsync(Participant entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Participants.Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(entity.Id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Unable to update participant", new[]
                {
                    new ErrorItem { Error = ex.Message, FieldName = "Participant" }
                });
            }
        }

        public async Task<RequestResult<int>> DeleteParticipantAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var participant = await _dbContext.Participants.FindAsync(new object[] { id }, cancellationToken);
                if (participant == null)
                {
                    return RequestResult<int>.Fail("Participant not found");
                }

                _dbContext.Participants.Remove(participant);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return RequestResult<int>.Succeed(id);
            }
            catch (Exception ex)
            {
                return RequestResult<int>.Fail("Unable to delete participant", new[]
                {
                    new ErrorItem { Error = ex.Message, FieldName = "Participant" }
                });
            }
        }
    }
}
