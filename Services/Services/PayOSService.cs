﻿using System;
using System.Threading.Tasks;
using Repositories.Common;
using Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Repositories.Enums;
using Services.Interfaces;
using Services.Models.PayOSModels;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
    private readonly string _returnUrl = "http://localhost:5000/api/payment/return"; // URL tạm thời

    public PayOSService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _clientId = configuration["PayOS:ClientId"];
        _apiKey = configuration["PayOS:ApiKey"];
        _checksumKey = configuration["PayOS:ChecksumKey"];
    }
    //public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
    //{
    //    var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
    //    if (package == null)
    //        throw new Exception("Package not found");

    //    if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_checksumKey))
    //        throw new Exception("PayOS configuration is missing. ");

    //    string orderCode = Guid.NewGuid().ToString("N").Substring(0, 12);

    //    var transaction = new Transaction
    //    {
    //        Id = Guid.NewGuid(),
    //        ProPackageId = packageId,
    //        UserId = accountId,
    //        MoneyAmount = package.Price,
    //        TransactionDate = DateTime.UtcNow,
    //        PaymentStatus = PaymentStatus.Pending,
    //        OrderCode = orderCode
    //    };

    //    await _unitOfWork.TransactionRepository.AddAsync(transaction);
    //    await _unitOfWork.SaveChangeAsync();

    //    var paymentData = new
    //    {
    //        orderCode = orderCode,
    //        amount = (int)Math.Round(package.Price, 0),
    //        description = $"Thanh toán package {package.Name}",
    //        cancelUrl = "http://localhost:5000/api/payment/cancel",
    //        returnUrl = _returnUrl,
    //        buyerName = "Nguyen Van A",
    //        buyerEmail = "email@gmail.com",
    //        buyerPhone = "0123456789"
    //    };

    //    string dataToSign = $"{paymentData.orderCode}|{paymentData.amount}|{paymentData.description}";
    //    string checksum = HmacSha256(dataToSign, _checksumKey);

    //    var requestBody = new
    //    {
    //        data = paymentData,
    //        signature = checksum
    //    };

    //    using var httpClient = new HttpClient();
    //    httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
    //    httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

    //    var response = await httpClient.PostAsJsonAsync("https://api.payos.vn/v2/payment-requests", requestBody);
    //    if (!response.IsSuccessStatusCode)
    //    {
    //        var error = await response.Content.ReadAsStringAsync();
    //        throw new Exception($"Failed to create payment: {error}");
    //    }

    //    var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
    //    if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.Data?.CheckoutUrl))
    //        throw new Exception("Invalid response from PayOS");

    //    return paymentResponse.Data.CheckoutUrl;
    //}
    public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
    {
        try
        {
            if (_unitOfWork == null)
                throw new Exception("UnitOfWork không được khởi tạo");

            if (_unitOfWork.ProPackageRepository == null)
                throw new Exception("ProPackageRepository không được khởi tạo");

            var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
            if (package == null)
                throw new Exception("Không tìm thấy gói");

            if (_unitOfWork.AccountRepository == null)
                throw new Exception("AccountRepository không được khởi tạo");
            var user = await _unitOfWork.AccountRepository.GetAsync(accountId);
            if (user == null)
                throw new Exception("Không tìm thấy người dùng");

            if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_checksumKey))
                throw new Exception("Thiếu cấu hình PayOS");

            string orderCode = DateTime.UtcNow.Ticks.ToString();

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                ProPackageId = packageId,
                UserId = accountId,
                MoneyAmount = package.Price,
                TransactionDate = DateTime.UtcNow,
                PaymentStatus = PaymentStatus.Pending,
                OrderCode = orderCode
            };

            if (_unitOfWork.TransactionRepository == null)
                throw new Exception("TransactionRepository không được khởi tạo");

            Console.WriteLine($"Transaction: Id={transaction.Id}, ProPackageId={transaction.ProPackageId}, UserId={transaction.UserId}, MoneyAmount={transaction.MoneyAmount}, OrderCode={transaction.OrderCode}, PaymentStatus={transaction.PaymentStatus}");

            await _unitOfWork.TransactionRepository.AddAsync(transaction);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi lưu giao dịch: {ex}");
                throw new Exception($"Lưu giao dịch thất bại: {ex.Message}", ex);
            }

            var paymentData = new
            {
                orderCode = long.Parse(orderCode),
                amount = (int)Math.Round(package.Price, 0),
                description = $"Thanh toán gói {package.Name}",
                cancelUrl = "http://localhost:5000/api/payment/cancel",
                returnUrl = _returnUrl,
                buyerName = "Nguyen Van A",
                buyerEmail = "email@gmail.com",
                buyerPhone = "0123456789"
            };

            var dataDict = new Dictionary<string, object>
        {
            { "orderCode", paymentData.orderCode },
            { "amount", paymentData.amount },
            { "description", paymentData.description },
            { "cancelUrl", paymentData.cancelUrl },
            { "returnUrl", paymentData.returnUrl },
            { "buyerName", paymentData.buyerName },
            { "buyerEmail", paymentData.buyerEmail },
            { "buyerPhone", paymentData.buyerPhone }
        };
            string dataToSign = string.Join("", dataDict.OrderBy(k => k.Key).Select(k => $"{k.Key}{k.Value}"));
            string checksum = HmacSha256(dataToSign, _checksumKey);

            var requestBody = new
            {
                orderCode = paymentData.orderCode,
                amount = paymentData.amount,
                description = paymentData.description,
                cancelUrl = paymentData.cancelUrl,
                returnUrl = paymentData.returnUrl,
                buyerName = paymentData.buyerName,
                buyerEmail = paymentData.buyerEmail,
                buyerPhone = paymentData.buyerPhone,
                signature = checksum
            };

            Console.WriteLine($"Request to PayOS: {JsonSerializer.Serialize(requestBody)}");

            using var httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true 
            });
            httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
            httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            Console.WriteLine("Attempting to connect to PayOS API...");
            var response = await httpClient.PostAsJsonAsync("https://api.payos.vn/v2/payment-requests", requestBody);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Lỗi API PayOS: {response.StatusCode} - {error}");
            }

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.Data?.CheckoutUrl))
                throw new Exception("Phản hồi không hợp lệ từ PayOS");

            return paymentResponse.Data.CheckoutUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi trong CreatePaymentUrlAsync: {ex}");
            throw;
        }
    }


    // Đảm bảo HmacSha256 được triển khai chính xác
    //private string HmacSha256(string data, string key)
    //{
    //    using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
    //    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
    //    return Convert.ToBase64String(hash); // Điều chỉnh mã hóa nếu PayOS yêu cầu hex
    //}


    //public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
    //{
    //    var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
    //    if (package == null)
    //        throw new Exception("Package not found");

    //    if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_apiKey) ||
    //        string.IsNullOrEmpty(_checksumKey))
    //        throw new Exception("PayOS configuration is missing. Please check appsettings.json");

    //    int orderCode = new Random().Next(100000, 999999);

    //    var transaction = new Transaction
    //    {
    //        Id = Guid.NewGuid(),
    //        ProPackageId = packageId,
    //        UserId = accountId,
    //        MoneyAmount = package.Price,
    //        TransactionDate = DateTime.UtcNow,
    //        PaymentStatus = PaymentStatus.Pending,
    //        OrderCode = orderCode.ToString()
    //    };

    //    await _unitOfWork.TransactionRepository.AddAsync(transaction);
    //    await _unitOfWork.SaveChangeAsync();

    //    var paymentData = new
    //    {
    //        orderCode = orderCode,
    //        amount = (int)package.Price,
    //        description = $"Thanh toán package {package.Name}",
    //        cancelUrl = "http://localhost:5000/api/payment/cancel",
    //        returnUrl = _returnUrl,
    //        buyerName = "Nguyen Van A",
    //        buyerEmail = "nguyenthanhdung@gmail.com",
    //        buyerPhone = "0329853292"
    //    };

    //    using var httpClient = new HttpClient();
    //    httpClient.DefaultRequestHeaders.Add("x-client-id", _clientId);
    //    httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

    //    string dataToSign = $"{paymentData.orderCode}{paymentData.amount}{paymentData.description}";
    //    string checksum = HmacSha256(dataToSign, _checksumKey);

    //    var requestBody = new
    //    {
    //        data = paymentData,
    //        signature = checksum
    //    };

    //    var response = await httpClient.PostAsJsonAsync("https://api.payos.vn/v2/payment-requests", requestBody);
    //    if (!response.IsSuccessStatusCode)
    //    {
    //        var error = await response.Content.ReadAsStringAsync();
    //        throw new Exception($"Failed to create payment: {error}");
    //    }

    //    var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
    //    if (paymentResponse == null || string.IsNullOrEmpty(paymentResponse.Data?.CheckoutUrl))
    //        throw new Exception("Invalid response from PayOS");

    //    return paymentResponse.Data.CheckoutUrl;
    //}

    public async Task<PaymentResponseModel> HandlePaymentReturnAsync(string orderCode, string status)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetByOrderCodeAsync(orderCode);

        if (transaction == null)
            throw new Exception("Transaction not found");

        if (transaction.PaymentStatus == PaymentStatus.Complete)
            throw new Exception("Transaction already completed");

        if (status == "PAID")
        {
            transaction.PaymentStatus = PaymentStatus.Complete;

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
                OrderId = transaction.Id.ToString(),
                PaymentStatus = "Success",
                Amount = transaction.MoneyAmount
            };
        }
        else
        {
            transaction.PaymentStatus = PaymentStatus.Canceled;
            await _unitOfWork.SaveChangeAsync();

            return new PaymentResponseModel
            {
                OrderId = transaction.Id.ToString(),
                PaymentStatus = "Cancelled",
                Amount = transaction.MoneyAmount
            };
        }
    }

    private string HmacSha256(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}
