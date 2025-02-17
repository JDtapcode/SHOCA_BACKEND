using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.AccountModels;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //[HttpPost]
        //public async Task<IActionResult> AddAccounts([FromBody] AccountRegisterModel accountRegisterModels)
        //{
        //    try
        //    {
        //        var result = await _accountService.AddAccounts(accountRegisterModels);
        //        if (result.Status)
        //        {
        //            return Ok(result);
        //        }

        //        return BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> AddAccounts([FromBody] AccountRegisterModel? accountRegisterModel)
        {
            try
            {
                if (accountRegisterModel == null)
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "Dữ liệu gửi lên không hợp lệ. Vui lòng kiểm tra lại."
                    });
                }

                var result = await _accountService.AddAccounts(accountRegisterModel);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = false, Message = "Đã xảy ra lỗi, vui lòng thử lại sau." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            try
            {
                var result = await _accountService.GetAccount(id);
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
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountFilterModel accountFilterModel)
        {
            try
            {
                var result = await _accountService.GetAllAccounts(accountFilterModel);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] AccountUpdateModel accountUpdateModel)
        {
            try
            {
                var result = await _accountService.UpdateAccount(id, accountUpdateModel);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var result = await _accountService.DeleteAccount(id);
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
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreAccount(Guid id)
        {
            try
            {
                var result = await _accountService.RestoreAccount(id);
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
