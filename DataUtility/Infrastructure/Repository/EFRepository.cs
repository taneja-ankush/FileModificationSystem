using DataUtility.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataUtility.Infrastructure.Repository
{
    public sealed class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly DbContext context;
        private DbSet<TEntity> entities;
        private Random _random;

        public object Context { get; private set; }

        public EFRepository(DbContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentException("context");
                }

                this.context = context;
                entities = context.Set<TEntity>();
                _random = new Random();
                Context = context;
            }
            catch (Exception exception)
            {
                // Log and throw exception further or Handle the exception.
                throw exception;
            }
            
        }

        /// <summary>
        /// GetAll TEntities
        /// </summary>
        /// <returns>List of TEntity</returns>
        public IEnumerable<TEntity> GetAll()
        {
            return entities.AsEnumerable();
        }

        /// <summary>
        /// Get all TEnities by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>List of TEntity</returns>
        public IEnumerable<TEntity> Get(int id)
        {
            return entities.Where(s => s.Id == id).AsEnumerable();
        }

        /// <summary>
        /// Add TEntity to XML file
        /// </summary>
        /// <param name="entity">TEntity</param>
        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entity.Id == 0)
            {
                entity.Id = _random.Next();
            }

            if (entity.CreatedDate == DateTime.MinValue || entity.CreatedDate == DateTime.MaxValue)
            {
                entity.CreatedDate = DateTime.Now;
            }

            entities.Add(entity);
            context.SaveChanges();
        }

        /// <summary>
        /// Update provided entity.
        /// </summary>
        /// <param name="entity">TEntity</param>
        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Delete entity by Id.
        /// </summary>
        /// <param name="Id">Id</param>
        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            entities.RemoveRange(Get(Id));
            context.SaveChanges();
        }
    }
}
