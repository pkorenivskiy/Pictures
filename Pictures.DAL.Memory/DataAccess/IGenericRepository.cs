using System;
using System.Linq;
using System.Linq.Expressions;

namespace Pictures.DAL.Memory.DataAccess
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById<TKey>(TKey id);
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        bool Add(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        bool Save();
    }
}
