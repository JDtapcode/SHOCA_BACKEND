using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.RatingModels;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/ratings")]
    public class RatingsController : Controller
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRatingsByArtwork([FromQuery] RatingFilterModel model)
        {
            var result = await _ratingService.GetRatingsByArtworkAsync(model);
            var metadata = new
            {
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRating([FromBody] RatingCreateModel ratingCreateModel)
        {
            var result = await _ratingService.CreateRating(ratingCreateModel);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("comments")]
        public async Task<IActionResult> AddComment([FromBody] RatingCommentCreateModel ratingCommentCreateModel)
        {
            var result = await _ratingService.AddComment(ratingCommentCreateModel);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRatingById(Guid id)
        {
            var result = await _ratingService.GetRatingById(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDeleteRating(Guid id)
        {
            var result = await _ratingService.HardDeleteRatingAsync(id);
            if (!result.Status)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
