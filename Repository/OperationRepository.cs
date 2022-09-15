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
    public class OperationRepository : RepositoryBase<Operation>, IOperationRepository
    {
        private ISortHelper<Operation> _sortHelper;

        public OperationRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Operation> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Operation>> GetPagedListAsync(OperationParameters operationParameters)
        {
            var operations = Enumerable.Empty<Operation>().AsQueryable();

            ApplyFilters(ref operations, operationParameters);

            PerformSearch(ref operations, operationParameters.SearchTerm);

            var sortedOperations = _sortHelper.ApplySort(operations, operationParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Operation>.ToPagedList
                (
                    sortedOperations,
                    operationParameters.PageNumber,
                    operationParameters.PageSize)
                );
        }

        public async Task<Operation> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(operation => operation.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Operation> GetDetailsAsync(Guid id)
        {
            return await BaseFindByCondition(operation => operation.Id.Equals(id))
                .Include(x => x.Intervenor)
                .Include(x => x.AppUser)
                .Include(x => x.Site)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task<bool> ExistAsync(Operation operation)
        {
            return await BaseFindByCondition(x => x.Name == operation.Name &&
                x.Reference == operation.Reference &&
                x.Type == operation.Type && 
                x.AmountBeforeTax == operation.AmountBeforeTax && 
                x.VAT == operation.VAT && 
                x.ATI == operation.ATI && 
                x.PaymentOptionId == operation.PaymentOptionId)
                .AnyAsync();
        }

        public async Task CreateAsync(Operation operation)
        {
            await BaseCreateAsync(operation);
        }

        public async Task UpdateAsync(Operation operation)
        {
            await BaseUpdateAsync(operation);
        }

        public async Task<int> CountsAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task DeleteAsync(Operation operation)
        {
            await BaseDeleteAsync(operation);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Operation> operations, OperationParameters operationQueryParameters)
        {
            operations = BaseFindAll();

            /*
            if (operationQueryParameters.AvailableOnly)
            {
                operations = operations.Where(x => x.EndsOn > DateTime.UtcNow.AddHours(1));
            }

            if (operationQueryParameters.FromDate != null)
            {
                operations = operations.Where(x => x.StartsOn >= operationQueryParameters.FromDate);
            }

            */

        }

        private void PerformSearch(ref IQueryable<Operation> operations, string searchTerm)
        {
            if (!operations.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            operations = operations.Where(x => x.Reference.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}
