
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISiteRepository
    {
        Task<PagedList<Site>> GetPagedListAsync(SiteParameters siteParameters);
        Task<Site> GetByIdAsync(Guid id);
        Task<Site> GetDetailsAsync(Guid id);
        Task<int> CountAsync();
        Task<bool> ExistAsync(Site site);
        Task CreateAsync(Site site);
        Task UpdateAsync(Site site);
        Task DeleteAsync(Site site);
    }
}
