
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRoleRepository
    {
        Task<PagedList<IdentityRole>> GetPagedListAsync(RoleParameters roleParameters);

        Task<IdentityRole> GetByIdAsync(string id);
        Task<IdentityRole> GetByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(IdentityRole role);
        Task<bool> AddClaimsSucceededAsync(IdentityRole role, IEnumerable<Claim> claims);
        Task<bool> RemoveClaimsSucceededAsync(IdentityRole role, IList<Claim> claims);
        Task<bool> ExistAsync(IdentityRole role);

        Task CreateAsync(IdentityRole role);
        Task UpdateAsync(IdentityRole role);
        Task DeleteAsync(IdentityRole role);
    }
}
