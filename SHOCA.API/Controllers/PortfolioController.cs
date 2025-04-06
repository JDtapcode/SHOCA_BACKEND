using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.PortfolioModels;
using System;
using System.Threading.Tasks;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/portfolios")]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _service;

        public PortfolioController(IPortfolioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPortfolios([FromQuery] PortfolioFilterModel filterModel)
        {
            try
            {
                var result = await _service.GetAllPortfolioAsync(filterModel);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPortfolioById(Guid id)
        {
            var result = await _service.GetPortfolioByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePortfolio([FromBody] PortfolioCreateModel model)
        {
            var result = await _service.CreatePortfolioAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePortfolio(Guid id, [FromBody] PortfolioUpdateModel model)
        {
            var result = await _service.UpdatePortfolioAsync(id, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortfolio(Guid id)
        {
            var result = await _service.DeletePortfolioAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestorePortfolio(Guid id)
        {
            try
            {
                var result = await _service.RestorePortfolio(id);
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
