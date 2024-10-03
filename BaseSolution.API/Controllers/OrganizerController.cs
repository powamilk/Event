using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Organizer.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Domain.Entities;
using BaseSolution.Infrastructure.ViewModels.OrganizerViewModel;
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

        public OrganizerController(
            IOrganizerReadOnlyRepository organizerReadOnlyRepository,
            IOrganizerReadWriteRepository organizerReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper)
        {
            _organizerReadOnlyRepository = organizerReadOnlyRepository;
            _organizerReadWriteRepository = organizerReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
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
            try
            {
                if (id != updateRequest.Id)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "ID của người tổ chức không khớp."
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi cập nhật người tổ chức.",
                    error = ex.Message
                });
            }
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
