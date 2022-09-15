
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOperationRepository
    {
        Task<PagedList<Operation>> GetPagedListAsync(OperationParameters operationParameters);

        Task<Operation> GetByIdAsync(Guid id);
        Task<Operation> GetDetailsAsync(Guid id);
        Task<bool> ExistAsync(Operation operation);

        Task CreateAsync(Operation operation);
        Task UpdateAsync(Operation operation);
        Task DeleteAsync(Operation operation);
        Task<int> CountAsync();

    }
}
