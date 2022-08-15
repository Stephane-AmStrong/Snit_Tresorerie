using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ISortHelper<AppUser> _sortHelper;

        public AppUserRepository(RepositoryContext repositoryContext, ISortHelper<AppUser> sortHelper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : base(repositoryContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<AppUser>> GetPagedListAsync(AppUserParameters appUserParameters)
        {
            var appUsers = Enumerable.Empty<AppUser>().AsQueryable();

            ApplyFilters(ref appUsers, appUserParameters);

            PerformSearch(ref appUsers, appUserParameters.SearchTerm);

            var sortedAppUsers = _sortHelper.ApplySort(appUsers, appUserParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<AppUser>.ToPagedList
                (
                    sortedAppUsers,
                    appUserParameters.PageNumber,
                    appUserParameters.PageSize)
                );
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _userManager.Users.Where(appUser => appUser.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetDetailsAsync(string id)
        {
            return await _userManager.Users.Where(appUser => appUser.Id.Equals(id))
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(AppUser appUser)
        {
            return await _userManager.Users.Where(x => x.FirstName == appUser.FirstName &&
                x.LastName == appUser.LastName
            ).AnyAsync();
        }

        public async Task CreateAsync(AppUser appUser)
        {
            await _userManager.CreateAsync(appUser);
        }

        public async Task UpdateAsync(AppUser appUser)
        {
            await _userManager.UpdateAsync(appUser);
        }

        public async Task DeleteAsync(AppUser appUser)
        {
            await _userManager.DeleteAsync(appUser);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<AppUser> appUsers, AppUserParameters appUserParameters)
        {
            appUsers = BaseFindAll();


            if (!string.IsNullOrEmpty(appUserParameters.WithRoleName))
            {
                var taskAppUsers = Task.Run(async () => await _userManager.GetUsersInRoleAsync(appUserParameters.WithRoleName));
                appUsers = taskAppUsers.Result.AsQueryable();
            }


            //if (appUserParameters.OfFormationLevelId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevelId == appUserParameters.OfFormationLevelId));
            //}

            //if (appUserParameters.OfFormationId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.FormationId == appUserParameters.OfFormationId));
            //}

            //if (appUserParameters.FromUniversityId != new Guid())
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.Formation.UniversityId == appUserParameters.FromUniversityId));
            //}

            //if (!string.IsNullOrEmpty(appUserParameters.ManagedByAppUserId))
            //{
            //    appUsers = appUsers.Where(x => x.Subscriptions.Any(x => x.FormationLevel.Formation.University.AppUserId == appUserParameters.ManagedByAppUserId));
            //}

        }

        private void PerformSearch(ref IQueryable<AppUser> appUsers, string searchTerm)
        {
            if (!appUsers.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            appUsers = appUsers.Where(x => x.FirstName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) ||
                x.LastName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
            );
        }

        #endregion
    }
}
