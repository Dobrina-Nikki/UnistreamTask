using UnistreamTaskWebAPI.Models;

namespace UnistreamTaskWebAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> CreateTransactionAsync(Transaction inputTransaction);
        Task<Transaction?> GetTransactionAsync(Guid id);
    }
}
