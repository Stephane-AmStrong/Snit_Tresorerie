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
    public class SiteRepository : RepositoryBase<Site>, ISiteRepository
    {
        private ISortHelper<Site> _sortHelper;

        public SiteRepository(
            RepositoryContext repositoryContext, 
            ISortHelper<Site> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Site>> GetPagedListAsync(SiteParameters siteParameters)
        {
            var sites = Enumerable.Empty<Site>().AsQueryable();

            ApplyFilters(ref sites, siteParameters);

            var sortedSites = _sortHelper.ApplySort(sites, siteParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Site>.ToPagedList
                (
                    sortedSites,
                    siteParameters.PageNumber,
                    siteParameters.PageSize)
                );
        }

        public async Task<Site> GetByIdAsync(Guid Id)
        {
            return await BaseFindByCondition(site => site.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Site> GetDetailsAsync(Guid Id)
        {
            return await BaseFindByCondition(site => site.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(Site site)
        {
            return await BaseFindByCondition(
                    x => x.Code == site.Code && 
                    x.Name == site.Name && 
                    x.Country == site.Country && 
                    x.City == site.City && 
                    x.Address == site.Address && 
                    x.Telephone1 == site.Telephone1 && 
                    x.Telephone2 == site.Telephone2 && 
                    x.Email == site.Email
                )
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(Site site)
        {
            await BaseCreateAsync(site);
        }

        public async Task UpdateAsync(Site site)
        {
            await BaseUpdateAsync(site);
        }

        public async Task DeleteAsync(Site site)
        {
            await BaseDeleteAsync(site);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Site> sites, SiteParameters siteParameters)
        {
            sites = BaseFindAll();
            /*
            if (!string.IsNullOrWhiteSpace(siteParameters.AppUserId))
            {
                sites = sites.Where(x => x.AppUserId == siteParameters.AppUserId);
            }

            if (siteParameters.MinBirthday != null)
            {
                sites = sites.Where(x => x.Birthday >= siteParameters.MinBirthday);
            }

            if (siteParameters.MaxBirthday != null)
            {
                sites = sites.Where(x => x.Birthday < siteParameters.MaxBirthday);
            }

            if (siteParameters.MinCreateAt != null)
            {
                sites = sites.Where(x => x.CreateAt >= siteParameters.MinCreateAt);
            }

            if (siteParameters.MaxCreateAt != null)
            {
                sites = sites.Where(x => x.CreateAt < siteParameters.MaxCreateAt);
            }
            */
        }

        #endregion

    }
}
