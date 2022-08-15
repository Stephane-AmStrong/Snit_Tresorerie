using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleRepository : RepositoryBase<IdentityRole>, IRoleRepository
    {
        private ISortHelper<IdentityRole> _sortHelper;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleRepository(
            RepositoryContext repositoryContext,
            ISortHelper<IdentityRole> sortHelper,
            RoleManager<IdentityRole> roleManager
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _roleManager = roleManager;
        }


        public async Task<PagedList<IdentityRole>> GetPagedListAsync(RoleParameters roleParameters)
        {
            var roles = Enumerable.Empty<IdentityRole>().AsQueryable();

            ApplyFilters(ref roles, roleParameters);

            PerformSearch(ref roles, roleParameters.SearchTerm);

            var sortedRoles = _sortHelper.ApplySort(roles, roleParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<IdentityRole>.ToPagedList
                (
                    sortedRoles,
                    roleParameters.PageNumber,
                    roleParameters.PageSize)
                );
        }


        public async Task<IdentityRole> GetByIdAsync(string id)
        {
            return await _roleManager.Roles.Where(role => role.Id.Equals(id))
                .FirstOrDefaultAsync();
        }


        public async Task<IdentityRole> GetByNameAsync(string roleName)
        {
            return await _roleManager.Roles.Where(role => role.Name.Equals(roleName))
                .FirstOrDefaultAsync();
        }


        public async Task<IList<Claim>> GetClaimsAsync(IdentityRole role)
        {
            return await _roleManager.GetClaimsAsync(role);
        }



        public async Task<bool> AddClaimsSucceededAsync(IdentityRole role, IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                var result = await _roleManager.AddClaimAsync(role, claim);
                if (!result.Succeeded) return false;
            }
            return true;
        }



        public async Task<bool> RemoveClaimsSucceededAsync(IdentityRole role, IList<Claim> claims)
        {
            foreach (var claim in claims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded) return false;
            }
            return true;
        }


        public async Task<bool> ExistAsync(IdentityRole role)
        {
            return await BaseFindByCondition(x => x.Name == role.Name)
                .AnyAsync();
        }


        public async Task CreateAsync(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
        }

        public async Task UpdateAsync(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
        }

        public async Task DeleteAsync(IdentityRole role)
        {
            await _roleManager.DeleteAsync(role);
        }


        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<IdentityRole> roles, RoleParameters roleParameters)
        {
            roles = _roleManager.Roles;

            /*
            if (!string.IsNullOrWhiteSpace(roleParameters.ManagedByAppUserId))
            {
                roles = roles.Where(x => x.AppUserId == roleParameters.ManagedByAppUserId);
            }

            if (roleParameters.showValidatedOnesOnly)
            {
                roles = roles.Where(x => x.ValiddatedAt !=null);
            }

            if (roleParameters.MaxBirthday != null)
            {
                roles = roles.Where(x => x.Birthday < roleParameters.MaxBirthday);
            }

            if (roleParameters.MinCreateAt != null)
            {
                roles = roles.Where(x => x.CreateAt >= roleParameters.MinCreateAt);
            }

            if (roleParameters.MaxCreateAt != null)
            {
                roles = roles.Where(x => x.CreateAt < roleParameters.MaxCreateAt);
            }
            */
        }

        private void PerformSearch(ref IQueryable<IdentityRole> roles, string searchTerm)
        {
            if (!roles.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            roles = roles.Where(x => x.Name.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}
