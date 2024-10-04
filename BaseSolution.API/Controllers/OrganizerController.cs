using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.ViewModels.OrganizerViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/organizers")]
    [ApiController]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerReadOnlyRepository _organizerReadOnlyRepository;
        private readonly IOrganizerReadWriteRepository _organizerReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IValidator<OrganizerCreateRequest> _createValidator;
        private readonly IValidator<OrganizerUpdateRequest> _updateValidator;

        public OrganizerController(
            IOrganizerReadOnlyRepository organizerReadOnlyRepository,
            IOrganizerReadWriteRepository organizerReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IValidator<OrganizerCreateRequest> createValidator,
            IValidator<OrganizerUpdateRequest> updateValidator)
        {
            _organizerReadOnlyRepository = organizerReadOnlyRepository;
            _organizerReadWriteRepository = organizerReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrganizers(CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = new OrganizerListWithPaginationViewModel(_organizerReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(new ViewOrganizerWithPaginationRequest(), cancellationToken);

                if (viewModel.Success)
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
                    message = "Không tìm thấy danh sách người tổ chức.",
                    error_items = viewModel.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi lấy danh sách người tổ chức.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizerById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = new OrganizerViewModel(_organizerReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(id, cancellationToken);

                if (viewModel.Success)
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
                    message = "Không tìm thấy người tổ chức.",
                    error_items = viewModel.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi lấy thông tin người tổ chức.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganizer([FromBody] OrganizerCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin người tổ chức không hợp lệ",
                    error_items = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var viewModel = new OrganizerCreateViewModel(_organizerReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(request, cancellationToken);

            if (viewModel.Success && viewModel.Data is int createdOrganizerId)
            {
                return CreatedAtAction(nameof(GetOrganizerById), new { id = createdOrganizerId }, new
                {
                    success = true,
                    data = viewModel.Data
                });
            }

            return BadRequest(new
            {
                success = false,
                message = viewModel.Message,
                error_items = viewModel.ErrorItems
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrganizer(int id, [FromBody] OrganizerUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "ID của người tổ chức không khớp."
                });
            }

            var validationResult = await _updateValidator.ValidateAsync(updateRequest, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin người tổ chức không hợp lệ",
                    error_items = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var viewModel = new OrganizerUpdateViewModel(_organizerReadWriteRepository, _localizationService, _mapper);
            await viewModel.HandleAsync(updateRequest, cancellationToken);

            if (viewModel.Success)
            {
                return Ok(new
                {
                    success = true,
                    message = "Cập nhật thông tin người tổ chức thành công."
                });
            }

            return NotFound(new
            {
                success = false,
                message = "Không tìm thấy người tổ chức.",
                error_items = viewModel.ErrorItems
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizer(int id, CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = new OrganizerDeleteViewModel(_organizerReadWriteRepository, _localizationService);
                await viewModel.HandleAsync(new OrganizerDeleteRequest { Id = id }, cancellationToken);

                if (viewModel.Success)
                {
                    return NoContent();
                }

                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy người tổ chức.",
                    error_items = viewModel.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi xóa người tổ chức.",
                    error = ex.Message
                });
            }
        }
    }
}
