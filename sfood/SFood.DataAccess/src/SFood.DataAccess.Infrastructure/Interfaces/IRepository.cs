using System.Collections.Generic;
using System.Threading.Tasks;
using SFood.DataAccess.Models.Infrastructure.Interfaces;

namespace SFood.DataAccess.Infrastructure.Interfaces
{
    public interface IRepository : IReadOnlyRepository
    {
        Task<TEntity> CreateAsync<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        Task CreateRangeAsync<TEntity>(List<TEntity> entities) 
            where TEntity : class, IEntity;

        TEntity Update<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void UpdateRange<TEntity>(List<TEntity> entities) 
            where TEntity : class, IEntity;

        void Delete<TEntity>(object id)
            where TEntity : class, IEntity;

        void Delete<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        void DeleteRange<TEntity>(List<TEntity> entities) 
            where TEntity : class, IEntity;

        void Save();

        Task SaveAsync();
    }
}