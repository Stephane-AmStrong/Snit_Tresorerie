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
    public class IntervenorRepository : RepositoryBase<Intervenor>, IIntervenorRepository
    {
        private ISortHelper<Intervenor> _sortHelper;

        public IntervenorRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Intervenor> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Intervenor>> GetPagedListAsync(IntervenorParameters intervenorParameters)
        {
            var intervenor = Enumerable.Empty<Intervenor>().AsQueryable();

            ApplyFilters(ref intervenor, intervenorParameters);

            PerformSearch(ref intervenor, intervenorParameters.SearchTerm);

            var sortedIntervenors = _sortHelper.ApplySort(intervenor, intervenorParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Intervenor>.ToPagedList
                (
                    sortedIntervenors,
                    intervenorParameters.PageNumber,
                    intervenorParameters.PageSize)
                );
        }

        public async Task<Intervenor> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(intervenor => intervenor.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Intervenor> GetDetailsAsync(Guid id)
        {
            return await BaseFindByCondition(intervenor => intervenor.Id.Equals(id))
                .Include(x=> x.Operations)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(Intervenor intervenor)
        {
            return await BaseFindByCondition(x => x.FirstName == intervenor.LastName)
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(Intervenor intervenor)
        {
            await BaseCreateAsync(intervenor);
        }

        public async Task UpdateAsync(Intervenor intervenor)
        {
            await BaseUpdateAsync(intervenor);
        }

        public async Task UpdateAsync(IEnumerable<Intervenor> intervenor)
        {
            await BaseUpdateAsync(intervenor);
        }

        public async Task DeleteAsync(Intervenor intervenor)
        {
            await BaseDeleteAsync(intervenor);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Intervenor> intervenors, IntervenorParameters intervenorParameters)
        {
            intervenors = BaseFindAll();
            
        }

        private static void PerformSearch(ref IQueryable<Intervenor> intervenors, string searchTerm)
        {
            if (!intervenors.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            intervenors = intervenors.Where(x => x.FirstName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) 
                || x.FirstName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
                || x.LastName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
                || x.BankAccount.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
            );
        }

        public Task<int> TestAujour()
        {
            throw new NotImplementedException();
        }

        public int TestNormal()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
