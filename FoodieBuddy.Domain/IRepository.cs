using System;
using System.Collections.Generic;

namespace FoodieBuddy.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Retrieve(Guid id);
        TEntity Create(TEntity entity);
        TEntity Update(Guid id, TEntity entity);
        IEnumerable<TEntity> Retrieve();
        void Delete(Guid id);
    }
}