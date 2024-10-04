using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
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
    public class EventReadOnlyRepository : IEventReadOnlyRepository
    {
        private readonly AppDbReadOnlyContext _dbContext;

        public EventReadOnlyRepository(AppDbReadOnlyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<EventDto>> GetEventByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var eventEntity = await _dbContext.Events
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (eventEntity == null)
                {
                    return RequestResult<EventDto>.Fail("Sự kiện không tồn tại.");
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
            catch (Exception ex)
            {
                return RequestResult<EventDto>.Fail("Có lỗi xảy ra khi lấy thông tin sự kiện.", new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "EventReadOnlyRepository.GetEventByIdAsync"
                    }
                });
            }
        }

        public async Task<RequestResult<IEnumerable<EventDto>>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var events = await _dbContext.Events
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

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
            catch (Exception ex)
            {
                return RequestResult<IEnumerable<EventDto>>.Fail("Có lỗi xảy ra khi lấy danh sách sự kiện.", new[]
                {
                    new ErrorItem
                    {
                        Error = ex.Message,
                        FieldName = "EventReadOnlyRepository.GetAllEventsAsync"
                    }
                });
            }
        }

        public async Task<IEnumerable<EventDto>> SearchEventsAsync(string name, string location, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken)
        {
            var query = _dbContext.Events.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.EndTime <= endDate.Value);
            }

            var result = await query.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Location = e.Location,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                MaxParticipants = e.MaxParticipants
            }).ToListAsync(cancellationToken);

            return result;
        }

        public async Task<IEnumerable<EventDto>> FilterEventsByStatusAsync(string status, CancellationToken cancellationToken)
        {
            var currentTime = DateTime.UtcNow; 

            var query = _dbContext.Events.AsQueryable();

            if (status == "ongoing")
            {
                query = query.Where(e => e.StartTime <= currentTime && e.EndTime >= currentTime);
            }
            else if (status == "completed")
            {
                query = query.Where(e => e.EndTime < currentTime);
            }
            else if (status == "upcoming")
            {
                query = query.Where(e => e.StartTime > currentTime);
            }

            var result = await query
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Location = e.Location,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    MaxParticipants = e.MaxParticipants
                }).ToListAsync(cancellationToken);

            return result;
        }


        public async Task<EventStatsDto> GetEventStatsAsync(DateTime? startDate, DateTime? endDate, string status, CancellationToken cancellationToken)
        {
            var currentTime = DateTime.UtcNow;
            var query = _dbContext.Events.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.EndTime <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "ongoing")
                {
                    query = query.Where(e => e.StartTime <= currentTime && e.EndTime >= currentTime);
                }
                else if (status == "completed")
                {
                    query = query.Where(e => e.EndTime < currentTime);
                }
                else if (status == "upcoming")
                {
                    query = query.Where(e => e.StartTime > currentTime);
                }
            }

            var totalEvents = await query.CountAsync(cancellationToken);
            var totalParticipants = await query.SumAsync(e => e.MaxParticipants, cancellationToken);
            var averageParticipantsPerEvent = totalEvents > 0 ? totalParticipants / (double)totalEvents : 0;
            var eventsByStatus = await query
                .GroupBy(e => new
                {
                    Status = e.StartTime <= currentTime && e.EndTime >= currentTime ? "ongoing" :
                             e.EndTime < currentTime ? "completed" : "upcoming"
                })
                .Select(group => new EventStatusStats
                {
                    Status = group.Key.Status,
                    Count = group.Count(),
                    Participants = group.Sum(e => e.MaxParticipants)
                }).ToListAsync(cancellationToken);

            return new EventStatsDto
            {
                TotalEvents = totalEvents,
                TotalParticipants = totalParticipants,
                AverageParticipantsPerEvent = averageParticipantsPerEvent,
                EventsByStatus = eventsByStatus,
                EventsInDateRange = new EventsInDateRangeDto
                {
                    Count = totalEvents,
                    Participants = totalParticipants
                }
            };
        }

    }
}
