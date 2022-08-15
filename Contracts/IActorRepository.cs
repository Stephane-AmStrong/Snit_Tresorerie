
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IActorRepository
    {
        Task<PagedList<Actor>> GetPagedListAsync(ActorParameters actorParameters);

        Task<Actor> GetByIdAsync(Guid id);
        Task<Actor> GetDetailsAsync(Guid id);
        Task<bool> ExistAsync(Actor actor);
        Task<int> CountAsync();

        Task CreateAsync(Actor actor);
        Task UpdateAsync(Actor actor);
        Task DeleteAsync(Actor actor);

    }
}
