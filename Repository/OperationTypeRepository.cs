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
    public class OperationTypeRepository : RepositoryBase<OperationType>, IOperationTypeRepository
    {
        private ISortHelper<OperationType> _sortHelper;

        public OperationTypeRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<OperationType> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<OperationType>> GetPagedListAsync(OperationTypeParameters operationTypeParameters)
        {
            var operationTypes = Enumerable.Empty<OperationType>().AsQueryable();

            ApplyFilters(ref operationTypes, operationTypeParameters);

            var sortedOperationTypes = _sortHelper.ApplySort(operationTypes, operationTypeParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<OperationType>.ToPagedList
                (
                    sortedOperationTypes,
                    operationTypeParameters.PageNumber,
                    operationTypeParameters.PageSize)
                );
        }

        public async Task<OperationType> GetByIdAsync(Guid Id)
        {
            return await BaseFindByCondition(operationType => operationType.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<OperationType> GetDetailsAsync(Guid Id)
        {
            return await BaseFindByCondition(operationType => operationType.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(OperationType operationType)
        {
            return await BaseFindByCondition(x => x.Name == operationType.Name && x.Description == operationType.Description)
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(OperationType operationType)
        {
            await BaseCreateAsync(operationType);
        }

        public async Task UpdateAsync(OperationType operationType)
        {
            await BaseUpdateAsync(operationType);
        }

        public async Task DeleteAsync(OperationType operationType)
        {
            await BaseDeleteAsync(operationType);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<OperationType> operationTypes, OperationTypeParameters operationTypeParameters)
        {
            operationTypes = BaseFindAll();
            if (!string.IsNullOrWhiteSpace(operationTypeParameters.Named))
            {
                operationTypes = operationTypes.Where(x => x.Name.Equals(operationTypeParameters.Named));
            }

            
        }

        #endregion

    }
}
