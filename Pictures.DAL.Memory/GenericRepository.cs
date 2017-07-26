using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pictures.DAL.Memory
{
    public class GenericRepository<TDto, TEntity> : Pictures.Domain.DataAccess.IGenericRepository<TDto>
        where TDto : class, new()
        where TEntity : class, new()
    {
        private readonly DataAccess.IGenericRepository<TEntity> _entityRepository;

        static GenericRepository()
        {
            AutoMapper.Mapper.Initialize(cfg => 
                {
                    cfg.CreateMap<Pictures.Dto.Picture, Pictures.DAL.Memory.Domain.Picture>();
                    cfg.CreateMap<Pictures.DAL.Memory.Domain.Picture, Pictures.Dto.Picture>();
                });
        }

        public GenericRepository(DataAccess.IGenericRepository<TEntity> repository)
        {
            _entityRepository = repository ?? throw new ArgumentNullException("EntityRepository");
        }

        public bool Add(TDto dto)
        {
            var entity = AutoMapper.Mapper.Map<TEntity>(dto);

            return _entityRepository.Add(entity);
        }

        public bool Delete(TDto dto)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDto> FindBy(Expression<Func<TDto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDto> GetAll()
        {
            var entities = _entityRepository.GetAll();

            foreach (var e in entities)
            {
                yield return AutoMapper.Mapper.Map<TDto>(e);
            }
        }

        public TDto GetById<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
