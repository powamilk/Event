using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Participant.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.ViewModels.ParticipantViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/participants")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly IParticipantReadWriteRepository _participantReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IValidator<ParticipantCreateRequest> _createValidator;
        private readonly IValidator<ParticipantUpdateRequest> _updateValidator;

        public ParticipantsController(
            IParticipantReadOnlyRepository participantReadOnlyRepository,
            IParticipantReadWriteRepository participantReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IValidator<ParticipantCreateRequest> createValidator,
            IValidator<ParticipantUpdateRequest> updateValidator)
        {
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _participantReadWriteRepository = participantReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetParticipantsWithPagination([FromQuery] ViewParticipantWithPaginationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ParticipantListWithPaginationViewModel(_participantReadOnlyRepository, _localizationService);
                await vm.HandleAsync(request, cancellationToken);

                if (vm.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = vm.Data
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Có lỗi xảy ra khi lấy danh sách người tham gia.",
                    error_items = vm.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi máy chủ: " + ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipantById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ParticipantViewModel(_participantReadOnlyRepository, _localizationService);
                await vm.HandleAsync(id, cancellationToken);

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
                    message = "Không tìm thấy người tham gia.",
                    error_items = vm.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi máy chủ: " + ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateParticipant([FromBody] ParticipantCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin người tham gia không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var vm = new ParticipantCreateViewModel(_participantReadWriteRepository, _localizationService);
            await vm.HandleAsync(request, cancellationToken);

            if (vm.Success && vm.Data is Participant createdParticipant)
            {
                return CreatedAtAction(nameof(GetParticipantById), new { id = createdParticipant.Id }, new
                {
                    success = true,
                    data = vm.Data
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Tạo người tham gia thất bại.",
                error_items = vm.ErrorItems
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantUpdateRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Mã ID người tham gia không khớp."
                });
            }

            var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin người tham gia không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var vm = new ParticipantUpdateViewModel(_participantReadWriteRepository, _localizationService, _mapper);
            await vm.HandleAsync(request, cancellationToken);

            if (vm.Success)
            {
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật người tham gia thành công."
                });
            }

            return NotFound(new
            {
                success = false,
                message = "Không tìm thấy người tham gia.",
                error_items = vm.ErrorItems
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new ParticipantDeleteViewModel(_participantReadWriteRepository, _localizationService);
                await vm.HandleAsync(new ParticipantDeleteRequest { Id = id }, cancellationToken);

                if (vm.Success)
                {
                    return NoContent();
                }

                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy người tham gia.",
                    error_items = vm.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Lỗi máy chủ: " + ex.Message
                });
            }
        }
    }
}
