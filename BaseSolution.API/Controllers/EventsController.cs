using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Event;
using BaseSolution.Application.DataTransferObjects.Event.Request;
using BaseSolution.Application.DataTransferObjects.Example.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.Implements.Services;
using BaseSolution.Infrastructure.ViewModels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseSolution.Application.DataTransferObjects.Event.Request; 


namespace BaseSolution.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly IEventReadWriteRepository _eventReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        public EventsController(
            IEventReadOnlyRepository eventReadOnlyRepository,
            IEventReadWriteRepository eventReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _eventReadWriteRepository = eventReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventsWithPagination([FromQuery] ViewEventWithPaginationRequest request, CancellationToken cancellationToken)
        {
            var vm = new EventListWithPaginationViewModel(_eventReadOnlyRepository, _localizationService);
            await vm.HandleAsync(request, cancellationToken);

            if (vm.Success && vm.Data != null)
            {
                return Ok(new
                {
                    success = true,
                    data = vm.Data
                });
            }

            return NotFound(new
            {
                success = false,
                message = "Không tìm thấy sự kiện",
                error_items = vm.ErrorItems
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
        {
            var viewModel = new EventViewModel(_eventReadOnlyRepository, _localizationService);
            await viewModel.HandleAsync(id, cancellationToken);

            if (viewModel.Success && viewModel.Data != null)
            {
                return Ok(new
                {
                    success = true,
                    data = viewModel.Data
                });
            }

            return NotFound(new
            {
                success = false,
                message = "Không tìm thấy sự kiện",
                error_items = viewModel.ErrorItems
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequest request, CancellationToken cancellationToken)
        {
            var vm = new EventCreateViewModel(_eventReadWriteRepository, _localizationService, _mapper  );
            await vm.HandleAsync(request, cancellationToken);

            if (vm.Success && vm.Data is Event createdEvent)
            {
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, new
                {
                    success = true,
                    data = vm.Data
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Tạo sự kiện thất bại",
                error_items = vm.ErrorItems
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventUpdateRequest updateRequest)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Mã ID sự kiện không khớp" });
            }

            var viewModel = new EventUpdateViewModel(_eventReadWriteRepository, _localizationService, _mapper);
            await viewModel.HandleAsync(updateRequest, CancellationToken.None);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = viewModel.Message,
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new
            {
                success = true,
                message = "Cập nhật sự kiện thành công"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            var viewModel = new EventDeleteViewModel(_eventReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(new EventDeleteRequest { Id = id }, cancellationToken);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy sự kiện",
                    error_items = viewModel.ErrorItems
                });
            }

            return NoContent();
        }
    }
}
