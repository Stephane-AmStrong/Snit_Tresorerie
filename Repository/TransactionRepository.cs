using Contracts;
using Entities;
using Entities.Extensions;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        private ISortHelper<Transaction> _sortHelper;

        public TransactionRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Transaction> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Transaction>> GetPagedListAsync(TransactionParameters transactionParameters)
        {
            var transactions = Enumerable.Empty<Transaction>().AsQueryable();

            ApplyFilters(ref transactions, transactionParameters);

            PerformSearch(ref transactions, transactionParameters.SearchTerm);

            var sortedTransactions = _sortHelper.ApplySort(transactions, transactionParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Transaction>.ToPagedList
                (
                    sortedTransactions,
                    transactionParameters.PageNumber,
                    transactionParameters.PageSize)
                );
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(transaction => transaction.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Transaction> GetDetailsAsync(Guid id)
        {
            return await BaseFindByCondition(transaction => transaction.Id.Equals(id))
                .Include(x => x.Actor)
                .Include(x => x.AppUser)
                .Include(x => x.Site)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task<bool> ExistAsync(Transaction transaction)
        {
            return await BaseFindByCondition(x => x.Name == transaction.Name &&
                x.Reference == transaction.Reference &&
                x.Type == transaction.Type && 
                x.AmountBeforeTax == transaction.AmountBeforeTax && 
                x.VAT == transaction.VAT && 
                x.ATI == transaction.ATI && 
                x.PaymentTypeId == transaction.PaymentTypeId)
                .AnyAsync();
        }

        public async Task CreateAsync(Transaction transaction)
        {
            await BaseCreateAsync(transaction);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            await BaseUpdateAsync(transaction);
        }

        public async Task<int> CountsAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            await BaseDeleteAsync(transaction);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Transaction> transactions, TransactionParameters transactionQueryParameters)
        {
            transactions = BaseFindAll();

            /*
            if (transactionQueryParameters.AvailableOnly)
            {
                transactions = transactions.Where(x => x.EndsOn > DateTime.UtcNow.AddHours(1));
            }

            if (transactionQueryParameters.FromDate != null)
            {
                transactions = transactions.Where(x => x.StartsOn >= transactionQueryParameters.FromDate);
            }

            */

        }

        private void PerformSearch(ref IQueryable<Transaction> transactions, string searchTerm)
        {
            if (!transactions.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            transactions = transactions.Where(x => x.Reference.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}
