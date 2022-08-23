
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

        Task CreateAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task<int> CountAsync();

    }
}
