using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.PayOSModels;
using System;
using System.Threading.Tasks;

namespace SHOCA.API.Controllers
{
    //[Route("api/payments")]
    //[ApiController]
    //public class PackagePaymentController : ControllerBase
    //{
    //    private readonly IPayOSService _payOSService;

    //    public PackagePaymentController(IPayOSService payOSService)
    //    {
    //        _payOSService = payOSService;
    //    }

    //    /// <summary>
    //    /// Tạo URL thanh toán cho gói Pro
    //    /// </summary>
    //    [HttpPost("create-payment-url")]
    //    public async Task<IActionResult> CreatePaymentUrl([FromBody] PaymentRequestModel request)
    //    {
    //        if (request == null || request.PackageId == Guid.Empty || request.AccountId == Guid.Empty)
    //        {
    //            return BadRequest("Invalid request data.");
    //        }

    //        var paymentUrl = await _payOSService.CreatePaymentUrlAsync(request.PackageId, request.AccountId);
    //        return Ok(new { PaymentUrl = paymentUrl });
    //    }

    //    /// <summary>
    //    /// Xử lý phản hồi thanh toán từ PayOS
    //    /// </summary>
    //    [HttpGet("handle-payment-return")]
    //    public async Task<IActionResult> HandlePaymentReturn([FromQuery] string transactionId)
    //    {
    //        if (string.IsNullOrEmpty(transactionId))
    //        {
    //            return BadRequest("Transaction ID is required.");
    //        }

    //        var response = await _payOSService.HandlePaymentReturnAsync(transactionId);
    //        return Ok(response);
    //    }
    //    //[HttpPost("webhook")]
    //    //public async Task<IActionResult> PayOSWebhook([FromBody] PayOSWebhookModel webhookData)
    //    //{
    //    //    if (webhookData == null || string.IsNullOrEmpty(webhookData.TransactionId))
    //    //        return BadRequest("Invalid webhook data.");

    //    //    bool isUpdated = await _paymentService.HandlePaymentReturnAsync(webhookData.TransactionId);

    //    //    if (isUpdated)
    //    //        return Ok(new { message = "Transaction updated successfully." });
    //    //    else
    //    //        return NotFound(new { message = "Transaction not found." });
    //    //}

    //}
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPayOSService _payOSService;

        public PaymentController(IPayOSService payOSService)
        {
            _payOSService = payOSService;
        }

        [HttpGet("return")]
        public async Task<IActionResult> PaymentReturn(
        [FromQuery] string orderCode,
        [FromQuery] string status,
        [FromQuery] string paymentLinkId, // Thêm tham số nếu PayOS gửi qua returnUrl
        [FromQuery] string code) // Thêm tham số code để kiểm tra lỗi
        {
            try
            {
                if (string.IsNullOrEmpty(orderCode) || string.IsNullOrEmpty(status))
                    return BadRequest(new { message = "orderCode and status are required" });

                // Log thông tin nhận được từ returnUrl
                Console.WriteLine($"PaymentReturn - orderCode: {orderCode}, status: {status}, paymentLinkId: {paymentLinkId}, code: {code}");

                // Kiểm tra trạng thái từ PayOS
                if (status != "PAID" && status != "CANCELLED")
                    return BadRequest(new { message = "Invalid status value" });

                var result = await _payOSService.HandlePaymentReturnAsync(orderCode, status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Invalid request data" });

                if (request.PackageId == Guid.Empty || request.AccountId == Guid.Empty)
                    return BadRequest(new { message = "PackageId and AccountId are required" });

                // Thêm log để kiểm tra request
                Console.WriteLine($"CreatePayment - PackageId: {request.PackageId}, AccountId: {request.AccountId}");

                var url = await _payOSService.CreatePaymentUrlAsync(request.PackageId, request.AccountId);
                return Ok(new { CheckoutUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }

}
