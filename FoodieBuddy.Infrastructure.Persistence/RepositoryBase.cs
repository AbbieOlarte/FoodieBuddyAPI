using FoodieBuddy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodieBuddy.Infrastructure.Persistence
{
    public class RepositoryBase<TEntity>: IRepository<TEntity> where TEntity:class
    {
        public readonly IFoodieBuddyDbContext context;

        public RepositoryBase(IFoodieBuddyDbContext context)
        {
            this.context = context;
        }

        public TEntity Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
            return entity;
        }

        public TEntity Retrieve(Guid id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public TEntity Update(Guid id, TEntity entity)
        {
            Retrieve(id);
            context.Set<TEntity>().Update(entity);
            context.SaveChanges();
            return entity;
        }

        public void Delete(Guid id)
        {
            var entity = this.Retrieve(id);
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public IEnumerable<TEntity> Retrieve()
        {
            return context.Set<TEntity>().ToList();
        }
    }
}
