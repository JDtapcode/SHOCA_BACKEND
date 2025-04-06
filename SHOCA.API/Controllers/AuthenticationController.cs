using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.AccountModels;
using Services.Models.CommonModels;
using Services.Models.TokenModels;
using System.ComponentModel.DataAnnotations;

namespace SHOCA.API.Controllers
{
    [Route("api/v1/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthenticationController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] AccountRegisterModel accountRegisterModel)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            // Lấy danh sách lỗi validation
        //            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
        //                                   .ToDictionary(
        //                                       kvp => kvp.Key,
        //                                       kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        //                                   );

        //            return BadRequest(new
        //            {
        //                Status = false,
        //                Message = "Validation failed",
        //                Errors = errors
        //            });
        //        }

        //        var result = await _accountService.Register(accountRegisterModel);
        //        if (result.Status)
        //        {
        //            return Ok(result);
        //        }

        //        return BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Status = false, Message = ex.Message });
        //    }
        //}
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel? accountRegisterModel)
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

                var errors = new List<string>();

                if (string.IsNullOrWhiteSpace(accountRegisterModel.FirstName))
                    errors.Add("Họ không được để trống.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.LastName))
                    errors.Add("Tên không được để trống.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.PhoneNumber))
                    errors.Add("Số điện thoại không được để trống.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.Email))
                    errors.Add("Email không được để trống.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.Password))
                    errors.Add("Mật khẩu không được để trống.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.ConfirmPassword))
                    errors.Add("Xác nhận mật khẩu không được để trống.");

                if (accountRegisterModel.Password != accountRegisterModel.ConfirmPassword)
                    errors.Add("Mật khẩu xác nhận không khớp.");

                if (string.IsNullOrWhiteSpace(accountRegisterModel.Address))
                    errors.Add("Địa chỉ không được để trống.");

                if (accountRegisterModel.DateOfBirth == default || accountRegisterModel.DateOfBirth.Year < 1900||accountRegisterModel.DateOfBirth== null)
                    errors.Add("Ngày sinh không hợp lệ. Vui lòng nhập đúng định dạng (yyyy-MM-dd).");

                if (errors.Any())
                {
                    return BadRequest(new
                    {
                        Status = false,
                        Message = "Vui lòng kiểm tra lại thông tin.",
                        Errors = errors
                    });
                }

                var result = await _accountService.Register(accountRegisterModel);
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





        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginModel accountLoginModel)
        {
            try
            {
                var result = await _accountService.Login(accountLoginModel);
                if (result.Status)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel)
        {
            try
            {
                var result = await _accountService.RefreshToken(refreshTokenModel);
                if (result.Status)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpGet("email/verify")]
        //public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string verificationCode)
        //{
        //    try
        //    {
        //        var result = await _accountService.VerifyEmail(email, verificationCode);
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
        [HttpGet("email/verify")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string verificationCode)
        {
            try
            {
                var result = await _accountService.VerifyEmail(email, verificationCode);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "An error occurred",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("email/resend-verification")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] EmailModel emailModel)
        {
            try
            {
                var result = await _accountService.ResendVerificationEmail(emailModel);
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

        [HttpPost("password/change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            [FromBody] AccountChangePasswordModel accountChangePasswordModel)
        {
            try
            {
                var result = await _accountService.ChangePassword(accountChangePasswordModel);
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

        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailModel emailModel)
        {
            try
            {
                var result = await _accountService.ForgotPassword(emailModel);
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

        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword([FromBody] AccountResetPasswordModel accountResetPasswordModel)
        {
            try
            {
                var result = await _accountService.ResetPassword(accountResetPasswordModel);
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

        /* [HttpGet("login/google")]
         public async Task<IActionResult> LoginGoogle([FromQuery] string code, [FromQuery] bool httpOnly = true)
         {
             try
             {
                 var result = await _accountService.LoginGoogle(code);
                 if (result.Status)
                 {
                     if (httpOnly)
                     {
                         HttpContext.Response.Cookies.Append("refreshToken", result.Data!.RefreshToken!,
                             new CookieOptions
                             {
                                 Expires = DateTimeOffset.Now.AddDays(7),
                                 HttpOnly = true,
                                 IsEssential = true,
                                 Secure = true,
                                 SameSite = SameSiteMode.None
                             });

                         result.Data.RefreshToken = null;
                     }

                     return Ok(result);
                 }

                 return BadRequest(result);
             }
             catch (Exception ex)
             {
                 return BadRequest(ex);
             }
         }*/
    }
}
