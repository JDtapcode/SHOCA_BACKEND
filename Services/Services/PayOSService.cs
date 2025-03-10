using System;
using System.Threading.Tasks;
using Repositories.Common;
using Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Repositories.Enums;
using Services.Interfaces;
using Services.Models.PayOSModels;
using System.Net.Http.Json;

//public class PayOSService : IPayOSService
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly string? _clientId;
//    private readonly string? _apiKey;
//    private readonly string? _checksumKey;
//    private readonly string? _webhookUrl;

//    public PayOSService(IUnitOfWork unitOfWork, IConfiguration configuration)
//    {
//        _unitOfWork = unitOfWork;
//        _clientId = configuration["PayOS:ClientId"];
//        _apiKey = configuration["PayOS:ApiKey"];
//        _checksumKey = configuration["PayOS:ChecksumKey"];
//        _webhookUrl = configuration["PayOS:WebhookUrl"];
//    }

//    public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
//    {
//        var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
//        if (package == null)
//            throw new Exception("Package not found");

//        if (string.IsNullOrEmpty(_webhookUrl))
//            throw new Exception("Webhook URL is not configured. Please check appsettings.json.");

//        // Tạo request thanh toán
//        var paymentRequest = new
//        {
//            amount = package.Price,
//            description = $"Thanh toán package {package.Name}",
//            callbackUrl = _webhookUrl,
//            returnUrl = "https://yourfrontend.com/payment-success",
//            orderCode = Guid.NewGuid().ToString(), // Mã đơn hàng duy nhất
//            buyerName = "Nguyen Van A",
//            buyerEmail = "email@example.com",
//            buyerPhone = "0123456789"
//        };

//        var httpClient = new HttpClient();
//        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}"); // API Key từ PayOS

//        var response = await httpClient.PostAsJsonAsync("https://api.payos.vn/v1/payment", paymentRequest);
//        if (!response.IsSuccessStatusCode)
//        {
//            var error = await response.Content.ReadAsStringAsync();
//            throw new Exception($"Failed to create payment: {error}");
//        }

//        var paymentResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
//        if (paymentResponse == null || !paymentResponse.ContainsKey("checkoutUrl"))
//            throw new Exception("Invalid response from PayOS");

//        return paymentResponse["checkoutUrl"]; // Trả về link thanh toán chính xác
//    }

//    //public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
//    //{
//    //    var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
//    //    if (package == null)
//    //        throw new Exception("Package not found");

//    //    if (string.IsNullOrEmpty(_webhookUrl))
//    //        throw new Exception("Webhook URL is not configured. Please check appsettings.json.");

//    //    var paymentUrl = $"https://payos.vn/pay?clientId={_clientId}&amount={package.Price}&callback={_webhookUrl}";

//    //    var transaction = new Transaction
//    //    {
//    //        Id = Guid.NewGuid(),
//    //        ProPackageId = packageId,
//    //        UserId = accountId,
//    //        MoneyAmount = package.Price,
//    //        TransactionDate = DateTime.UtcNow,
//    //        PaymentStatus = PaymentStatus.Pending
//    //    };

//    //    await _unitOfWork.TransactionRepository.AddAsync(transaction);
//    //    await _unitOfWork.SaveChangeAsync();

//    //    Console.WriteLine($"ClientId: {_clientId}, Webhook: {_webhookUrl}, PaymentUrl: {paymentUrl}");
//    //    return paymentUrl;
//    //}


//    public async Task<PaymentResponseModel> HandlePaymentReturnAsync(string transactionId)
//    {
//        var transaction = await _unitOfWork.TransactionRepository.GetAsync(Guid.Parse(transactionId));
//        if (transaction == null)
//            throw new Exception("Transaction not found");

//        transaction.PaymentStatus = PaymentStatus.Complete;
//        await _unitOfWork.SaveChangeAsync();

//        var accountProPackage = new AccountProPackage
//        {
//            Id = Guid.NewGuid(),
//            AccountId = transaction.UserId.Value,
//            ProPackageId = transaction.ProPackageId.Value,
//            StartDate = DateTime.UtcNow,
//            EndDate = DateTime.UtcNow.AddMonths(1),
//            PackageStatus = PackageStatus.OnGoing
//        };

//        await _unitOfWork.AccountProPackageRepository.AddAsync(accountProPackage);
//        await _unitOfWork.SaveChangeAsync();

//        return new PaymentResponseModel
//        {
//            OrderId = transactionId,
//            PaymentStatus = "Success",
//            Amount = transaction.MoneyAmount
//        };

//    }
//}
public class PayOSService : IPayOSService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly string? _clientId;
    private readonly string? _apiKey;
    private readonly string? _checksumKey;
    private readonly string? _webhookUrl;

    public PayOSService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _clientId = configuration["PayOS:ClientId"];
        _apiKey = configuration["PayOS:ApiKey"];
        _checksumKey = configuration["PayOS:ChecksumKey"];
        _webhookUrl = configuration["PayOS:WebhookUrl"];
    }

    public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
    {
        var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
        if (package == null)
            throw new Exception("Package not found");

        if (string.IsNullOrEmpty(_webhookUrl))
            throw new Exception("Webhook URL is not configured. Please check appsettings.json.");

        // Tạo giao dịch trước
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            ProPackageId = packageId,
            UserId = accountId,
            MoneyAmount = package.Price,
            TransactionDate = DateTime.UtcNow,
            PaymentStatus = PaymentStatus.Pending
        };

        await _unitOfWork.TransactionRepository.AddAsync(transaction);
        await _unitOfWork.SaveChangeAsync();

        // Tạo request thanh toán
        var paymentRequest = new
        {
            amount = package.Price,
            description = $"Thanh toán package {package.Name}",
            callbackUrl = _webhookUrl,
            returnUrl = "https://yourfrontend.com/payment-success",
            orderCode = transaction.Id.ToString(), 
            buyerName = "Nguyen Van A",
            buyerEmail = "email@example.com",
            buyerPhone = "0123456789"
        };

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var response = await httpClient.PostAsJsonAsync("https://api.payos.vn/v1/payment", paymentRequest);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create payment: {error}");
        }

        var paymentResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (paymentResponse == null || !paymentResponse.ContainsKey("checkoutUrl"))
            throw new Exception("Invalid response from PayOS");

        return paymentResponse["checkoutUrl"];
    }

    public async Task<PaymentResponseModel> HandlePaymentReturnAsync(string transactionId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetAsync(Guid.Parse(transactionId));
        if (transaction == null)
            throw new Exception("Transaction not found");

        if (transaction.PaymentStatus == PaymentStatus.Complete)
            throw new Exception("Transaction already completed");

        transaction.PaymentStatus = PaymentStatus.Complete;
        await _unitOfWork.SaveChangeAsync();

        var accountProPackage = new AccountProPackage
        {
            Id = Guid.NewGuid(),
            AccountId = transaction.UserId.Value,
            ProPackageId = transaction.ProPackageId.Value,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1),
            PackageStatus = PackageStatus.OnGoing
        };

        await _unitOfWork.AccountProPackageRepository.AddAsync(accountProPackage);
        await _unitOfWork.SaveChangeAsync();

        return new PaymentResponseModel
        {
            OrderId = transactionId,
            PaymentStatus = "Success",
            Amount = transaction.MoneyAmount
        };
    }
}
