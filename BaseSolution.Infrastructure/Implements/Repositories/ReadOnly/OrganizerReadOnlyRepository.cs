using BaseSolution.Application.DataTransferObjects.Organizer;
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
    public class OrganizerReadOnlyRepository : IOrganizerReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;

        public OrganizerReadOnlyRepository(AppDbReadOnlyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<OrganizerDto?>> GetOrganizerByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var organizer = await _dbContext.Organizers.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

                if (organizer == null)
                {
                    return RequestResult<OrganizerDto?>.Fail("Không tìm thấy người tổ chức.");
                }

                var organizerDto = new OrganizerDto
                {
                    Id = organizer.Id,
                    Name = organizer.Name,
                    ContactEmail = organizer.ContactEmail,
                    Phone = organizer.Phone
                };

                return RequestResult<OrganizerDto?>.Succeed(organizerDto);
            }
            catch (Exception ex)
            {
                return RequestResult<OrganizerDto?>.Fail("Lỗi khi lấy dữ liệu: " + ex.Message);
            }
        }

        public async Task<RequestResult<IEnumerable<OrganizerDto>>> GetAllOrganizersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var organizers = await _dbContext.Organizers.AsNoTracking().ToListAsync(cancellationToken);
                var organizerDtos = organizers.Select(o => new OrganizerDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    ContactEmail = o.ContactEmail,
                    Phone = o.Phone
                }).ToList();

                return RequestResult<IEnumerable<OrganizerDto>>.Succeed(organizerDtos);
            }
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<OrganizerDto>>.Fail("Lỗi khi lấy danh sách người tổ chức: " + ex.Message);
            }
        }
    }
}
