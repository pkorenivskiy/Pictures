using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pictures.DAL.Memory.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, new()
    {
        protected readonly List<TEntity> _dataContext;

        public GenericRepository()
        {
            _dataContext = new List<TEntity>();
            _dataContext.Add(new TEntity());
        }

        public bool Add(TEntity entity)
        {
            _dataContext.Add(entity);
            return true;
        }

        public bool Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dataContext.AsQueryable();
        }

        public TEntity GetById<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
