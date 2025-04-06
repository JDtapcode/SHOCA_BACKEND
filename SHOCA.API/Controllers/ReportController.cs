using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.ReportModels;

namespace SHOCA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromForm] ReportCreateModel model)
        {
            var result = await _reportService.CreateReportAsync(model);
            if (!result.Status)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReportsByUser(Guid userId)
        {
            var result = await _reportService.GetReportsByUserAsync(userId);
            if (!result.Status)
                return NotFound(result.Message);

            return Ok(result);
        }

        //[HttpGet("artwork/{artworkId}")]
        //public async Task<IActionResult> GetReportsByArtwork(Guid artworkId)
        //{
        //    var result = await _reportService.GetReportsByArtworkAsync(artworkId);
        //    if (!result.Status)
        //        return NotFound(result.Message);

        //    return Ok(result);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAllReports([FromQuery] bool includeDeleted = false)
        {
            var result = await _reportService.GetAllReportsAsync(includeDeleted);
            if (!result.Status)
                return NotFound(result.Message);

            return Ok(result);
        }
        [HttpPut("{reportId}/status")]
        public async Task<IActionResult> UpdateReportStatus(Guid reportId, [FromBody] ReportStatusUpdateModel model)
        {
            var result = await _reportService.UpdateReportStatusAsync(reportId, model);
            if (!result.Status)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
