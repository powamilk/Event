using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Registration.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.Implements.Repositories.ReadOnly;
using BaseSolution.Infrastructure.ViewModels.RegistrationViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/registrations")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationReadWriteRepository _registrationReadWriteRepository;
        private readonly IRegistrationReadOnlyRepository _registrationReadOnlyRepository;
        private readonly IEventReadOnlyRepository _eventReadOnlyRepository;
        private readonly IParticipantReadOnlyRepository _participantReadOnlyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IValidator<RegistrationCreateRequest> _createValidator;
        private readonly IValidator<RegistrationUpdateRequest> _updateValidator;

        public RegistrationsController(
            IRegistrationReadWriteRepository registrationReadWriteRepository,
            IRegistrationReadOnlyRepository registrationReadOnlyRepository,
            IEventReadOnlyRepository eventReadOnlyRepository,
            IParticipantReadOnlyRepository participantReadOnlyRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IValidator<RegistrationCreateRequest> createValidator,
            IValidator<RegistrationUpdateRequest> updateValidator)
        {
            _registrationReadWriteRepository = registrationReadWriteRepository;
            _registrationReadOnlyRepository = registrationReadOnlyRepository;
            _eventReadOnlyRepository = eventReadOnlyRepository;
            _participantReadOnlyRepository = participantReadOnlyRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegistrations(CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = new RegistrationListWithPaginationViewModel(_registrationReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(new ViewRegistrationWithPaginationRequest(), cancellationToken);

                if (!viewModel.Success)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Không tìm thấy danh sách đăng ký.",
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, data = viewModel.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi lấy danh sách đăng ký.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = new RegistrationViewModel(_registrationReadOnlyRepository, _localizationService);
                await viewModel.HandleAsync(id, cancellationToken);

                if (!viewModel.Success || viewModel.Data == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đăng ký." });
                }

                return Ok(new { success = true, data = viewModel.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi lấy thông tin đăng ký.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistration([FromBody] RegistrationCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin đăng ký không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            try
            {
                var vm = new RegistrationCreateViewModel(_registrationReadWriteRepository, _localizationService);
                await vm.HandleAsync(request, cancellationToken);

                if (vm.Success)
                {
                    return CreatedAtAction(nameof(GetRegistrationById), new { id = vm.Data }, new
                    {
                        success = true,
                        data = vm.Data
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Tạo đăng ký thất bại.",
                    error_items = vm.ErrorItems
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi tạo đăng ký.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromBody] RegistrationUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Mã đăng ký không khớp." });
            }

            var validationResult = await _updateValidator.ValidateAsync(updateRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin đăng ký không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            try
            {
                var viewModel = new RegistrationUpdateViewModel(
                    _registrationReadWriteRepository,
                    _registrationReadOnlyRepository,
                    _eventReadOnlyRepository,
                    _participantReadOnlyRepository,
                    _localizationService,
                    _mapper
                );

                await viewModel.HandleAsync(updateRequest, cancellationToken);

                if (!viewModel.Success)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Không tìm thấy thông tin đăng ký hoặc thông tin không hợp lệ.",
                        error_items = viewModel.ErrorItems
                    });
                }

                return Ok(new { success = true, message = "Cập nhật đăng ký thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi cập nhật đăng ký.", error = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id, CancellationToken cancellationToken)
        {
            try
            {
                var vm = new RegistrationDeleteViewModel(_registrationReadWriteRepository, _localizationService);
                await vm.HandleAsync(new RegistrationDeleteRequest { Id = id }, cancellationToken);

                if (!vm.Success)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đăng ký." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xóa đăng ký.", error = ex.Message });
            }
        }
    }
}
