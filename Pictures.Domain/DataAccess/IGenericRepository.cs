using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pictures.Domain.DataAccess
{
    public interface IGenericRepository<TDto>
        where TDto : class
    {
        IEnumerable<TDto> GetAll();
        TDto GetById<TKey>(TKey id);
        IEnumerable<TDto> FindBy(Expression<Func<TDto, bool>> predicate);
        bool Add(TDto dto);
        bool Delete(TDto dto);
    }
}
