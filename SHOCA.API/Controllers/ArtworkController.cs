using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.ArtworkModels;
using Services.Services;
using System;
using System.Threading.Tasks;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/artworks")]
    [ApiController]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtworkService _service;

        public ArtworkController(IArtworkService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArtworks([FromQuery] ArtworkFilterModel filterModel)
        {
            try
            {
                var result = await _service.GetAllArtworkAsync(filterModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("images-by-creator/{creatorId}")]
        public async Task<IActionResult> GetArtworkImagesByCreator(Guid creatorId)
        {
            var result = await _service.GetArtworkImagesByCreatorAsync(creatorId);

            if (!result.Status)
                return NotFound(result);

            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtworkById(Guid id)
        {
            var result = await _service.GetArtworkByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewArtwork([FromBody] ArtworkCreateModel model)
        {
            var result = await _service.CreateArtworkAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtwork(Guid id, [FromBody] ArtworkUpdateModel model)
        {
            var result = await _service.UpdateArtworkAsync(id, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtwork(Guid id)
        {
            var result = await _service.DeleteArtworkAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreArtwork(Guid id)
        {
            try
            {
                var result = await _service.RestoreArtwork(id);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
