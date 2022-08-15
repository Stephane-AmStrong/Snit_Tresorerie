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
    public class ActorRepository : RepositoryBase<Actor>, IActorRepository
    {
        private ISortHelper<Actor> _sortHelper;

        public ActorRepository(
            RepositoryContext repositoryContext,
            ISortHelper<Actor> sortHelper
            ) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<Actor>> GetPagedListAsync(ActorParameters actorParameters)
        {
            var actor = Enumerable.Empty<Actor>().AsQueryable();

            ApplyFilters(ref actor, actorParameters);

            PerformSearch(ref actor, actorParameters.SearchTerm);

            var sortedActors = _sortHelper.ApplySort(actor, actorParameters.OrderBy);

            return await Task.Run(() =>
                PagedList<Actor>.ToPagedList
                (
                    sortedActors,
                    actorParameters.PageNumber,
                    actorParameters.PageSize)
                );
        }

        public async Task<Actor> GetByIdAsync(Guid id)
        {
            return await BaseFindByCondition(actor => actor.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<Actor> GetDetailsAsync(Guid id)
        {
            return await BaseFindByCondition(actor => actor.Id.Equals(id))
                .Include(x=> x.Transactions)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistAsync(Actor actor)
        {
            return await BaseFindByCondition(x => x.FirstName == actor.LastName)
                .AnyAsync();
        }

        public async Task<int> CountAsync()
        {
            return await BaseFindAll().CountAsync();
        }

        public async Task CreateAsync(Actor actor)
        {
            await BaseCreateAsync(actor);
        }

        public async Task UpdateAsync(Actor actor)
        {
            await BaseUpdateAsync(actor);
        }

        public async Task UpdateAsync(IEnumerable<Actor> actor)
        {
            await BaseUpdateAsync(actor);
        }

        public async Task DeleteAsync(Actor actor)
        {
            await BaseDeleteAsync(actor);
        }

        #region ApplyFilters and PerformSearch Region
        private void ApplyFilters(ref IQueryable<Actor> actors, ActorParameters actorParameters)
        {
            actors = BaseFindAll();
            /*
            if (!string.IsNullOrWhiteSpace(actorParameters.AppUserId))
            {
                actors = actors.Where(x => x.AppUserId == actorParameters.AppUserId);
            }

            if (actorParameters.MinBirthday != null)
            {
                actors = actors.Where(x => x.Birthday >= actorParameters.MinBirthday);
            }

            if (actorParameters.MaxBirthday != null)
            {
                actors = actors.Where(x => x.Birthday < actorParameters.MaxBirthday);
            }

            if (actorParameters.MinCreateAt != null)
            {
                actors = actors.Where(x => x.CreateAt >= actorParameters.MinCreateAt);
            }
            */
        }

        private static void PerformSearch(ref IQueryable<Actor> actors, string searchTerm)
        {
            if (!actors.Any() || string.IsNullOrWhiteSpace(searchTerm)) return;

            actors = actors.Where(x => x.FirstName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase) 
                || x.FirstName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
                || x.LastName.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
                || x.BankAccount.Trim().Contains(searchTerm.Trim(), StringComparison.OrdinalIgnoreCase)
            );
        }

        #endregion

    }
}
