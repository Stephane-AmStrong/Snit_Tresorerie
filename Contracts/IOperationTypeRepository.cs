
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOperationTypeRepository
    {
        Task<PagedList<OperationType>> GetPagedListAsync(OperationTypeParameters operationTypeParameters);
        Task<OperationType> GetByIdAsync(Guid id);
        Task<OperationType> GetDetailsAsync(Guid id);
        Task<int> CountAsync();
        Task<bool> ExistAsync(OperationType operationType);
        Task CreateAsync(OperationType operationType);
        Task UpdateAsync(OperationType operationType);
        Task DeleteAsync(OperationType operationType);
    }
}
