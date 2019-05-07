using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.DataAccess.Infrastructure.Implements
{
    public class EntityFrameworkRepository<TContext> : EntityFrameworkReadOnlyRepository<TContext>, IRepository
        where TContext : DbContext
    {
        public EntityFrameworkRepository(TContext context)
            : base(context)
        {
        }

        public async virtual Task<TEntity> CreateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var hasCreatedTime = typeof(IHasCreatedTime).IsAssignableFrom(typeof(TEntity));
            if (hasCreatedTime && ((IHasCreatedTime)entity).CreatedTime == default(DateTime))
            {
                ((IHasCreatedTime)entity).CreatedTime = DateTime.UtcNow;
            }
            var entityEntry = await context.Set<TEntity>().AddAsync(entity);
            return entityEntry.Entity;
        }

        public async virtual Task CreateRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class, IEntity
        {
            var hasCreatedTime = typeof(IHasCreatedTime).IsAssignableFrom(typeof(TEntity));
            if (hasCreatedTime)
            {
                entities.ForEach(entity => {
                    if (((IHasCreatedTime)entity).CreatedTime == default(DateTime))
                    {
                        ((IHasCreatedTime)entity).CreatedTime = DateTime.UtcNow;
                    }                    
                });
            }
            await context.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual TEntity Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var hasModifiedTime = typeof(IHasModifiedTime).IsAssignableFrom(typeof(TEntity));
            if (hasModifiedTime)
            {
                ((IHasModifiedTime)entity).LastModifiedTime = DateTime.UtcNow;
            }

            return context.Set<TEntity>().Update(entity).Entity;
        }

        public virtual void UpdateRange<TEntity>(List<TEntity> entities) where TEntity : class, IEntity
        {
            var hasModifiedTime = typeof(IHasModifiedTime).IsAssignableFrom(typeof(TEntity));
            if (hasModifiedTime)
            {
                entities.ForEach(entity => {
                    ((IHasModifiedTime)entity).LastModifiedTime = DateTime.UtcNow;
                });
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public virtual void Delete<TEntity>(object id) where TEntity : class, IEntity
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            var isSoftDelete = typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));
            if (isSoftDelete)
            {
                ((ISoftDelete)entity).IsDeleted = true;
                Update(entity);
            }
            else
            {
                Delete(entity);
            }            
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            context.Set<TEntity>().Remove(entity);
        }

        public virtual void DeleteRange<TEntity>(List<TEntity> entities) where TEntity : class, IEntity
        {
            var isSoftDelete = typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));
            if (isSoftDelete)
            {
                entities.ForEach(entity => {
                    ((ISoftDelete)entity).IsDeleted = true;
                });
                UpdateRange(entities);
            }
            else
            {
                context.Set<TEntity>().RemoveRange(entities);
            }            
        }

        public virtual void Save()
        {            
            context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}