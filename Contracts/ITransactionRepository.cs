
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITransactionRepository
    {
        Task<PagedList<Transaction>> GetPagedListAsync(TransactionParameters transactionParameters);

        Task<Transaction> GetByIdAsync(Guid id);
        Task<Transaction> GetDetailsAsync(Guid id);
        Task<bool> ExistAsync(Transaction transaction);

        Task BaseCreateAsync(Transaction transaction);
        Task BaseUpdateAsync(Transaction transaction);
        Task BaseDeleteAsync(Transaction transaction);
        Task<int> CountAsync();

    }
}
