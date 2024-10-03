using BaseSolution.Application.DataTransferObjects.Registration;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Application.ValueObjects.Common;
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
    public class RegistrationReadOnlyRepository : IRegistrationReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;
        private readonly ILocalizationService _localizationService;

        public RegistrationReadOnlyRepository(AppDbReadOnlyContext dbContext, ILocalizationService localizationService)
        {
            _dbContext = dbContext;
            _localizationService = localizationService;
        }

        public async Task<RequestResult<RegistrationDto>> GetRegistrationByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var registration = await _dbContext.Registrations
                    .Include(r => r.Event)
                    .Include(r => r.Participant)
                    .Where(r => r.Id == id)
                    .Select(r => new RegistrationDto
                    {
                        Id = r.Id,
                        EventId = r.EventId,
                        EventName = r.Event.Name,
                        EventLocation = r.Event.Location,
                        ParticipantId = r.ParticipantId,
                        ParticipantName = r.Participant.Name,
                        ParticipantEmail = r.Participant.Email,
                        RegistrationDate = r.RegistrationDate,
                        Status = r.Status
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (registration == null)
                {
                    return RequestResult<RegistrationDto>.Fail("Không tìm thấy đăng ký.");
                }

                return RequestResult<RegistrationDto>.Succeed(registration);
            }
            catch (Exception ex)
            {
                return RequestResult<RegistrationDto>.Fail(_localizationService["Lỗi trong quá trình truy vấn dữ liệu."], new[]
                {
            new ErrorItem { Error = ex.Message, FieldName = "RegistrationReadOnlyRepository.GetRegistrationByIdAsync" }
        });
            }
        }


        public async Task<RequestResult<IEnumerable<RegistrationDto>>> GetAllRegistrationsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var registrations = await _dbContext.Registrations
                    .Include(r => r.Event)
                    .Include(r => r.Participant)
                    .Select(r => new RegistrationDto
                    {
                        Id = r.Id,
                        EventId = r.EventId,
                        EventName = r.Event.Name,
                        EventLocation = r.Event.Location,
                        ParticipantId = r.ParticipantId,
                        ParticipantName = r.Participant.Name,
                        ParticipantEmail = r.Participant.Email,
                        RegistrationDate = r.RegistrationDate,
                        Status = r.Status
                    })
                    .ToListAsync(cancellationToken);

                return RequestResult<IEnumerable<RegistrationDto>>.Succeed(registrations);
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<RegistrationDto>>.Fail(_localizationService["Lỗi trong quá trình truy vấn dữ liệu."], new[]
                {
                new ErrorItem { Error = ex.Message, FieldName = "RegistrationReadOnlyRepository.GetAllRegistrationsAsync" }
            });
            }
        }
    }
}
