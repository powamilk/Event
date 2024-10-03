using AutoMapper;
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

namespace BaseSolution.API.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly IEventReadWriteRepository _eventReadWriteRepository;
        private readonly ILocalizationService _localizationService; 

        public EventsController(
            IEventReadOnlyRepository eventReadOnlyRepository,
            IEventReadWriteRepository eventReadWriteRepository,
            ILocalizationService localizationService)
        {
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _eventReadWriteRepository = eventReadWriteRepository;
            _localizationService = localizationService; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents(CancellationToken cancellationToken)
        {
            var viewModel = new EventListWithPaginationViewModel(_eventReadOnlyRepository, _localizationService); 
            await viewModel.HandleAsync(new ViewExampleWithPaginationRequest(), cancellationToken);
            return Ok(viewModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
        {
            var viewModel = new EventViewModel(_eventReadOnlyRepository, _localizationService);
            await viewModel.HandleAsync(id, cancellationToken);

            if (viewModel.Success)
            {
                return Ok(viewModel);
            }

            return NotFound(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequest request, CancellationToken cancellationToken)
        {
            var viewModel = new EventCreateViewModel(_eventReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(request, cancellationToken);

            if (viewModel.Success && viewModel.Data is Event createdEvent)
            {
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, viewModel);
            }

            return BadRequest(viewModel);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventUpdateRequest request, CancellationToken cancellationToken)
        {
            request.Id = id; 
            var viewModel = new EventUpdateViewModel(_eventReadWriteRepository, _localizationService, new MapperConfiguration(cfg => { }).CreateMapper());
            await viewModel.HandleAsync(request, cancellationToken);

            if (viewModel.Success)
            {
                return Ok(viewModel);
            }

            return NotFound(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            var viewModel = new EventDeleteViewModel(_eventReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(new EventDeleteRequest { Id = id }, cancellationToken);

            if (viewModel.Success)
            {
                return NoContent();
            }

            return NotFound(viewModel);
        }
    }
}
