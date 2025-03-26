using System;
using System.Threading.Tasks;
using Repositories.Common;
using Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Repositories.Enums;
using Services.Interfaces;
using Services.Models.PayOSModels;
using Net.payOS.Types; 
using System.Collections.Generic;
using System.Text.Json;
using Net.payOS;
using Transaction = Repositories.Entities.Transaction;

namespace Services.Services
{
    public class PayOSService : IPayOSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        private readonly string _returnUrl; 
        private static readonly Random _random = new Random(); 

        public PayOSService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _clientId = configuration["PayOS:ClientId"] ?? throw new ArgumentNullException("PayOS:ClientId is not configured");
            _apiKey = configuration["PayOS:ApiKey"] ?? throw new ArgumentNullException("PayOS:ApiKey is not configured");
            _checksumKey = configuration["PayOS:ChecksumKey"] ?? throw new ArgumentNullException("PayOS:ChecksumKey is not configured");
            _returnUrl = configuration["PayOS:ReturnUrl"] ?? "http://localhost:5000/api/payment/return";

        }

        public async Task<string> CreatePaymentUrlAsync(Guid packageId, Guid accountId)
        {
            try
            {
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

                long orderCode = GenerateOrderCode();
                string orderCodeString = orderCode.ToString();

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    ProPackageId = packageId,
                    UserId = accountId,
                    MoneyAmount = package.Price,
                    TransactionDate = DateTime.UtcNow,
                    PaymentStatus = PaymentStatus.Pending,
                    OrderCode = orderCodeString
                };

                if (_unitOfWork.TransactionRepository == null)
                    throw new Exception("TransactionRepository không được khởi tạo");

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

                var item = new ItemData("Gói Premium", 1, (int)Math.Round(package.Price, 0));
                var items = new List<ItemData> { item };

                string description = $"Thanh toán gói {package.Name}";
                if (description.Length > 25)
                {
                    description = description.Substring(0, 25);
                    Console.WriteLine($"Description truncated to 25 characters: {description}");
                }
                else
                {
                    Console.WriteLine($"Description: {description}");
                }

                //var paymentData = new Net.payOS.Types.PaymentData(
                //    orderCode: orderCode,
                //    amount: (int)Math.Round(package.Price, 0),
                //    description: description,
                //    items: items,
                //    cancelUrl: "http://localhost:5000/api/payment/cancel",
                //    returnUrl: _returnUrl,
                //    buyerName: user.FirstName + " " + (user.LastName ?? ""),
                //    buyerEmail: user.Email ?? "email@gmail.com",
                //    buyerPhone: user.PhoneNumber ?? "0123456789"
                //);
                var paymentData = new Net.payOS.Types.PaymentData(
    orderCode: orderCode,
    amount: (int)Math.Round(package.Price, 0),
    description: description,
    items: items,
    cancelUrl: "http://localhost:5173/payment-fail",  
    returnUrl: "http://localhost:5173/payment-success",  
    buyerName: user.FirstName + " " + (user.LastName ?? ""),
    buyerEmail: user.Email ?? "email@gmail.com",
    buyerPhone: user.PhoneNumber ?? "0123456789"
);


                var payOS = new PayOS(_clientId, _apiKey, _checksumKey);

                Console.WriteLine($"Creating payment link for orderCode: {orderCode}");
                var createPaymentResult = await payOS.createPaymentLink(paymentData);

                if (createPaymentResult == null || string.IsNullOrEmpty(createPaymentResult.checkoutUrl))
                    throw new Exception("Không tạo được link thanh toán từ PayOS");

                Console.WriteLine($"Payment URL: {createPaymentResult.checkoutUrl}");
                return createPaymentResult.checkoutUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong CreatePaymentUrlAsync: {ex}");
                throw;
            }
        }

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

        private long GenerateOrderCode()
        {
            const long minOrderCode = 1;
            const long maxOrderCode = 9999999999;

            long orderCode;
            lock (_random) 
            {
                int randomInt = _random.Next();
                orderCode = minOrderCode + (long)((double)randomInt / int.MaxValue * (maxOrderCode - minOrderCode));
            }

            if (orderCode < minOrderCode || orderCode > maxOrderCode)
            {
                return GenerateOrderCode(); 
            }

            return orderCode;
        }
    }
}