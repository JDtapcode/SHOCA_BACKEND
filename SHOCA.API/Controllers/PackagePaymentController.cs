using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.PayOSModels;
using System;
using System.Threading.Tasks;

namespace SHOCA.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PackagePaymentController : ControllerBase
    {
        private readonly IPayOSService _payOSService;

        public PackagePaymentController(IPayOSService payOSService)
        {
            _payOSService = payOSService;
        }

        /// <summary>
        /// Tạo URL thanh toán cho gói Pro
        /// </summary>
        [HttpPost("create-payment-url")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentRequestModel request)
        {
            if (request == null || request.PackageId == Guid.Empty || request.AccountId == Guid.Empty)
            {
                return BadRequest("Invalid request data.");
            }

            var paymentUrl = await _payOSService.CreatePaymentUrlAsync(request.PackageId, request.AccountId);
            return Ok(new { PaymentUrl = paymentUrl });
        }

        /// <summary>
        /// Xử lý phản hồi thanh toán từ PayOS
        /// </summary>
        [HttpGet("handle-payment-return")]
        public async Task<IActionResult> HandlePaymentReturn([FromQuery] string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                return BadRequest("Transaction ID is required.");
            }

            var response = await _payOSService.HandlePaymentReturnAsync(transactionId);
            return Ok(response);
        }
    }
}
