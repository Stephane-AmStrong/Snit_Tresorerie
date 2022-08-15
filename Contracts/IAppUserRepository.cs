
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAppUserRepository
    {
        Task<PagedList<AppUser>> GetPagedListAsync(AppUserParameters appUserParameters);

        Task<AppUser> GetByIdAsync(string id);
        Task<AppUser> GetDetailsAsync(string id);
        Task<bool> ExistAsync(AppUser appUser);

        Task CreateAsync(AppUser appUser);
        Task UpdateAsync(AppUser appUser);
        Task DeleteAsync(AppUser appUser);
    }
}
