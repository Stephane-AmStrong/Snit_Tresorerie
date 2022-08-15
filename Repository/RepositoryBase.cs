using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> BaseFindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> BaseFindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task BaseCreateAsync(T entity)
        {
            await this.RepositoryContext.Set<T>().AddAsync(entity);
        }

        public async Task BaseCreateAsync(IEnumerable<T> entities)
        {
            await this.RepositoryContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task BaseUpdateAsync(T entity)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().Update(entity));
        }

        public async Task BaseUpdateAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().UpdateRange(entities));
        }

        public async Task BaseDeleteAsync(T entity)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().Remove(entity));
        }

        public async Task BaseDeleteAsync(IEnumerable<T> entities)
        {
            await Task.Run(() => this.RepositoryContext.Set<T>().RemoveRange(entities));
        }
    }
}
