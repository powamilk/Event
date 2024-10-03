using BaseSolution.Application.DataTransferObjects.Participant;
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
    public class ParticipantReadOnlyRepository : IParticipantReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;

        public ParticipantReadOnlyRepository(AppDbReadOnlyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<ParticipantDto>> GetParticipantByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var participant = await _dbContext.Participants.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
                if (participant == null)
                {
                    return RequestResult<ParticipantDto>.Fail("Participant not found");
                }

                var participantDto = new ParticipantDto
                {
                    Id = participant.Id,
                    Name = participant.Name,
                    Email = participant.Email,
                    Phone = participant.Phone,
                    RegisteredAt = participant.RegisteredAt
                };

                return RequestResult<ParticipantDto>.Succeed(participantDto);
            }
            catch (Exception ex)
            {
                return RequestResult<ParticipantDto>.Fail(ex.Message);
            }
        }

        public async Task<RequestResult<IEnumerable<ParticipantDto>>> GetAllParticipantsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var participants = await _dbContext.Participants.AsNoTracking().ToListAsync(cancellationToken);
                var participantDtos = participants.Select(p => new ParticipantDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    Phone = p.Phone,
                    RegisteredAt = p.RegisteredAt
                }).ToList();

                return RequestResult<IEnumerable<ParticipantDto>>.Succeed(participantDtos);
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<ParticipantDto>>.Fail(ex.Message);
            }
        }
    }
}
