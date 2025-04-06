using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.JobModels;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/jobs")]
    public class JobController : Controller
    {
        public readonly IJobService _service;
        public JobController(IJobService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs([FromQuery] JobFilterModel filterModel)
        {
            try
            {
                var result = await _service.GetAllJobAsync(filterModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewJob([FromBody] JobCreateModel jobCreateModel)
        {
            var result = await _service.CreateJobAsync(jobCreateModel);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, [FromBody] JobUpdateModel model)
        {
            var result = await _service.UpdateJobAsync(id, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var result = await _service.DeleteJobAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(Guid id)
        {
            var result = await _service.GetJobByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreJob(Guid id)
        {
            try
            {
                var result = await _service.RestoreJob(id);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
