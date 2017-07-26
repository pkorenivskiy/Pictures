using System;
using System.Collections.Generic;

namespace Pictures.Domain.DataAccess
{
    public class GenericService<TDto>
        where TDto : class
    {
        private readonly IGenericRepository<TDto> _repository;

        public GenericService(IGenericRepository<TDto> repository)
        {
            _repository = repository ?? throw new ArgumentNullException("Repository");
        }

        public IEnumerable<TDto> GetAll()
        {
            return _repository.GetAll();
        }

        public bool Add(TDto dto)
        {
            return _repository.Add(dto);
        }
    }
}
