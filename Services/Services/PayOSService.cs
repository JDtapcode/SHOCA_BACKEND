using System;
using System.Threading.Tasks;
using Repositories.Common;
using Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Repositories.Enums;
using Services.Interfaces;
using Services.Models.PayOSModels;

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
        //var package = await _unitOfWork.ProPackageRepository.GetByIdAsync(packageId);
        var package = await _unitOfWork.ProPackageRepository.GetAsync(packageId);
        if (package == null)
            throw new Exception("Package not found");

        var paymentUrl = $"https://payos.vn/pay?clientId={_clientId}&amount={package.Price}&callback={_webhookUrl}";

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

        return paymentUrl;
    }

    public async Task<PaymentResponseModel> HandlePaymentReturnAsync(string transactionId)
    {
        var transaction = await _unitOfWork.TransactionRepository.GetAsync(Guid.Parse(transactionId));
        if (transaction == null)
            throw new Exception("Transaction not found");

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