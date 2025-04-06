using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.ProPackageModels;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/pro-packages")]
    public class ProPackageController : Controller
    {
        private readonly IProPackageService _service;

        public ProPackageController(IProPackageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProPackages([FromQuery] ProPackageFilterModel filterModel)
        {
            try
            {
                var result = await _service.GetAllProPackageAsync(filterModel);
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
        public async Task<IActionResult> CreateNewProPackage([FromBody] ProPackageCreateModel model)
        {
            var result = await _service.CreateProPackageAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProPackage(Guid id, [FromBody] ProPackageUpdateModel model)
        {
            var result = await _service.UpdateProPackageAsync(id, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProPackage(Guid id)
        {
            var result = await _service.DeleteProPackageAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProPackageById(Guid id)
        {
            var result = await _service.GetProPackageByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreProPackage(Guid id)
        {
            try
            {
                var result = await _service.RestoreProPackage(id);
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
