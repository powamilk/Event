using AutoMapper;
using BaseSolution.Application.DataTransferObjects.Review;
using BaseSolution.Application.DataTransferObjects.Review.Request;
using BaseSolution.Application.Interfaces.Repositories.ReadOnly;
using BaseSolution.Application.Interfaces.Repositories.ReadWrite;
using BaseSolution.Application.Interfaces.Services;
using BaseSolution.Infrastructure.ViewModels.ReviewViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseSolution.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewReadOnlyRepository _reviewReadOnlyRepository;
        private readonly IReviewReadWriteRepository _reviewReadWriteRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IValidator<ReviewCreateRequest> _createValidator;
        private readonly IValidator<ReviewUpdateRequest> _updateValidator;

        public ReviewsController(
            IReviewReadOnlyRepository reviewReadOnlyRepository,
            IReviewReadWriteRepository reviewReadWriteRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IValidator<ReviewCreateRequest> createValidator,
            IValidator<ReviewUpdateRequest> updateValidator)
        {
            _reviewReadOnlyRepository = reviewReadOnlyRepository;
            _reviewReadWriteRepository = reviewReadWriteRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews(CancellationToken cancellationToken)
        {
            var viewModel = new ReviewListWithPaginationViewModel(_reviewReadOnlyRepository, _localizationService, _mapper);
            await viewModel.HandleAsync(null, cancellationToken);

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


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id, CancellationToken cancellationToken)
        {
            var viewModel = new ReviewViewModel(_reviewReadOnlyRepository, _localizationService);
            await viewModel.HandleAsync(id, cancellationToken);

            if (!viewModel.Success || viewModel.Data == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy đánh giá.",
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new { success = true, data = viewModel.Data });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin đánh giá không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            var viewModel = new ReviewCreateViewModel(_reviewReadWriteRepository, _localizationService, _mapper);
            await viewModel.HandleAsync(request, cancellationToken);

            if (!viewModel.Success)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Tạo đánh giá thất bại.",
                    error_items = viewModel.ErrorItems
                });
            }

            var createdReview = (ReviewDto)viewModel.Data;
            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, new
            {
                success = true,
                data = createdReview
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewUpdateRequest updateRequest, CancellationToken cancellationToken)
        {
            if (id != updateRequest.Id)
            {
                return BadRequest(new { success = false, message = "Mã đánh giá không khớp." });
            }

            var validationResult = await _updateValidator.ValidateAsync(updateRequest, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Thông tin đánh giá không hợp lệ.",
                    error_items = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            var viewModel = new ReviewUpdateViewModel(_reviewReadWriteRepository, _localizationService, _mapper);
            await viewModel.HandleAsync(updateRequest, cancellationToken);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy thông tin đánh giá hoặc thông tin không hợp lệ.",
                    error_items = viewModel.ErrorItems
                });
            }

            return Ok(new { success = true, message = "Cập nhật thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id, CancellationToken cancellationToken)
        {
            var viewModel = new ReviewDeleteViewModel(_reviewReadWriteRepository, _localizationService);
            await viewModel.HandleAsync(id, cancellationToken);

            if (!viewModel.Success)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Không tìm thấy đánh giá.",
                    error_items = viewModel.ErrorItems
                });
            }

            return NoContent();
        }
    }
}
