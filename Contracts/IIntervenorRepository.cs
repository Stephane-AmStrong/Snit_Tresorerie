
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IIntervenorRepository
    {
        Task<PagedList<Intervenor>> GetPagedListAsync(IntervenorParameters intervenorParameters);

        Task<Intervenor> GetByIdAsync(Guid id);
        Task<Intervenor> GetDetailsAsync(Guid id);
        Task<bool> ExistAsync(Intervenor intervenor);
        Task<int> CountAsync();

        Task CreateAsync(Intervenor intervenor);
        Task UpdateAsync(Intervenor intervenor);
        Task DeleteAsync(Intervenor intervenor);

    }
}
