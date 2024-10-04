using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.Orther;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/other")]
    [ApiController]
    public class OrtherController : ControllerBase
    {
        public class OthersController : ControllerBase
        {
            private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
            private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
            private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
            private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
            private readonly ILocalizationService _localizationService;
            private readonly IMapper _mapper;

            public OthersController(
                IEventReadOnlyRepository eventReadOnlyRepository,
                IRegistrationReadWriteRepository registrationReadWriteRepository,
                IReviewReadWriteRepository reviewReadWriteRepository,
                IParticipantReadOnlyRepository participantReadOnlyRepository,
                ILocalizationService localizationService,
                IMapper mapper)
            {
                _eventReadOnlyRepository = eventReadOnlyRepository;
                _registrationReadWriteRepository = registrationReadWriteRepository;
                _reviewReadWriteRepository = reviewReadWriteRepository;
                _participantReadOnlyRepository = participantReadOnlyRepository;
                _localizationService = localizationService;
                _mapper = mapper;
            }

            [HttpGet("events/search")]
            public async Task<IActionResult> SearchEvents([FromQuery] EventSearchRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new EventSearchViewModel(_eventReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(request, cancellationToken);

                if (!viewModel.Success)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = viewModel.Message,
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, data = viewModel.Data });
            }

            [HttpGet("events/filter")]
            public async Task<IActionResult> FilterEventsByStatus([FromQuery] EventFilterRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new EventFilterViewModel(_eventReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(request, cancellationToken);

                if (!viewModel.Success)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = viewModel.Message,
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, data = viewModel.Data });
            }

            [HttpGet("events/stats")]
            public async Task<IActionResult> GetEventStats([FromQuery] EventStatsRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new EventStatsViewModel(_eventReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(request, cancellationToken);

                if (!viewModel.Success)
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = viewModel.Message,
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, data = viewModel.Data });
            }

            [HttpPost("registrations/bulk")]
            public async Task<IActionResult> BulkRegisterParticipants([FromBody] BulkRegistrationRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new BulkRegistrationViewModel(_registrationReadWriteRepository, _localizationService);
                await viewModel.HandleAsync(request, cancellationToken);

                if (!viewModel.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Đăng ký nhiều người tham gia thất bại.",
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, message = "Đăng ký thành công cho nhiều người tham gia.", data = viewModel.Data });
            }
            [HttpPut("registrations/{id}/status")]
            public async Task<IActionResult> UpdateRegistrationStatus(int id, [FromBody] UpdateRegistrationStatusRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new UpdateRegistrationStatusViewModel(_registrationReadWriteRepository, _localizationService);
                await viewModel.HandleAsync(new UpdateRegistrationStatusRequest { Id = id, Status = request.Status }, cancellationToken);

                if (!viewModel.Success)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Cập nhật trạng thái đăng ký thất bại.",
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, message = "Cập nhật trạng thái đăng ký thành công." });
            }

            [HttpPost("reviews/from-organizers")]
            public async Task<IActionResult> CreateReviewFromOrganizers([FromBody] ReviewFromOrganizersRequest request, CancellationToken cancellationToken)
            {
                var viewModel = new ReviewFromOrganizersViewModel(_reviewReadWriteRepository, _eventReadOnlyRepository, _participantReadOnlyRepository, _localizationService, _mapper);
                await viewModel.HandleAsync(request, cancellationToken);

                if (!viewModel.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Tạo đánh giá từ người tổ chức thất bại.",
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, message = "Đánh giá từ người tổ chức thành công.", data = viewModel.Data });
            }
        }
    }
}
