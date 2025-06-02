using Microsoft.EntityFrameworkCore;
using Serilog;
using UnistreamTaskWebAPI.Data;
using UnistreamTaskWebAPI.Models;
using UnistreamTaskWebAPI.Models.Exceptions;

namespace UnistreamTaskWebAPI.Services
{
    // Логи и ошибки на русском, что бы их было лучше видно в логах.
    public class TransactionService : Interfaces.ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private const int MaxTransactions = 100;

        public TransactionService(AppDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TransactionResponse> CreateTransactionAsync(Transaction inputTransaction)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["TransactionId"] = inputTransaction.Id,
                ["Amount"] = inputTransaction.Amount
            }))
            {
                try
                {
                    Log.Debug("Проверка транзакции");
                    ValidateTransaction(inputTransaction);

                    Log.Debug("Проверка существующей транзакции");
                    var existingTransaction = await _context.Transactions.FindAsync(inputTransaction.Id);
                    if (existingTransaction != null)
                    {
                        Log.Information("Данная танзакция уже существует");
                        return new TransactionResponse(existingTransaction.InsertDateTime);
                    }

                    Log.Debug("Проверка максимального кол-ва транзакций");
                    if (await _context.Transactions.CountAsync() >= MaxTransactions)
                    {
                        Log.Warning("Достигнут лимит транзакций");
                        throw new ApiException($"Ошибка: Максимальное кол-во транзакций ({MaxTransactions}) было достигнуто", 429);
                    }

                    Log.Debug("Создание транзакции");
                    var transaction = new UnistreamTaskWebAPI.Models.Entities.Transaction
                    {
                        Id = inputTransaction.Id,
                        TransactionDate = inputTransaction.TransactionDate,
                        Amount = inputTransaction.Amount,
                        InsertDateTime = DateTime.UtcNow
                    };

                    await _context.Transactions.AddAsync(transaction);
                    await _context.SaveChangesAsync();

                    Log.Information("Транзакция успешно создана");
                    return new TransactionResponse(transaction.InsertDateTime);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Ошибка: Ошибка обработки транзакции");
                    throw;
                }
            }
        }

        public async Task<Transaction?> GetTransactionAsync(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
                    
            if (transaction == null)
            {
                _logger.LogWarning("Транзакция с id: {Id} не найдена", id);
                return null;
            }

            return new Transaction(
                transaction.Id,
                transaction.TransactionDate,
                transaction.Amount,
                transaction.InsertDateTime);
        }

        private void ValidateTransaction(Transaction inputTransaction)
        {
            if (inputTransaction.Amount <= 0)
    {
        throw new ApiException("Ошибка: Сумма должна быть положительной", StatusCodes.Status400BadRequest);
    }

    if (inputTransaction.TransactionDate > DateTime.UtcNow)
    {
        throw new ApiException("Ошибка: Дата транзакции не может быть больше текущей", StatusCodes.Status400BadRequest);
    }
        }
    }
}
