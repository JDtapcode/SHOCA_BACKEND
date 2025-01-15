using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.FreelancerServiceModels;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/freelancerservices")]
    public class FreelancerServiceController : Controller
    {
        public readonly IFreelancerServiceService _service;
        public FreelancerServiceController(IFreelancerServiceService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFreelancerServices([FromQuery] FreelancerServiceFilterModel filterModel)
        {
            try
            {
                var result = await _service.GetAllFreelancerServicesAsync(filterModel);
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
        public async Task<IActionResult> CreateNewFreelancerService([FromBody] FreelancerServiceCreateModel freelancerServiceCreateModel)
        {
            var result = await _service.CreateFreelancerServiceAsync(freelancerServiceCreateModel);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFreelancerService(Guid id, [FromBody] FreelancerServiceUpdateModel model)
        {
            var result = await _service.UpdateFreelancerServiceAsync(id, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreelancerService(Guid id)
        {
            var result = await _service.DeleteFreelancerServiceAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreelancerServiceById(Guid id)
        {
            var result = await _service.GetFreelancerServiceByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreFreelancerService(Guid id)
        {
            try
            {
                var result = await _service.RestoreFreelancerService(id);
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
